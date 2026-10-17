using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Step 4 of the Add Pet dialog: "Choose a Grooming Service". Shown after the vaccine step. The cards come from
/// dbo.Services (price and duration are whatever the admin set in the service catalog), and the step bar shows the
/// duration of the chosen card under "Service", e.g. "(30 mins)". Built in code, like the vaccine step.
/// </summary>
internal sealed partial class AddPetForm
{
    private Panel? _servicePanel;
    private Panel? _serviceScroll;
    private readonly List<ServiceCard> _serviceCards = new();
    private ServiceChoice? _chosenServiceChoice;

    private bool ServiceStepVisible => _servicePanel is { Visible: true };

    // ------------------------------------------------------------------ show / hide

    private void ShowServiceStep()
    {
        List<ServiceChoice> choices;
        try { choices = EmployeeData.GetServiceChoices(); }
        catch (SqlException) { choices = new List<ServiceChoice>(); }

        if (choices.Count == 0)
        {
            // Nothing to show (no services set up, or the database can't be reached): keep the old behaviour and go on
            // to the appointment request, where the service is chosen from a list.
            FinishVaccineStepWithoutService();
            return;
        }

        EnsureServicePanel();
        HideVaccineStep();   // hides the vaccine panel and resets the title position; the step dots are set below

        BuildServiceCards(choices);

        contactPanel.Visible = false;
        selectPanel.Visible = false;
        _servicePanel!.Visible = true;

        titleLabel.Text = "Choose a Grooming Service";
        subtitleLabel.Text = "Select a service package for your pet.";

        stepContact.State = StepState.Done;
        stepSelect.State = StepState.Done;
        stepVaccine.State = StepState.Done;
        stepGrooming.State = StepState.Active;

        // preselect the service picked in Pet Details when it is on offer, otherwise the first card (as in the design)
        var preselect = _serviceCards.FirstOrDefault(c => string.Equals(((ServiceChoice)c.Tag!).Name, ChosenService, StringComparison.OrdinalIgnoreCase))
                        ?? _serviceCards[0];
        ChooseService(preselect);

        continueButton.Text = "Continue  \u2192";
        continueButton.Visible = true;
    }

    private void HideServiceStep()
    {
        if (_servicePanel is not null) _servicePanel.Visible = false;
        stepGrooming.State = StepState.Pending;
        stepGrooming.Caption = "";
    }

    // ------------------------------------------------------------------ the cards

    private void EnsureServicePanel()
    {
        if (_servicePanel is not null) return;

        _servicePanel = new Panel { Location = new Point(0, 164), Size = new Size(900, 318), BackColor = Theme.Bg, Visible = false };

        // two columns, as many rows as there are services; it scrolls when there are more than four
        _serviceScroll = new Panel { Location = new Point(40, 0), Size = new Size(836, 256), BackColor = Theme.Bg, AutoScroll = true };

        var note = new Label
        {
            Text = "Prices may vary depending on your pet's size and condition.",
            Location = new Point(40, 268),
            Size = new Size(820, 38),
            BackColor = Color.FromArgb(222, 235, 254),
            ForeColor = Color.FromArgb(30, 64, 175),
            Font = Theme.Face(9f),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(14, 0, 0, 0),
        };

        _servicePanel.Controls.Add(_serviceScroll);
        _servicePanel.Controls.Add(note);
        ContentHost.Controls.Add(_servicePanel);
    }

    private void BuildServiceCards(List<ServiceChoice> choices)
    {
        _serviceScroll!.SuspendLayout();
        foreach (var old in _serviceCards) { _serviceScroll.Controls.Remove(old); old.Dispose(); }
        _serviceCards.Clear();
        _serviceScroll.AutoScrollPosition = new Point(0, 0);

        const int w = 398, h = 120, gapX = 20, gapY = 16;
        for (int i = 0; i < choices.Count; i++)
        {
            var s = choices[i];
            var card = new ServiceCard
            {
                Text = s.Name,
                Price = Money.Format(s.Price),
                Duration = "~ " + DurationText(s.DurationMinutes),
                Glyph = GlyphFor(s.Name),
                Tag = s,
                BackColor = Theme.Bg,
                Location = new Point((i % 2) * (w + gapX), (i / 2) * (h + gapY)),
                Size = new Size(w, h),
            };
            card.Click += (_, _) => ChooseService(card);
            _serviceCards.Add(card);
            _serviceScroll.Controls.Add(card);
        }
        _serviceScroll.ResumeLayout();
    }

    private void ChooseService(ServiceCard card)
    {
        foreach (var c in _serviceCards) c.Selected = c == card;
        _chosenServiceChoice = (ServiceChoice)card.Tag!;
        stepGrooming.Caption = "(" + DurationText(_chosenServiceChoice.DurationMinutes) + ")";   // the "(30 mins)" under Service
        continueButton.Enabled = true;
    }

    private static string DurationText(int minutes)
        => minutes >= 60 && minutes % 60 == 0 ? $"{minutes / 60} hr" : $"{minutes} mins";

    private static string GlyphFor(string name)
    {
        string n = name.ToLowerInvariant();
        return n.Contains("groom") || n.Contains("cut") || n.Contains("trim") ? "\uE8C6" : "\uEB51";   // scissors, otherwise a heart
    }

    // ------------------------------------------------------------------ moving on / back

    /// <summary>Continue on step 4: the appointment request opens with this service already selected.</summary>
    private void FinishServiceStep()
    {
        if (_chosenServiceChoice is null) return;
        ChosenService = _chosenServiceChoice.Name;
        ChosenPetId = _vaccinePetId;
        DialogResult = DialogResult.OK;
    }

    /// <summary>No service cards available: finish the way the flow did before step 4 existed.</summary>
    private void FinishVaccineStepWithoutService()
    {
        ChosenPetId = _vaccinePetId;
        DialogResult = DialogResult.OK;
    }

    /// <summary>Back on step 4 returns to the vaccine step (a card already scanned stays scanned).</summary>
    private void BackToVaccineStep()
    {
        bool scanned = _vaccineDone;
        HideServiceStep();
        ShowVaccineStep(_vaccinePetId, _vaccinePetName, isNew: false, _vaccineState);
        if (scanned)
        {
            _vaccineDone = true;
            _vaccineStatus!.Text = "Vaccine card scanned.";
            _vaccineStatus.ForeColor = Theme.Green;
        }
    }
}

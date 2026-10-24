using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Step 4 of the Add Pet dialog: "Choose a Grooming Service" (left) and "Select a Schedule" (right). Shown after the vaccine step.
/// The tiles come from dbo.Services (price and duration are whatever the admin set in the service catalog). The whole page is one
/// full-size panel (<see cref="ServiceStepCanvas"/>) laid out for the 900 x 540 design and scaled with the rest of the form, so it
/// fits 1536 x 864 and 1920 x 1080 alike. Built in code, like the vaccine step.
/// </summary>
internal sealed partial class AddPetForm
{
    private ServiceStepCanvas? _servicePanel;
    private Panel? _serviceScroll;
    private InfoBar? _serviceNote;
    private InfoBar? _scheduleInfo;
    private readonly List<ServiceTile> _serviceCards = new();
    private ServiceChoice? _chosenServiceChoice;
    private readonly List<DayCard> _dayChips = new();

    // service tiles: four across, laid out in the 900 x 540 design
    private const int TileW = ServiceTile.DesignWidth, TileH = ServiceTile.DesignHeight, TileGap = 9;

    /// <summary>The days the customer can come in, e.g. "Tue,Wed". Staff pick the real date and time, but only on these days.</summary>
    public string PreferredDays { get; private set; } = "";

    private bool ServiceStepVisible => _servicePanel is { Visible: true };

    // ------------------------------------------------------------------ show / hide

    private void ShowServiceStep() => PageFade.Swap(ContentHost, ShowServiceStepCore);

    private void ShowServiceStepCore()
    {
        List<ServiceChoice> choices;
        decimal? weightKg = null;
        try
        {
            choices = ServicePricing.GetChoices(_vaccinePetId);   // priced for this pet's weight and species
            weightKg = ServicePricing.GetPet(_vaccinePetId)?.WeightKg;
        }
        catch (SqlException) { choices = new List<ServiceChoice>(); }

        if (choices.Count == 0)
        {
            // Nothing to show (no services set up, or the database can't be reached): keep the old behaviour and go on
            // to the appointment request, where the service is chosen from a list.
            FinishVaccineStepWithoutService();
            return;
        }

        EnsureServicePanel();
        _serviceNote!.Text = weightKg is decimal kg && kg > 0
            ? $"Prices are based on your pet's weight ({kg:0.##} kg)."
            : "Prices may vary depending on your pet's size and condition.";
        foreach (var chip in _dayChips) chip.Checked = false;   // a fresh visit: no days picked yet
        UpdateScheduleInfo();
        HideVaccineStep();   // hides the vaccine panel and resets the title position; the step dots are set below

        BuildServiceCards(choices);

        contactPanel.Visible = false;
        selectPanel.Visible = false;

        titleLabel.Text = "Choose a Grooming Service";
        subtitleLabel.Text = "Select a service package for your pet.";

        stepContact.State = StepState.Done;
        stepSelect.State = StepState.Done;
        stepVaccine.State = StepState.Done;
        stepGrooming.State = StepState.Active;

        // the page draws its own header, so it takes the step names from the form's step bar
        _servicePanel!.StepNames = new[] { stepContact.Text, stepSelect.Text, stepVaccine.Text, stepGrooming.Text, stepTicket.Text };
        _servicePanel.ActiveStep = 3;
        _servicePanel.Visible = true;
        _servicePanel.BringToFront();   // covers the shared header, title and buttons of the other steps

        // nothing was picked yet (Pet Details no longer asks for a service): start on the first tile
        var preselect = _serviceCards[0];
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

    // ------------------------------------------------------------------ the page

    private void EnsureServicePanel()
    {
        if (_servicePanel is not null) return;

        // Everything below is written in the 900 x 540 design; the content scaler grows it when the panel joins the form.
        var page = new ServiceStepCanvas { Location = new Point(0, 0), Visible = false };

        // ---- left: the service tiles (four across; scrolls when there are more than three rows)
        int gridW = TileW * 4 + TileGap * 3;
        _serviceScroll = new Panel
        {
            Location = new Point(46, 148),
            Size = new Size(gridW + 16, TileH * 3 + TileGap * 2),
            BackColor = Color.White,
            AutoScroll = true,
            TabStop = false,
        };
        page.Controls.Add(_serviceScroll);

        _serviceNote = new InfoBar
        {
            Location = new Point(46, 478),
            Size = new Size(gridW, 26),
            DesignWidth = gridW,
            DesignHeight = 26,
            Text = "Prices may vary depending on your pet's size and condition.",
        };
        page.Controls.Add(_serviceNote);

        // ---- right: preferred days (the customer picks which days of the week suit them, no exact date or time)
        const int dayX = 575, row1Y = 170, row2Y = 224, dayH = 42;
        DayOfWeek[] order = { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };
        for (int i = 0; i < order.Length; i++)
        {
            var day = order[i];
            bool firstRow = i < 4;
            var chip = new DayCard
            {
                Text = PreferredDaysText.ShortName(day),
                LongName = day.ToString(),
                Tag = day,
                Location = firstRow ? new Point(dayX + i * 68, row1Y) : new Point(dayX + (i - 4) * 91, row2Y),
                Size = firstRow ? new Size(60, dayH) : new Size(82, dayH),
            };
            chip.CheckedChanged += (_, _) => UpdateScheduleInfo();
            _dayChips.Add(chip);
            page.Controls.Add(chip);
        }

        _scheduleInfo = new InfoBar
        {
            Location = new Point(dayX, 277),
            Size = new Size(264, 38),
            DesignWidth = 264,
            DesignHeight = 38,
            Text = "Pick at least one day.",
            Detail = "You can choose more than one day if you're available.",
        };
        page.Controls.Add(_scheduleInfo);

        // ---- Back / Continue (they do what the form's own Back / Continue buttons do)
        var back = new StepButton { Text = "Back", Location = new Point(560, 472), Size = new Size(76, 32) };
        back.Click += (_, _) => GoBack();
        page.Controls.Add(back);

        var next = new StepButton { Text = "Continue", Primary = true, Location = new Point(748, 472), Size = new Size(106, 32) };
        next.Click += (_, _) => ContinueWithPet();
        page.Controls.Add(next);

        _servicePanel = page;
        ContentHost.Controls.Add(page);   // added last, so it is scaled once, with everything inside it
    }

    /// <summary>The note under the day cards: asks for a day until one is picked.</summary>
    private void UpdateScheduleInfo()
    {
        if (_scheduleInfo is null) return;
        bool any = _dayChips.Any(c => c.Checked);
        _scheduleInfo.Text = any ? "Selected at least one day." : "Pick at least one day.";
        _scheduleInfo.Emphasis = any;
    }

    private void BuildServiceCards(List<ServiceChoice> choices)
    {
        _serviceScroll!.SuspendLayout();
        foreach (var old in _serviceCards) { _serviceScroll.Controls.Remove(old); old.Dispose(); }
        _serviceCards.Clear();
        _serviceScroll.AutoScrollPosition = new Point(0, 0);

        for (int i = 0; i < choices.Count; i++)
        {
            var s = choices[i];
            var card = new ServiceTile
            {
                Text = s.Name,
                Price = Money.Format(s.Price),
                Duration = "~ " + DurationText(s.DurationMinutes),
                IconKey = SvcLook.IconKeyFor(s.Name),
                Tag = s,
                BackColor = Color.White,
                Location = new Point((i % 4) * (TileW + TileGap), (i / 4) * (TileH + TileGap)),
                Size = new Size(TileW, TileH),
            };
            card.Click += (_, _) => ChooseService(card);
            _serviceCards.Add(card);
            _serviceScroll.Controls.Add(card);
        }
        _serviceScroll.ResumeLayout();
    }

    private void ChooseService(ServiceTile card)
    {
        foreach (var c in _serviceCards) c.Selected = c == card;
        _chosenServiceChoice = (ServiceChoice)card.Tag!;
        stepGrooming.Caption = "(" + DurationText(_chosenServiceChoice.DurationMinutes) + ")";   // the "(30 mins)" under Service
        continueButton.Enabled = true;
    }

    /// <summary>"30 mins", "1 hr", "1 hr 30 mins", "2 hr".</summary>
    private static string DurationText(int minutes)
    {
        if (minutes < 60) return $"{minutes} mins";
        int h = minutes / 60, m = minutes % 60;
        return m == 0 ? $"{h} hr" : $"{h} hr {m} mins";
    }

    // ------------------------------------------------------------------ moving on / back

    /// <summary>Continue on step 4: the appointment request opens with this service already selected.</summary>
    private void FinishServiceStep()
    {
        if (_chosenServiceChoice is null) return;

        var days = _dayChips.Where(c => c.Checked).Select(c => (DayOfWeek)c.Tag!).ToList();
        if (days.Count == 0)
        {
            Msg.Show(this, "Please pick at least one preferred day (for example Tuesday and Wednesday).\n\nYou don't choose the exact date or time - our staff will set it on one of your preferred days.",
                "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        PreferredDays = PreferredDaysText.Join(days);
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
            HideVaccineWarning();
        }
    }
}
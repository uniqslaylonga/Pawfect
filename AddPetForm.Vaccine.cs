using System.Drawing.Drawing2D;
using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Step 3 of the Add Pet dialog: "Scan Your Pet's Vaccine Card". Shown after the Pet Details pop-up is saved (new pet, or an
/// existing pet that has no vaccine card on file yet). It is built in code so the Visual Studio designer file stays untouched.
/// </summary>
internal sealed partial class AddPetForm
{
    private Panel? _vaccinePanel;
    private Label? _vaccineBannerText;
    private Label? _vaccineStatus;
    private Button? _skipButton;
    private int _vaccinePetId;
    private string _vaccinePetName = "";
    private bool _vaccineDone;

    /// <summary>The service picked in the Pet Details pop-up; the appointment request opens with it selected.</summary>
    public string? ChosenService { get; private set; }

    private bool VaccineStepVisible => _vaccinePanel is { Visible: true };

    // ------------------------------------------------------------------ the Pet Details pop-up comes first

    /// <summary>Add New Pet tab or "New Pet?" tile clicked: Pet Details (empty), then step 3.</summary>
    private void OpenNewPetFlow() => OpenPetDetails(null);

    /// <summary>An existing pet was clicked (or Continue pressed): Pet Details with its data, then step 3 if it has no card yet.</summary>
    private void OpenExistingPetFlow(PetCard card)
    {
        if (card.Tag is not int petId) return;
        PetProfile? pet = null;
        try { pet = PetProfiles.GetForCustomer(_customerId).FirstOrDefault(p => p.Id == petId); }
        catch (SqlException ex) { petErrorLabel.Text = "Couldn't load that pet. " + ex.Message; return; }
        if (pet is null) return;
        OpenPetDetails(pet);
    }

    private void OpenPetDetails(PetProfile? pet)
    {
        using var dialog = new NewPetDetailsDialog(_customerId, pet);
        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        ChosenService = dialog.Service;
        if (pet is null) CreatedPetId = dialog.PetId;

        if (pet is not null && PetHasVaccineCard(dialog.PetId))
        {
            ChosenPetId = dialog.PetId;   // already has a card on file: nothing to scan
            DialogResult = DialogResult.OK;
            return;
        }
        ShowVaccineStep(dialog.PetId, dialog.PetName, isNew: pet is null);
    }

    // ------------------------------------------------------------------ step 3: show / hide

    private void ShowVaccineStep(int petId, string petName, bool isNew)
    {
        EnsureVaccinePanel();
        _vaccinePetId = petId;
        _vaccinePetName = petName;
        _vaccineDone = false;

        _vaccineBannerText!.Text = isNew
            ? "This pet is new to our system. A valid vaccine card is required before proceeding to grooming."
            : $"{petName} has no vaccine card on file. A valid vaccine card is required before proceeding to grooming.";
        _vaccineStatus!.Text = "Click the frame to scan the vaccine card.";
        _vaccineStatus.ForeColor = Theme.Muted;

        contactPanel.Visible = false;
        selectPanel.Visible = false;
        _vaccinePanel!.Visible = true;
        _skipButton!.Visible = true;

        titleLabel.Text = "Scan Your Pet's Vaccine Card";
        subtitleLabel.Text = "Please scan the vaccine card for verification.";
        stepContact.State = StepState.Done;
        stepSelect.State = StepState.Done;
        stepVaccine.State = StepState.Active;

        continueButton.Text = "Continue  \u2192";
        continueButton.Enabled = true;
        continueButton.Visible = true;
    }

    private void HideVaccineStep()
    {
        if (_vaccinePanel is not null) _vaccinePanel.Visible = false;
        if (_skipButton is not null) _skipButton.Visible = false;
        stepVaccine.State = StepState.Pending;
    }

    // ------------------------------------------------------------------ actions

    private bool PetHasVaccineCard(int petId)
    {
        try { return PetProfiles.GetPictures(petId).Card is { Length: > 0 }; }
        catch (SqlException) { return false; }   // can't tell: ask for the card rather than let it through
    }

    private void ScanVaccineCard()
    {
        try
        {
            using var form = new VaccinationScanForm(_customerId, _ownerName, _vaccinePetId);
            if (form.ShowDialog(this) != DialogResult.OK) return;
        }
        catch (SqlException ex)
        {
            Msg.Show(this, "Couldn't open the scanner.\n\n" + ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        _vaccineDone = true;
        _vaccineStatus!.Text = "\u2713  Vaccine card verified and saved. Press Continue.";
        _vaccineStatus.ForeColor = Theme.Green;
    }

    private void SkipVaccineScan()
    {
        var answer = Msg.Show(this,
            $"Skip scanning?\n\nThe shop can't confirm {_vaccinePetName}'s vaccinations until a card is on file, so the appointment may be held until you bring one.",
            "Pawfect", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (answer == DialogResult.Yes) FinishVaccineStep();
    }

    /// <summary>Done with the vaccine step: the caller opens the appointment request for this pet.</summary>
    private void FinishVaccineStep()
    {
        ChosenPetId = _vaccinePetId;
        DialogResult = DialogResult.OK;
    }

    // ------------------------------------------------------------------ the panel

    private void EnsureVaccinePanel()
    {
        if (_vaccinePanel is not null) return;

        _vaccinePanel = new Panel { Location = new Point(0, 150), Size = new Size(900, 330), BackColor = Theme.Bg, Visible = false };

        // warning banner
        var banner = new Panel { Location = new Point(40, 0), Size = new Size(820, 48), BackColor = Theme.Bg };
        banner.Paint += (_, e) =>
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var r = new Rectangle(0, 0, banner.Width - 1, banner.Height - 1);
            using (var path = RoundedRect(r, 10))
            {
                using var fill = new SolidBrush(Color.FromArgb(255, 251, 235));
                using var pen = new Pen(Color.FromArgb(253, 230, 138));
                g.FillPath(fill, path);
                g.DrawPath(pen, path);
            }
            var dot = new Rectangle(14, 10, 28, 28);
            using (var orange = new SolidBrush(Color.FromArgb(251, 191, 36))) g.FillEllipse(orange, dot);
            TextRenderer.DrawText(g, "!", Theme.Face(11f, FontStyle.Bold), dot, Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        };
        _vaccineBannerText = new Label
        {
            Location = new Point(54, 4), Size = new Size(752, 40), BackColor = Color.Transparent,
            ForeColor = Color.FromArgb(146, 64, 14), Font = Theme.Face(9.5f, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft,
        };
        banner.Controls.Add(_vaccineBannerText);

        // scan frame (click to scan)
        var frame = new Panel { Location = new Point(40, 64), Size = new Size(500, 256), BackColor = Theme.Bg, Cursor = Cursors.Hand };
        frame.Paint += PaintScanFrame;
        frame.Click += (_, _) => ScanVaccineCard();
        _vaccineStatus = new Label
        {
            Dock = DockStyle.Bottom, Height = 34, BackColor = Color.Transparent, ForeColor = Theme.Muted,
            Font = Theme.Face(9f, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter, Cursor = Cursors.Hand,
        };
        _vaccineStatus.Click += (_, _) => ScanVaccineCard();
        frame.Controls.Add(_vaccineStatus);

        // instructions card
        var info = new Panel { Location = new Point(556, 64), Size = new Size(304, 256), BackColor = Theme.Bg };
        info.Paint += (_, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var path = RoundedRect(new Rectangle(0, 0, info.Width - 1, info.Height - 1), 18);
            using var fill = new SolidBrush(Theme.PrimarySoft);
            using var pen = new Pen(Theme.Border);
            e.Graphics.FillPath(fill, path);
            e.Graphics.DrawPath(pen, path);
        };
        var infoIcon = new Label
        {
            Location = new Point(22, 100), Size = new Size(26, 26), BackColor = Color.Transparent, ForeColor = Theme.Primary,
            Font = Theme.GlyphFont(13f), Text = "\uE946", TextAlign = ContentAlignment.MiddleCenter,
        };
        var infoText = new Label
        {
            Location = new Point(54, 60), Size = new Size(226, 136), BackColor = Color.Transparent, ForeColor = Theme.TextSoft,
            Font = Theme.Face(9.5f, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft,
            Text = "Place the vaccine card in the frame and make sure it's clear and readable.",
        };
        info.Controls.Add(infoIcon);
        info.Controls.Add(infoText);

        _vaccinePanel.Controls.Add(banner);
        _vaccinePanel.Controls.Add(frame);
        _vaccinePanel.Controls.Add(info);
        Controls.Add(_vaccinePanel);

        // "Skip Scanning" sits beside Continue on the form itself
        _skipButton = new Button
        {
            Text = "Skip Scanning", Location = new Point(586, 486), Size = new Size(130, 42), FlatStyle = FlatStyle.Flat,
            BackColor = Color.White, ForeColor = Theme.Primary, Font = Theme.Face(9.5f, FontStyle.Bold), Cursor = Cursors.Hand,
            Visible = false, UseVisualStyleBackColor = false,
        };
        _skipButton.FlatAppearance.BorderColor = Theme.Border;
        _skipButton.Click += (_, _) => SkipVaccineScan();
        Controls.Add(_skipButton);
    }

    private static void PaintScanFrame(object? sender, PaintEventArgs e)
    {
        var p = (Panel)sender!;
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var path = RoundedRect(new Rectangle(0, 0, p.Width - 1, p.Height - 1), 18))
        {
            using var fill = new SolidBrush(Theme.PrimarySoft);
            using var pen = new Pen(Theme.Border);
            g.FillPath(fill, path);
            g.DrawPath(pen, path);
        }

        // corner brackets
        using (var br = new Pen(Theme.Primary, 3f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
        {
            int m = 16, len = 26, w = p.Width - 1, h = p.Height - 1;
            g.DrawLines(br, new[] { new Point(m, m + len), new Point(m, m), new Point(m + len, m) });
            g.DrawLines(br, new[] { new Point(w - m - len, m), new Point(w - m, m), new Point(w - m, m + len) });
            g.DrawLines(br, new[] { new Point(m, h - m - len), new Point(m, h - m), new Point(m + len, h - m) });
            g.DrawLines(br, new[] { new Point(w - m - len, h - m), new Point(w - m, h - m), new Point(w - m, h - m - len) });
        }

        // camera tile
        var tile = new Rectangle(p.Width / 2 - 28, 28, 56, 56);
        using (var path = RoundedRect(tile, 14))
        {
            using var white = new SolidBrush(Color.White);
            using var pen = new Pen(Theme.Border);
            g.FillPath(white, path);
            g.DrawPath(pen, path);
        }
        TextRenderer.DrawText(g, "\uE722", Theme.GlyphFont(20f), tile, Theme.Primary,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);

        // little vaccination-record card
        var card = new Rectangle(p.Width / 2 - 110, 100, 220, 92);
        using (var path = RoundedRect(card, 10))
        {
            using var white = new SolidBrush(Color.White);
            using var pen = new Pen(Theme.Border);
            g.FillPath(white, path);
            g.DrawPath(pen, path);
        }
        TextRenderer.DrawText(g, "VACCINATION RECORD", Theme.Face(7f, FontStyle.Bold),
            new Point(card.X + 70, card.Y + 12), Theme.Primary, TextFormatFlags.NoPadding);
        using (var bar = new SolidBrush(Color.FromArgb(207, 224, 248)))
        {
            g.FillRectangle(bar, card.X + 70, card.Y + 34, 130, 6);
            g.FillRectangle(bar, card.X + 70, card.Y + 48, 100, 6);
            g.FillRectangle(bar, card.X + 70, card.Y + 62, 70, 6);
        }
        using (var paw = new SolidBrush(Theme.Primary)) g.FillEllipse(paw, card.X + 20, card.Y + 38, 30, 24);
    }

    private static GraphicsPath RoundedRect(Rectangle r, int radius)
    {
        int d = radius * 2;
        var path = new GraphicsPath();
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}

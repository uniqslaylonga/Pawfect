using System.Drawing.Drawing2D;
using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Step 3 of the Add Pet dialog: the vaccine card. Shown after the Pet Details pop-up is saved. It has three looks:
/// <list type="bullet">
/// <item><b>Missing</b> - new pet, or no card on file yet: amber banner, "Scan Your Pet's Vaccine Card".</item>
/// <item><b>Expired</b> - a card is on file but none of its vaccinations is still in date: red banner, "Scan Updated Vaccine Card".</item>
/// <item><b>Valid</b> - a card is on file and still good: green banner and "No need to scan!".</item>
/// </list>
/// It is built in code so the Visual Studio designer file stays untouched.
/// </summary>
internal sealed partial class AddPetForm
{
    private enum VaccineState { Missing, Expired, Valid }

    private enum BannerKind { Warning, Danger, Success }

    // Everything on this step sits in one panel that starts under the step dots (form y = 84).
    private const int PanelTop = 84;

    private Panel? _vaccinePanel;
    private Panel? _vaccineBanner;
    private Label? _vaccineBannerText;
    private Panel? _scanGroup;
    private Panel? _validGroup;
    private Label? _vaccineInfoText;
    private Label? _vaccineStatus;
    private Button? _skipButton;
    private int _vaccinePetId;
    private string _vaccinePetName = "";
    private bool _vaccineDone;
    private VaccineState _vaccineState;
    private BannerKind _bannerKind;

    /// <summary>The service picked in the Pet Details pop-up; the appointment request opens with it selected.</summary>
    public string? ChosenService { get; private set; }

    private bool VaccineStepVisible => _vaccinePanel is { Visible: true };

    // ------------------------------------------------------------------ the Pet Details pop-up comes first

    /// <summary>Add New Pet tab or "New Pet?" tile clicked: Pet Details (empty), then step 3.</summary>
    private void OpenNewPetFlow() => OpenPetDetails(null);

    /// <summary>An existing pet was clicked (or Continue pressed): Pet Details with its data, then step 3.</summary>
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

        // A pet with a card on file is either fine ("No need to scan!") or its vaccinations have lapsed (scan a new card).
        var state = VaccineState.Missing;
        if (pet is not null && PetHasVaccineCard(dialog.PetId))
        {
            try { state = GetVaccineState(dialog.PetId); }
            catch (SqlException) { state = VaccineState.Missing; }   // can't tell: ask for the card rather than let it through
        }
        ShowVaccineStep(dialog.PetId, dialog.PetName, isNew: pet is null, state);
    }

    // ------------------------------------------------------------------ step 3: show / hide

    private void ShowVaccineStep(int petId, string petName, bool isNew, VaccineState state = VaccineState.Missing)
        => PageFade.Swap(ContentHost, () => ShowVaccineStepCore(petId, petName, isNew, state));

    private void ShowVaccineStepCore(int petId, string petName, bool isNew, VaccineState state)
    {
        HideServiceStep();
        EnsureVaccinePanel();
        _vaccinePetId = petId;
        _vaccinePetName = petName;
        _vaccineDone = false;
        _vaccineState = state;

        bool valid = state == VaccineState.Valid;
        _scanGroup!.Visible = !valid;
        _validGroup!.Visible = valid;
        _skipButton!.Visible = !valid;
        titleLabel.Visible = !valid;       // the "No need to scan!" look has its own heading
        subtitleLabel.Visible = !valid;

        switch (state)
        {
            case VaccineState.Valid:
                SetBanner(BannerKind.Success, "Your pet's vaccine record is already on file.");
                break;

            case VaccineState.Expired:
                SetBanner(BannerKind.Danger, "The vaccine record has expired.");
                titleLabel.Text = "Scan Updated Vaccine Card";
                subtitleLabel.Text = "Please scan the latest vaccine card to continue.";
                _vaccineInfoText!.Text = "Use the updated vaccine card with the current date.";
                break;

            default:
                SetBanner(BannerKind.Warning, isNew
                    ? "This pet is new to our system. A valid vaccine card is required before proceeding to grooming."
                    : $"{PetNameRules.Short(petName)} has no vaccine card on file. A valid vaccine card is required before proceeding to grooming.");
                titleLabel.Text = "Scan Your Pet's Vaccine Card";
                subtitleLabel.Text = "Please scan the vaccine card for verification.";
                _vaccineInfoText!.Text = "Place the vaccine card in the frame and make sure it's clear and readable.";
                break;
        }

        if (!valid)
        {
            // the banner comes first, so the title moves down under it
            titleLabel.Location = Px(40, 134);
            subtitleLabel.Location = Px(40, 172);
            _vaccineStatus!.Text = "Click the frame to scan the vaccine card.";
            _vaccineStatus.ForeColor = Theme.Muted;
        }

        contactPanel.Visible = false;
        selectPanel.Visible = false;
        _vaccinePanel!.Visible = true;

        stepContact.State = StepState.Done;
        stepSelect.State = StepState.Done;
        stepVaccine.State = StepState.Active;

        continueButton.Text = "Continue  \u2192";
        continueButton.Enabled = true;
        continueButton.Visible = true;
    }

    private void HideVaccineStep()
    {
        HideServiceStep();
        if (_vaccinePanel is not null) _vaccinePanel.Visible = false;
        if (_skipButton is not null) _skipButton.Visible = false;
        stepVaccine.State = StepState.Pending;

        // the other steps use the title where the designer put it
        titleLabel.Location = Px(40, 84);
        subtitleLabel.Location = Px(40, 122);
        titleLabel.Visible = true;
        subtitleLabel.Visible = true;
    }

    private void SetBanner(BannerKind kind, string text)
    {
        _bannerKind = kind;
        _vaccineBannerText!.Text = text;
        _vaccineBannerText.ForeColor = BannerStyle(kind).Text;
        _vaccineBanner!.Invalidate();
    }

    // ------------------------------------------------------------------ actions

    private bool PetHasVaccineCard(int petId)
    {
        try { return PetProfiles.GetPictures(petId).Card is { Length: > 0 }; }
        catch (SqlException) { return false; }   // can't tell: ask for the card rather than let it through
    }

    /// <summary>
    /// The record is good while at least one saved vaccination has an expiry date that is still in the future. A row with no
    /// readable expiry date does not count as in date. A card that was filed
    /// without any readable vaccination rows is taken as good: nothing on it says it has lapsed.
    /// </summary>
    private static VaccineState GetVaccineState(int petId)
    {
        var rows = PetVaccinations.GetForPet(petId);
        if (rows.Count == 0) return VaccineState.Valid;
        var today = DateTime.Today;
        return rows.Any(r => r.Expires is { } e && e.Date >= today) ? VaccineState.Valid : VaccineState.Expired;
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

        // The card was read and saved. If every vaccination on it has lapsed too, it isn't accepted: stay on the scan step.
        VaccineState after;
        try { after = GetVaccineState(_vaccinePetId); }
        catch (SqlException) { after = VaccineState.Valid; }   // it saved fine; don't block on a failed re-check

        if (after == VaccineState.Expired)
        {
            ShowVaccineStep(_vaccinePetId, _vaccinePetName, isNew: false, VaccineState.Expired);
            _vaccineStatus!.Text = "This card has expired too. Please scan a card with current dates.";
            _vaccineStatus.ForeColor = Theme.Danger;
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
    private void FinishVaccineStep() => ShowServiceStep();   // step 4: choose the grooming service

    // ------------------------------------------------------------------ the panel

    private static (Color Fill, Color Border, Color Dot, string Glyph, Color Text) BannerStyle(BannerKind kind) => kind switch
    {
        BannerKind.Danger => (Color.FromArgb(254, 242, 242), Color.FromArgb(254, 202, 202), Color.FromArgb(239, 68, 68), "!", Color.FromArgb(185, 28, 28)),
        BannerKind.Success => (Color.FromArgb(232, 247, 238), Color.FromArgb(187, 229, 204), Theme.Green, "\uE73E", Color.FromArgb(22, 101, 52)),
        _ => (Color.FromArgb(255, 251, 235), Color.FromArgb(253, 230, 138), Color.FromArgb(251, 191, 36), "!", Color.FromArgb(146, 64, 14)),
    };

    private void EnsureVaccinePanel()
    {
        if (_vaccinePanel is not null) return;

        _vaccinePanel = new Panel { Location = new Point(0, PanelTop), Size = new Size(900, 396), BackColor = Theme.Bg, Visible = false };

        // ---- status banner (amber / red / green)
        _vaccineBanner = new Panel { Location = new Point(40, 0), Size = new Size(820, 44), BackColor = Theme.Bg };
        _vaccineBanner.Paint += (_, e) =>
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            float bk = Math.Max(0.1f, _vaccineBanner.Width / 820f);
            g.ScaleTransform(bk, bk);                       // drawn in the original 820 x 44 design, then enlarged
            var style = BannerStyle(_bannerKind);
            var r = new Rectangle(0, 0, 819, 43);
            using (var path = RoundedRect(r, 10))
            {
                using var fill = new SolidBrush(style.Fill);
                using var pen = new Pen(style.Border);
                g.FillPath(fill, path);
                g.DrawPath(pen, path);
            }
            var dot = new Rectangle(14, 8, 28, 28);
            using (var brush = new SolidBrush(style.Dot)) g.FillEllipse(brush, dot);
            var glyphFont = style.Glyph == "!" ? Theme.Face(11f, FontStyle.Bold) : Theme.GlyphFont(10f);
            using var centre = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString(style.Glyph, glyphFont, Brushes.White, new RectangleF(dot.X, dot.Y, dot.Width, dot.Height), centre);
        };
        _vaccineBannerText = new Label
        {
            Location = new Point(54, 0),
            Size = new Size(752, 44),
            BackColor = Color.Transparent,
            ForeColor = Color.FromArgb(146, 64, 14),
            Font = Theme.Face(9.5f, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft,
        };
        _vaccineBanner.Controls.Add(_vaccineBannerText);

        // ---- scan look: wide frame (click to scan) + a note under it
        _scanGroup = new Panel { Location = new Point(0, 116), Size = new Size(900, 276), BackColor = Theme.Bg };

        var frame = new Panel { Location = new Point(40, 0), Size = new Size(820, 214), BackColor = Theme.Bg, Cursor = Cursors.Hand };
        frame.Paint += PaintScanFrame;
        frame.Click += (_, _) => ScanVaccineCard();
        _vaccineStatus = new Label
        {
            Dock = DockStyle.Bottom,
            Height = 34,
            BackColor = Color.Transparent,
            ForeColor = Theme.Muted,
            Font = Theme.Face(9f, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            Cursor = Cursors.Hand,
        };
        _vaccineStatus.Click += (_, _) => ScanVaccineCard();
        frame.Controls.Add(_vaccineStatus);

        var info = new Panel { Location = new Point(40, 224), Size = new Size(820, 44), BackColor = Theme.Bg };
        info.Paint += (_, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var path = RoundedRect(new Rectangle(0, 0, info.Width - 1, info.Height - 1), 12);
            using var fill = new SolidBrush(Theme.PrimarySoft);
            using var pen = new Pen(Theme.Border);
            e.Graphics.FillPath(fill, path);
            e.Graphics.DrawPath(pen, path);
        };
        var infoIcon = new Label
        {
            Location = new Point(16, 10),
            Size = new Size(24, 24),
            BackColor = Color.Transparent,
            ForeColor = Theme.Primary,
            Font = Theme.GlyphFont(11f),
            Text = "\uE946",
            TextAlign = ContentAlignment.MiddleCenter,
        };
        _vaccineInfoText = new Label
        {
            Location = new Point(48, 0),
            Size = new Size(756, 44),
            BackColor = Color.Transparent,
            ForeColor = Theme.TextSoft,
            Font = Theme.Face(9.5f),
            TextAlign = ContentAlignment.MiddleLeft,
        };
        info.Controls.Add(infoIcon);
        info.Controls.Add(_vaccineInfoText);

        _scanGroup.Controls.Add(frame);
        _scanGroup.Controls.Add(info);

        // ---- "No need to scan!" look
        _validGroup = new Panel { Location = new Point(0, 80), Size = new Size(900, 290), BackColor = Theme.Bg, Visible = false };
        _validGroup.Paint += (_, e) =>
        {
            float vk = Math.Max(0.1f, _validGroup.Width / 900f);
            var saved = e.Graphics.Save();
            e.Graphics.ScaleTransform(vk, vk);           // drawn in the original 900-wide design, then enlarged
            PaintNoScanIllustration(e.Graphics, 450, 86);
            e.Graphics.Restore(saved);
        };
        var heading = new Label
        {
            Location = new Point(0, 196),
            Size = new Size(900, 40),
            BackColor = Theme.Bg,
            ForeColor = Color.FromArgb(30, 58, 138),
            Font = Theme.Face(20f, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            Text = "No need to scan!",
        };
        var sub = new Label
        {
            Location = new Point(0, 238),
            Size = new Size(900, 24),
            BackColor = Theme.Bg,
            ForeColor = Theme.Muted,
            Font = Theme.Face(9.5f),
            TextAlign = ContentAlignment.MiddleCenter,
            Text = "You can now proceed to select a grooming service.",
        };
        _validGroup.Controls.Add(heading);
        _validGroup.Controls.Add(sub);

        _vaccinePanel.Controls.Add(_vaccineBanner);
        _vaccinePanel.Controls.Add(_scanGroup);
        _vaccinePanel.Controls.Add(_validGroup);
        ContentHost.Controls.Add(_vaccinePanel);

        // "Skip Scanning" sits between Back and Continue on the form itself
        _skipButton = new Button
        {
            Text = "Skip Scanning",
            Location = new Point(385, 486),
            Size = new Size(130, 42),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            ForeColor = Theme.Primary,
            Font = Theme.Face(9.5f, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Visible = false,
            UseVisualStyleBackColor = false,
        };
        _skipButton.FlatAppearance.BorderColor = Theme.Border;
        _skipButton.Click += (_, _) => SkipVaccineScan();
        ContentHost.Controls.Add(_skipButton);
    }

    private static void PaintScanFrame(object? sender, PaintEventArgs e)
    {
        var p = (Panel)sender!;
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Drawn in the original 820-wide design, then enlarged to the size the frame has now.
        float k = Math.Max(0.1f, p.Width / 820f);
        g.ScaleTransform(k, k);
        int pw = 820, ph = (int)Math.Round(p.Height / k);

        using (var path = RoundedRect(new Rectangle(0, 0, pw - 1, ph - 1), 18))
        {
            using var fill = new SolidBrush(Theme.PrimarySoft);
            using var pen = new Pen(Theme.Border);
            g.FillPath(fill, path);
            g.DrawPath(pen, path);
        }

        // corner brackets
        using (var br = new Pen(Theme.Primary, 3f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
        {
            int m = 16, len = 26, w = pw - 1, h = ph - 1;
            g.DrawLines(br, new[] { new Point(m, m + len), new Point(m, m), new Point(m + len, m) });
            g.DrawLines(br, new[] { new Point(w - m - len, m), new Point(w - m, m), new Point(w - m, m + len) });
            g.DrawLines(br, new[] { new Point(m, h - m - len), new Point(m, h - m), new Point(m + len, h - m) });
            g.DrawLines(br, new[] { new Point(w - m - len, h - m), new Point(w - m, h - m), new Point(w - m, h - m - len) });
        }

        using var centre = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

        // camera tile
        var tile = new Rectangle(pw / 2 - 28, 20, 56, 56);
        using (var path = RoundedRect(tile, 14))
        {
            using var fill = new SolidBrush(Theme.Primary);
            g.FillPath(fill, path);
        }
        g.DrawString("\uE722", Theme.GlyphFont(20f), Brushes.White, new RectangleF(tile.X, tile.Y, tile.Width, tile.Height), centre);

        // little vaccination-record card
        var card = new Rectangle(pw / 2 - 110, 88, 220, 84);
        using (var path = RoundedRect(card, 10))
        {
            using var white = new SolidBrush(Color.White);
            using var pen = new Pen(Theme.Border);
            g.FillPath(white, path);
            g.DrawPath(pen, path);
        }
        using (var ink = new SolidBrush(Theme.Primary))
            g.DrawString("VACCINATION RECORD", Theme.Face(7f, FontStyle.Bold), ink, card.X + 70, card.Y + 10);
        using (var bar = new SolidBrush(Color.FromArgb(207, 224, 248)))
        {
            g.FillRectangle(bar, card.X + 70, card.Y + 30, 130, 6);
            g.FillRectangle(bar, card.X + 70, card.Y + 44, 100, 6);
            g.FillRectangle(bar, card.X + 70, card.Y + 58, 70, 6);
        }
        using (var paw = new SolidBrush(Theme.Primary)) g.FillEllipse(paw, card.X + 20, card.Y + 30, 30, 24);
    }

    /// <summary>Line-art dog and cat with a check badge under them (centred on <paramref name="cx"/>, <paramref name="cy"/>).</summary>
    private static void PaintNoScanIllustration(Graphics g, int cx, int cy)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var ink = Theme.Primary;

        // sparkles and confetti
        using (var spark = new Pen(ink, 1.6f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
        {
            void Plus(int x, int y, int r) { g.DrawLine(spark, x - r, y, x + r, y); g.DrawLine(spark, x, y - r, x, y + r); }
            Plus(cx - 104, cy - 70, 4);
            Plus(cx + 88, cy - 50, 4);
        }
        using (var soft = new SolidBrush(Color.FromArgb(160, 190, 235)))
        {
            g.FillEllipse(soft, cx - 124, cy - 36, 4, 4);
            g.FillEllipse(soft, cx + 112, cy - 8, 4, 4);
            void Chip(int x, int y, float angle)
            {
                var state = g.Save();
                g.TranslateTransform(x, y);
                g.RotateTransform(angle);
                g.FillRectangle(soft, -4, -2, 8, 4);
                g.Restore(state);
            }
            Chip(cx - 52, cy - 74, 40);
            Chip(cx + 62, cy - 84, 80);
            Chip(cx - 86, cy + 40, 35);
            Chip(cx + 98, cy + 36, -35);
        }

        using var pen = new Pen(ink, 2.6f) { LineJoin = LineJoin.Round, StartCap = LineCap.Round, EndCap = LineCap.Round };
        using var thin = new Pen(ink, 1.8f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
        using var white = new SolidBrush(Color.White);
        using var dark = new SolidBrush(ink);

        // dog (left, in the back)
        int dx = cx - 22, dy = cy;
        void Ear(int ex, int ey, float angle)
        {
            var state = g.Save();
            g.TranslateTransform(ex, ey);
            g.RotateTransform(angle);
            g.FillEllipse(white, -11, -20, 22, 40);
            g.DrawEllipse(pen, -11, -20, 22, 40);
            g.Restore(state);
        }
        Ear(dx - 38, dy - 4, 18);
        Ear(dx + 38, dy - 4, -18);
        g.FillEllipse(white, dx - 34, dy - 30, 68, 60);
        g.DrawEllipse(pen, dx - 34, dy - 30, 68, 60);
        g.FillEllipse(dark, dx - 17, dy - 8, 7, 9);
        g.FillEllipse(dark, dx + 10, dy - 8, 7, 9);
        g.FillEllipse(dark, dx - 7, dy + 5, 14, 9);
        g.DrawArc(thin, dx - 12, dy + 12, 12, 10, 10, 160);
        g.DrawArc(thin, dx, dy + 12, 12, 10, 20, 160);

        // cat (right, in front)
        int kx = cx + 44, ky = cy + 14;
        var earL = new[] { new Point(kx - 26, ky - 4), new Point(kx - 22, ky - 30), new Point(kx - 4, ky - 18) };
        var earR = new[] { new Point(kx + 26, ky - 4), new Point(kx + 22, ky - 30), new Point(kx + 4, ky - 18) };
        g.FillPolygon(white, earL);
        g.DrawPolygon(pen, earL);
        g.FillPolygon(white, earR);
        g.DrawPolygon(pen, earR);
        g.FillEllipse(white, kx - 28, ky - 22, 56, 46);
        g.DrawEllipse(pen, kx - 28, ky - 22, 56, 46);
        g.FillEllipse(dark, kx - 13, ky - 6, 6, 8);
        g.FillEllipse(dark, kx + 7, ky - 6, 6, 8);
        g.FillEllipse(dark, kx - 3, ky + 4, 6, 4);
        g.DrawLine(thin, kx - 30, ky + 6, kx - 42, ky + 3);
        g.DrawLine(thin, kx - 30, ky + 11, kx - 42, ky + 14);
        g.DrawLine(thin, kx + 30, ky + 6, kx + 42, ky + 3);
        g.DrawLine(thin, kx + 30, ky + 11, kx + 42, ky + 14);

        // check badge
        var badge = new Rectangle(cx - 15, cy + 52, 30, 30);
        using (var ring = new Pen(Color.White, 4f)) g.DrawEllipse(ring, Rectangle.Inflate(badge, 2, 2));
        g.FillEllipse(dark, badge);
        using var centre = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString("\uE73E", Theme.GlyphFont(11f), Brushes.White, new RectangleF(badge.X, badge.Y, badge.Width, badge.Height), centre);
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
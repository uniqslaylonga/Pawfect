using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Drop-in replacement for <see cref="MessageBox"/> with the Pawfect look.
/// Use <c>Msg.Show(this, "text", "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information)</c> exactly like MessageBox.Show.
/// If the text has a blank line in it, the first paragraph becomes the bold heading.
/// </summary>
internal static class Msg
{
    public static DialogResult Show(string text)
        => Show(null, text, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.None);

    public static DialogResult Show(string text, string caption)
        => Show(null, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None);

    public static DialogResult Show(IWin32Window? owner, string text)
        => Show(owner, text, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.None);

    public static DialogResult Show(IWin32Window? owner, string text, string caption)
        => Show(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None);

    public static DialogResult Show(IWin32Window? owner, string text, string caption,
        MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
    {
        owner ??= Form.ActiveForm;
        using var form = new PawMessageForm(text, caption, buttons, icon, defaultButton, owner is not null);
        return owner is null ? form.ShowDialog() : form.ShowDialog(owner);
    }
}

/// <summary>The window behind <see cref="Msg"/>: round icon, message and Pawfect buttons.</summary>
internal sealed class PawMessageForm : Form
{
    private const int FormWidth = 440;
    private const int Pad = 24;
    private const int IconSize = 44;
    private const int TextLeft = Pad + IconSize + 16;
    private const int BarHeight = 64;

    public PawMessageForm(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton, bool hasOwner)
    {
        Icon = AppIcon.Value;
        Text = caption;
        Font = Theme.Face(9f);
        BackColor = Color.White;
        ForeColor = Theme.Text;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = hasOwner ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        ShowIcon = false;

        // ---- icon
        var (glyph, fore, fill) = icon switch
        {
            MessageBoxIcon.Error => ("", Theme.Danger, ColorTranslator.FromHtml("#FDE8E8")),
            MessageBoxIcon.Warning => ("", ColorTranslator.FromHtml("#C77A0A"), ColorTranslator.FromHtml("#FEF3E0")),
            MessageBoxIcon.Question => ("", Theme.Primary, Theme.PrimarySoft),
            _ => ("", Theme.Primary, Theme.PrimarySoft),
        };
        var badge = new IconBadge(glyph, fill, fore, Color.White, IconSize, circle: true, glyphPt: 16f) { Location = new Point(Pad, Pad) };

        // ---- text: first paragraph is the heading when there is more than one
        string heading = "";
        string body = text.Trim();
        int split = body.IndexOf("\n\n", StringComparison.Ordinal);
        if (split > 0 && split <= 110 && !body[..split].Contains('\n'))
        {
            heading = body[..split].Trim();
            body = body[(split + 2)..].Trim();
        }

        int textWidth = FormWidth - TextLeft - Pad;
        int y = Pad;
        var controls = new List<Control> { badge };

        int Measure(string s, Font f) => TextRenderer.MeasureText(s, f, new Size(textWidth, int.MaxValue),
            TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding).Height + 4;

        if (heading.Length > 0)
        {
            var f = Theme.Face(11f, FontStyle.Bold);
            int h = Measure(heading, f);
            var l = Ui.Text(heading, 11f, Theme.Text, Color.White, FontStyle.Bold, ContentAlignment.TopLeft);
            l.SetBounds(TextLeft, y, textWidth, h);
            controls.Add(l);
            y += h + 6;
        }
        if (body.Length > 0)
        {
            bool alone = heading.Length == 0;
            var f = Theme.Face(alone ? 10f : 9.5f);
            int h = Measure(body, f);
            // A single short line sits level with the icon's middle rather than the top.
            int top = alone && h < IconSize ? y + (IconSize - h) / 2 : y;
            var l = Ui.Text(body, alone ? 10f : 9.5f, alone ? Theme.Text : Theme.TextSoft, Color.White,
                FontStyle.Regular, ContentAlignment.TopLeft);
            l.SetBounds(TextLeft, top, textWidth, h);
            controls.Add(l);
            y = Math.Max(y, top + h);
        }

        int contentBottom = Math.Max(y, Pad + IconSize);
        ClientSize = new Size(FormWidth, contentBottom + Pad + BarHeight);

        // ---- buttons
        var results = buttons switch
        {
            MessageBoxButtons.OKCancel => new[] { DialogResult.OK, DialogResult.Cancel },
            MessageBoxButtons.YesNo => new[] { DialogResult.Yes, DialogResult.No },
            MessageBoxButtons.YesNoCancel => new[] { DialogResult.Yes, DialogResult.No, DialogResult.Cancel },
            MessageBoxButtons.RetryCancel => new[] { DialogResult.Retry, DialogResult.Cancel },
            MessageBoxButtons.AbortRetryIgnore => new[] { DialogResult.Abort, DialogResult.Retry, DialogResult.Ignore },
            _ => new[] { DialogResult.OK },
        };
        int defIndex = Math.Min(results.Length - 1, defaultButton switch
        {
            MessageBoxDefaultButton.Button2 => 1,
            MessageBoxDefaultButton.Button3 => 2,
            _ => 0,
        });

        var bar = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = BarHeight,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(Pad - 8, 14, Pad - 8, 10),
            BackColor = Theme.Subtle,
            WrapContents = false,
        };
        bar.Paint += (_, e) =>
        {
            using var pen = new Pen(Theme.Border);
            e.Graphics.DrawLine(pen, 0, 0, bar.Width, 0);
        };

        PawButton? defaultBtn = null;
        PawButton? cancelBtn = null;
        for (int i = results.Length - 1; i >= 0; i--)   // right-most is added first
        {
            var r = results[i];
            bool isDefault = i == defIndex;
            var btn = new PawButton
            {
                Text = ButtonText(r),
                DialogResult = r,
                Width = (r is DialogResult.Cancel or DialogResult.No or DialogResult.Ignore) ? 92 : 100,
                Margin = new Padding(8, 0, 0, 0),
                Kind = isDefault
                    ? ((icon is MessageBoxIcon.Warning or MessageBoxIcon.Error) && r == DialogResult.Yes ? PawButtonKind.Danger : PawButtonKind.Primary)
                    : PawButtonKind.Outline,
            };
            if (isDefault) defaultBtn = btn;
            if (r == DialogResult.Cancel || (r == DialogResult.No && cancelBtn is null)) cancelBtn = btn;
            if (results.Length == 1) cancelBtn = btn;
            bar.Controls.Add(btn);
        }

        Controls.AddRange(controls.ToArray());
        Controls.Add(bar);
        AcceptButton = defaultBtn;
        CancelButton = cancelBtn;
        ControlBox = cancelBtn is not null;   // Esc / the X answer with Cancel (or No); Abort/Retry/Ignore needs a button
        ActiveControl = defaultBtn;
    }

    private static string ButtonText(DialogResult r) => r switch
    {
        DialogResult.OK => "OK",
        DialogResult.Cancel => "Cancel",
        DialogResult.Yes => "Yes",
        DialogResult.No => "No",
        DialogResult.Retry => "Retry",
        DialogResult.Abort => "Abort",
        DialogResult.Ignore => "Ignore",
        _ => r.ToString(),
    };
}

/// <summary>Read-only appointment card: pet, service and status on top, then who/when/where.</summary>
internal sealed class AppointmentDetailsDialog : Form
{
    private const int FormWidth = 580;
    private const int Pad = 24;

    /// <summary>Set when the person pressed Confirm or Check in instead of Close (the caller applies it).</summary>
    public AppointmentStatus? RequestedStatus { get; private set; }

    /// <summary>True when Reschedule was chosen (the caller opens the schedule editor).</summary>
    public bool RescheduleRequested { get; private set; }

    public AppointmentDetailsDialog(AppointmentItem a, bool offerActions = false)
    {
        Icon = AppIcon.Value;
        // Pet photo and vaccination card live on the pet, so look them up through the appointment.
        byte[]? photoBytes = null, cardBytes = null;
        try
        {
            if (EmployeeData.GetAppointmentRefs(a.Id) is { } refs)
                (photoBytes, cardBytes) = PetProfiles.GetPictures(refs.PetId);
        }
        catch (Exception ex) when (ex is Microsoft.Data.SqlClient.SqlException or InvalidOperationException) { }

        Text = "Appointment";
        Font = Theme.Face(9f);
        BackColor = Color.White;
        ForeColor = Theme.Text;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        ShowIcon = false;

        // ---- header: pet avatar, name, service, status
        var header = new Panel { Dock = DockStyle.Top, Height = 84, BackColor = Color.White };
        var (tintFill, tintFore) = AppointmentRowView.PetTint(a.PetName);
        string initial = a.PetName.Trim().Length > 0 ? a.PetName.Trim()[..1].ToUpperInvariant() : "?";
        string pet = a.PetBreed.Length == 0 ? a.PetName : $"{a.PetName}  ({a.PetBreed})";
        var title = Ui.Text(pet, 12f, Theme.Text, Color.White, FontStyle.Bold);
        title.SetBounds(Pad + 62, 18, FormWidth - Pad - 62 - 130, 26);
        var service = Ui.Text(a.Service, 9.5f, Theme.TextSoft, Color.White);
        service.SetBounds(Pad + 62, 44, FormWidth - Pad - 62 - 130, 22);

        string statusText = AppointmentStatusStyle.Label(a.Status);
        int pillW = TextRenderer.MeasureText(statusText, Theme.Face(8f, FontStyle.Bold)).Width + 18;
        using var avatar = AvatarImages.FromBytes(photoBytes);
        var avatarCopy = avatar is null ? null : new Bitmap(avatar);
        Disposed += (_, _) => avatarCopy?.Dispose();
        header.Paint += (_, e) =>
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            if (avatarCopy is not null)
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                using var clip = new System.Drawing.Drawing2D.GraphicsPath();
                clip.AddEllipse(Pad, 18, 48, 48);
                var state = g.Save();
                g.SetClip(clip);
                float scale = Math.Max(48f / avatarCopy.Width, 48f / avatarCopy.Height);
                float w = avatarCopy.Width * scale, h = avatarCopy.Height * scale;
                g.DrawImage(avatarCopy, Pad + (48 - w) / 2f, 18 + (48 - h) / 2f, w, h);
                g.Restore(state);
                using var ring = new Pen(Theme.Border);
                g.DrawEllipse(ring, Pad, 18, 48, 48);
            }
            else
            {
                using (var b = new SolidBrush(tintFill))
                    g.FillEllipse(b, Pad, 18, 48, 48);
                TextRenderer.DrawText(g, initial, Theme.Face(14f, FontStyle.Bold), new Rectangle(Pad, 18, 48, 48), tintFore,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding |
                    TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
            }
            AppointmentStatusStyle.DrawPill(g, a.Status, header.Width - Pad - pillW, 42, pillW + 2);
            using var pen = new Pen(Theme.Border);
            e.Graphics.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
        };
        header.Controls.AddRange(new Control[] { title, service });

        // ---- body: icon chip + small caption + value, two per row
        var body = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
        int colW = (FormWidth - Pad * 2) / 2;
        int y = 20;

        void Item(string glyph, string caption, string value, int x, int width, int valueHeight = 20)
        {
            var chip = new IconBadge(glyph, Theme.PrimarySoft, Theme.Primary, Color.White, 34, radius: 9, glyphPt: 10f)
            {
                Location = new Point(x, y),
            };
            var cap = Ui.Text(caption, 8f, Theme.Muted, Color.White);
            cap.SetBounds(x + 44, y - 1, width - 48, 15);
            var val = Ui.Text(value, 9.5f, Theme.Text, Color.White, FontStyle.Regular, ContentAlignment.TopLeft);
            val.SetBounds(x + 44, y + 15, width - 48, valueHeight);
            body.Controls.AddRange(new Control[] { chip, cap, val });
        }

        string Or(string s, string fallback) => s.Length == 0 ? fallback : s;

        Item("", "CUSTOMER", a.CustomerName, Pad, colW);
        Item("", "PHONE", Or(a.CustomerPhone, "—"), Pad + colW, colW);
        y += 56;
        bool hasPreferred = a.PreferredDayList.Count > 0;
        if (a.Status == AppointmentStatus.Pending && hasPreferred)
            Item("", "WHEN", "Not scheduled yet - staff will set it", Pad, colW);   // the stored start is only a placeholder until staff pick a date
        else
            Item("", "WHEN", a.Start.ToString("dddd, MMM d, yyyy  ·  h:mm tt", System.Globalization.CultureInfo.CurrentCulture),
                Pad, hasPreferred ? colW : FormWidth - Pad * 2);
        if (hasPreferred)
            Item("", "PREFERRED DAYS", PreferredDaysText.Readable(a.PreferredDayList, full: true), Pad + colW, colW);
        y += 56;
        Item("", "GROOMER", Or(a.Groomer, "Not assigned"), Pad, colW);
        Item("", "ROOM", Or(a.Room, "Not assigned"), Pad + colW, colW);
        y += 56;
        if (a.Notes.Length > 0)
        {
            int noteH = TextRenderer.MeasureText(a.Notes, Theme.Face(9.5f),
                new Size(FormWidth - Pad * 2 - 48, int.MaxValue), TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix).Height + 4;
            Item("", "CUSTOMER NOTES", a.Notes, Pad, FormWidth - Pad * 2, Math.Min(noteH, 120));
            y += 36 + Math.Min(noteH, 120);
        }

        // ---- vaccination card (click to enlarge)
        {
            var cap = Ui.Text("VACCINATION CARD", 8f, Theme.Muted, Color.White);
            cap.SetBounds(Pad, y - 1, FormWidth - Pad * 2, 15);
            var card = new PetPictureBox("Vaccination card - " + a.PetName, "No vaccination card on file",
                new Size(FormWidth - Pad * 2, 170)) { Location = new Point(Pad, y + 17) };
            card.SetBytes(cardBytes);
            body.Controls.AddRange(new Control[] { cap, card });
            y += 17 + card.Height + 12;
        }

        // ---- footer
        // Buttons are sized from their own text (the font scales with Windows display scaling, fixed pixel widths don't)
        // and laid out by hand: Cancel sits on the left, the other actions are right-aligned, and the card widens
        // if they would not fit - so nothing is ever clipped at an edge.
        const int ButtonH = 36, ButtonGap = 10;
        var bar = new Panel { Dock = DockStyle.Bottom, Height = 64, BackColor = Theme.Subtle };
        bar.Paint += (_, e) =>
        {
            using var pen = new Pen(Theme.Border);
            e.Graphics.DrawLine(pen, 0, 0, bar.Width, 0);
        };

        PawButton MakeButton(string text, PawButtonKind kind, int minWidth)
        {
            int textW = TextRenderer.MeasureText(text, Theme.Face(9f, FontStyle.Bold), Size.Empty,
                TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine).Width;
            return new PawButton { Text = text, Kind = kind, Size = new Size(Math.Max(minWidth, textW + 36), ButtonH) };
        }

        bool open = a.Status is AppointmentStatus.Pending or AppointmentStatus.Rescheduled or AppointmentStatus.Confirmed;
        bool actions = offerActions && open;
        var close = MakeButton("Close", actions ? PawButtonKind.Outline : PawButtonKind.Primary, 84);
        close.DialogResult = DialogResult.OK;

        var rightButtons = new List<PawButton>();   // left to right
        if (actions)
        {
            if (a.Status is AppointmentStatus.Pending or AppointmentStatus.Rescheduled)
            {
                var confirm = MakeButton(a.Groomer.Length == 0 || a.Room.Length == 0 ? "Schedule and confirm" : "Confirm", PawButtonKind.Primary, 96);
                confirm.Click += (_, _) => { RequestedStatus = AppointmentStatus.Confirmed; DialogResult = DialogResult.Yes; };
                rightButtons.Add(confirm);
            }
            var reschedule = MakeButton("Reschedule", PawButtonKind.Outline, 96);
            reschedule.Click += (_, _) => { RescheduleRequested = true; DialogResult = DialogResult.Yes; };
            rightButtons.Add(reschedule);
            var checkIn = MakeButton("Check in", PawButtonKind.Primary, 96);
            checkIn.Click += (_, _) => { RequestedStatus = AppointmentStatus.CheckedIn; DialogResult = DialogResult.Yes; };
            rightButtons.Add(checkIn);
        }
        rightButtons.Add(close);

        PawButton? cancel = null;
        // Once an appointment is confirmed or checked in it can no longer be cancelled from here.
        if (a.Status is AppointmentStatus.Pending or AppointmentStatus.Rescheduled)
        {
            cancel = MakeButton("Cancel appointment", PawButtonKind.Danger, 96);
            cancel.Click += (_, _) => { RequestedStatus = AppointmentStatus.Cancelled; DialogResult = DialogResult.Yes; };
            bar.Controls.Add(cancel);
        }
        foreach (var b in rightButtons) bar.Controls.Add(b);

        void LayoutBar()
        {
            int by = (bar.Height - ButtonH) / 2;
            int x = bar.Width - Pad;
            for (int i = rightButtons.Count - 1; i >= 0; i--)
            {
                x -= rightButtons[i].Width;
                rightButtons[i].Location = new Point(x, by);
                x -= ButtonGap;
            }
            if (cancel is not null) cancel.Location = new Point(Pad, by);
        }
        bar.Resize += (_, _) => LayoutBar();

        int neededWidth = Pad * 2 + rightButtons.Sum(b => b.Width) + ButtonGap * (rightButtons.Count - 1)
            + (cancel is null ? 0 : cancel.Width + ButtonGap * 2);
        AcceptButton = close;
        CancelButton = close;

        Controls.Add(body);
        Controls.Add(header);
        Controls.Add(bar);
        ClientSize = new Size(Math.Max(FormWidth, neededWidth), header.Height + y + 4 + bar.Height);
        LayoutBar();
    }
}

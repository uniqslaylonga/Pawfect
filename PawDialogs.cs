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
    private const int FormWidth = 460;
    private const int Pad = 24;

    public AppointmentDetailsDialog(AppointmentItem a)
    {
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
        header.Paint += (_, e) =>
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (var b = new SolidBrush(tintFill))
                g.FillEllipse(b, Pad, 18, 48, 48);
            TextRenderer.DrawText(g, initial, Theme.Face(14f, FontStyle.Bold), new Rectangle(Pad, 18, 48, 48), tintFore,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding |
                TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
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
        Item("", "WHEN", a.Start.ToString("dddd, MMM d, yyyy  ·  h:mm tt", System.Globalization.CultureInfo.CurrentCulture),
            Pad, FormWidth - Pad * 2);
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

        // ---- footer
        var bar = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 64,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(Pad - 8, 14, Pad - 8, 10),
            BackColor = Theme.Subtle,
        };
        bar.Paint += (_, e) =>
        {
            using var pen = new Pen(Theme.Border);
            e.Graphics.DrawLine(pen, 0, 0, bar.Width, 0);
        };
        var close = new PawButton { Text = "Close", Kind = PawButtonKind.Primary, DialogResult = DialogResult.OK, Width = 100 };
        bar.Controls.Add(close);
        AcceptButton = close;
        CancelButton = close;

        Controls.Add(body);
        Controls.Add(header);
        Controls.Add(bar);
        ClientSize = new Size(FormWidth, header.Height + y + 4 + bar.Height);
    }
}

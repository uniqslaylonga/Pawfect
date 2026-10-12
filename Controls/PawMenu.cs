using System.Drawing.Drawing2D;
using System.Globalization;

namespace MyApp.Controls;

/// <summary>Colour family of a menu entry (icon chip, hover tint and text accent).</summary>
internal enum MenuTone
{
    Neutral,
    Primary,
    Success,
    Teal,
    Warning,
    Danger,
}

/// <summary>What a <see cref="PawMenuItem"/> looks like.</summary>
internal enum MenuKind
{
    /// <summary>Icon chip + label; the chip fills with its colour on hover.</summary>
    Action,
    /// <summary>Bell icon, title, subtitle and an optional count badge (not clickable).</summary>
    Header,
    /// <summary>Small grey caption line (not clickable).</summary>
    Caption,
    /// <summary>Pet avatar, "Customer - Pet", "Service - time" and a status badge.</summary>
    Notification,
    /// <summary>Full-width solid button (e.g. "Open appointments").</summary>
    Button,
    /// <summary>Centred icon + message shown when there is nothing to list.</summary>
    Empty,
}

/// <summary>Builds the rounded, shadowed dropdowns used for notifications and the check-in / confirm / cancel menus.</summary>
internal static class PawMenu
{
    public static Color Fore(MenuTone t) => t switch
    {
        MenuTone.Primary => Theme.Primary,
        MenuTone.Success => Theme.Green,
        MenuTone.Teal => ColorTranslator.FromHtml("#0E8F9E"),
        MenuTone.Warning => ColorTranslator.FromHtml("#C77A0A"),
        MenuTone.Danger => ColorTranslator.FromHtml("#D93636"),
        _ => Theme.TextSoft,
    };

    public static Color Soft(MenuTone t) => t switch
    {
        MenuTone.Primary => Theme.PrimarySoft,
        MenuTone.Success => Theme.GreenSoft,
        MenuTone.Teal => ColorTranslator.FromHtml("#E0F5F7"),
        MenuTone.Warning => ColorTranslator.FromHtml("#FEF3E0"),
        MenuTone.Danger => ColorTranslator.FromHtml("#FDE8E8"),
        _ => Theme.Track,
    };

    /// <summary>Creates an empty menu with the Pawfect look: white card, rounded corners, soft border.</summary>
    public static ContextMenuStrip Create(int minWidth = 0)
    {
        var menu = new ContextMenuStrip
        {
            ShowImageMargin = false,
            ShowCheckMargin = false,
            Font = Theme.Face(9f),
            BackColor = Color.White,
            Padding = new Padding(6),
            DropShadowEnabled = false, // a square OS shadow would show behind the rounded corners
            Renderer = new PawMenuRenderer(),
            MinimumSize = new Size(minWidth, 0),
        };

        void Round() { if (menu.Width > 0 && menu.Height > 0) menu.Region = new Region(Ui.RoundRect(new RectangleF(0, 0, menu.Width, menu.Height), 12)); }
        menu.Opened += (_, _) => Round();
        menu.SizeChanged += (_, _) => Round();
        menu.Closed += (_, _) => menu.BeginInvoke(menu.Dispose);
        return menu;
    }

    /// <summary>Adds an action row (icon chip + label) and returns it.</summary>
    public static PawMenuItem AddAction(this ContextMenuStrip menu, string text, string glyph, MenuTone tone, Action onClick)
    {
        var item = new PawMenuItem(MenuKind.Action, text) { Glyph = glyph, Tone = tone };
        item.Click += (_, _) => onClick();
        menu.Items.Add(item);
        return item;
    }

    public static void AddDivider(this ContextMenuStrip menu) => menu.Items.Add(new ToolStripSeparator());
}

/// <summary>Draws the dropdown card (white, rounded border) and thin dividers.</summary>
internal sealed class PawMenuRenderer : ToolStripProfessionalRenderer
{
    public PawMenuRenderer() : base(new PawColors()) { RoundedEdges = false; }

    protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
    {
        using var b = new SolidBrush(Color.White);
        e.Graphics.FillRectangle(b, e.AffectedBounds);
    }

    protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using var path = Ui.RoundRect(new RectangleF(0.5f, 0.5f, e.ToolStrip.Width - 1.5f, e.ToolStrip.Height - 1.5f), 12);
        using var pen = new Pen(Theme.Border);
        g.DrawPath(pen, path);
    }

    protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
    {
        using var pen = new Pen(Theme.Border);
        int y = e.Item.Height / 2;
        e.Graphics.DrawLine(pen, 10, y, e.Item.Width - 10, y);
    }

    protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e) { /* items paint themselves */ }

    private sealed class PawColors : ProfessionalColorTable
    {
        public override Color ToolStripDropDownBackground => Color.White;
        public override Color MenuBorder => Theme.Border;
    }
}

/// <summary>A self-painted menu row. <see cref="MenuKind"/> picks the layout.</summary>
internal sealed class PawMenuItem : ToolStripMenuItem
{
    private const TextFormatFlags LineFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
        TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;

    private const TextFormatFlags CenterFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
        TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

    public PawMenuItem(MenuKind kind, string text) : base(text)
    {
        Kind = kind;
        AutoSize = true;   // the dropdown asks GetPreferredSize and then stretches each row to the full menu width
        Padding = Padding.Empty;
        Margin = new Padding(0, 1, 0, 1);
        // Headers, captions and the empty state are display-only; disabled items never highlight or click.
        if (kind is MenuKind.Header or MenuKind.Caption or MenuKind.Empty) Enabled = false;
    }

    public MenuKind Kind { get; }
    public string Glyph { get; set; } = "";
    public MenuTone Tone { get; set; } = MenuTone.Neutral;
    public string SubText { get; set; } = "";
    public string Badge { get; set; } = "";
    public AppointmentStatus? Status { get; set; }

    private int PreferredWidth { get; set; } = 160;
    public int MenuWidth { set => PreferredWidth = value; }

    public override Size GetPreferredSize(Size constrainingSize)
    {
        switch (Kind)
        {
            case MenuKind.Header: return new Size(PreferredWidth, 52);
            case MenuKind.Caption: return new Size(PreferredWidth, 24);
            case MenuKind.Notification: return new Size(PreferredWidth, 56);
            case MenuKind.Button: return new Size(PreferredWidth, 38);
            case MenuKind.Empty: return new Size(PreferredWidth, 110);
            default:
                int w = TextRenderer.MeasureText(Text, Theme.Face(9f, FontStyle.Bold)).Width + (Glyph.Length > 0 ? 62 : 32);
                return new Size(Math.Max(PreferredWidth, w), 38);
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        var r = new Rectangle(0, 0, Width, Height);
        bool hot = Selected && Enabled;

        switch (Kind)
        {
            case MenuKind.Header: PaintHeader(g, r); break;
            case MenuKind.Caption: PaintCaption(g, r); break;
            case MenuKind.Notification: PaintNotification(g, r, hot); break;
            case MenuKind.Button: PaintButton(g, r, hot); break;
            case MenuKind.Empty: PaintEmpty(g, r); break;
            default: PaintAction(g, r, hot); break;
        }
    }

    private static void FillRound(Graphics g, Rectangle rect, int radius, Color color)
    {
        using var path = Ui.RoundRect(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), radius);
        using var b = new SolidBrush(color);
        g.FillPath(b, path);
    }

    private void PaintAction(Graphics g, Rectangle r, bool hot)
    {
        var fore = PawMenu.Fore(Tone);
        var soft = PawMenu.Soft(Tone);

        if (hot) FillRound(g, new Rectangle(r.X, r.Y, r.Width - 1, r.Height - 1), 8, Tone == MenuTone.Neutral ? Theme.SubtleHover : soft);

        int textX = 12;
        if (Glyph.Length > 0)
        {
            // Icon chip: soft tint normally, solid colour with a white icon on hover.
            var chip = new Rectangle(8, (r.Height - 26) / 2, 26, 26);
            FillRound(g, chip, 8, hot ? fore : soft);
            TextRenderer.DrawText(g, Glyph, Theme.GlyphFont(10f), chip, hot ? Color.White : fore, CenterFlags);
            textX = 44;
        }

        TextRenderer.DrawText(g, Text, Theme.Face(9f, FontStyle.Bold),
            new Rectangle(textX, 0, r.Width - textX - 30, r.Height),
            Tone == MenuTone.Neutral ? Theme.Text : fore, LineFlags);

        if (Checked)
            TextRenderer.DrawText(g, "\uE73E", Theme.GlyphFont(9f), new Rectangle(r.Width - 28, 0, 20, r.Height), Theme.Primary, CenterFlags);
        else if (hot && Tone != MenuTone.Neutral)
            TextRenderer.DrawText(g, "\uE76C", Theme.GlyphFont(8f), new Rectangle(r.Width - 28, 0, 20, r.Height), fore, CenterFlags);
    }

    private void PaintHeader(Graphics g, Rectangle r)
    {
        var chip = new Rectangle(8, (r.Height - 34) / 2, 34, 34);
        FillRound(g, chip, 10, Theme.PrimarySoft);
        TextRenderer.DrawText(g, Glyph.Length > 0 ? Glyph : "\uEA8F", Theme.GlyphFont(12f), chip, Theme.Primary, CenterFlags);

        int badgeW = 0;
        if (Badge.Length > 0)
        {
            var bf = Theme.Face(8f, FontStyle.Bold);
            badgeW = Math.Max(24, TextRenderer.MeasureText(Badge, bf).Width + 14);
            var pill = new Rectangle(r.Width - badgeW - 10, (r.Height - 22) / 2, badgeW, 22);
            FillRound(g, pill, 11, Theme.Primary);
            TextRenderer.DrawText(g, Badge, bf, pill, Color.White, CenterFlags);
            badgeW += 8;
        }

        int textW = r.Width - 52 - badgeW - 10;
        TextRenderer.DrawText(g, Text, Theme.Face(10f, FontStyle.Bold), new Rectangle(52, 8, textW, 20), Theme.Text, LineFlags);
        if (SubText.Length > 0)
            TextRenderer.DrawText(g, SubText, Theme.Face(8f), new Rectangle(52, 27, textW, 16), Theme.Muted, LineFlags);
    }

    private void PaintCaption(Graphics g, Rectangle r)
        => TextRenderer.DrawText(g, (Text ?? "").ToUpperInvariant(), Theme.Face(7.5f, FontStyle.Bold),
            new Rectangle(12, 0, r.Width - 24, r.Height), Theme.Muted, LineFlags);

    private void PaintNotification(Graphics g, Rectangle r, bool hot)
    {
        if (hot) FillRound(g, new Rectangle(r.X, r.Y, r.Width - 1, r.Height - 1), 8, Theme.PrimarySoft);

        // Pet avatar, same look as the Pending Requests rows.
        var avatar = new Rectangle(8, (r.Height - 36) / 2, 36, 36);
        using (var b = new SolidBrush(Color.FromArgb(254, 243, 224)))
            g.FillEllipse(b, avatar);
        string initial = Glyph.Length > 0 ? Glyph : "?";
        TextRenderer.DrawText(g, initial, Theme.Face(10f, FontStyle.Bold), avatar, Color.FromArgb(199, 122, 10), CenterFlags);

        // Status badge on the right.
        int rightEdge = r.Width - 10;
        if (Status is { } status)
        {
            var (fore, fill, _) = AppointmentStatusStyle.Pill(status == AppointmentStatus.Pending ? AppointmentStatus.Completed : status);
            string label = status == AppointmentStatus.Pending ? "Waiting" : AppointmentStatusStyle.Label(status);
            var f = Theme.Face(7.5f, FontStyle.Bold);
            int w = TextRenderer.MeasureText(label, f).Width + 16;
            var pill = new Rectangle(r.Width - w - 10, (r.Height - 20) / 2, w, 20);
            FillRound(g, pill, 10, fill);
            TextRenderer.DrawText(g, label, f, pill, fore, CenterFlags);
            rightEdge = pill.X - 8;
        }

        int textW = Math.Max(20, rightEdge - 52);
        TextRenderer.DrawText(g, Text, Theme.Face(9f, FontStyle.Bold), new Rectangle(52, 9, textW, 18), Theme.Text, LineFlags);
        TextRenderer.DrawText(g, SubText, Theme.Face(8f), new Rectangle(52, 28, textW, 16), Theme.Muted, LineFlags);
    }

    private void PaintButton(Graphics g, Rectangle r, bool hot)
    {
        var rect = new Rectangle(0, 0, r.Width - 1, r.Height - 1);
        using (var path = Ui.RoundRect(rect, 9))
        using (var b = new LinearGradientBrush(rect, hot ? Color.FromArgb(46, 108, 196) : Theme.Primary,
                   hot ? Color.FromArgb(58, 123, 213) : Color.FromArgb(78, 144, 228), LinearGradientMode.Vertical))
            g.FillPath(b, path);

        const string arrow = "\uE76C";
        var tf = Theme.Face(9f, FontStyle.Bold);
        int tw = TextRenderer.MeasureText(Text, tf).Width;
        int total = tw + 8 + 12;
        int x = (r.Width - total) / 2;
        TextRenderer.DrawText(g, Text, tf, new Rectangle(x, 0, tw + 2, r.Height), Color.White, LineFlags);
        TextRenderer.DrawText(g, arrow, Theme.GlyphFont(8f), new Rectangle(x + tw + 8, 0, 14, r.Height), Color.White, CenterFlags);
    }

    private void PaintEmpty(Graphics g, Rectangle r)
    {
        var chip = new Rectangle((r.Width - 44) / 2, 14, 44, 44);
        using (var b = new SolidBrush(Theme.GreenSoft)) g.FillEllipse(b, chip);
        TextRenderer.DrawText(g, "\uE73E", Theme.GlyphFont(16f), chip, Theme.Green, CenterFlags);
        TextRenderer.DrawText(g, Text, Theme.Face(9.5f, FontStyle.Bold), new Rectangle(0, 62, r.Width, 20), Theme.Text, CenterFlags);
        if (SubText.Length > 0)
            TextRenderer.DrawText(g, SubText, Theme.Face(8f), new Rectangle(0, 82, r.Width, 18), Theme.Muted, CenterFlags);
    }

    public static string Time(DateTime t) => t.ToString("h:mm tt", CultureInfo.CurrentCulture);
}
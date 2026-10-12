using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace MyApp.Controls;

/// <summary>Text-drawing flags shared by the billing controls.</summary>
internal static class Tf
{
    private const TextFormatFlags Base = TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
        TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;

    public const TextFormatFlags Left = Base | TextFormatFlags.Left;
    public const TextFormatFlags Right = Base | TextFormatFlags.Right;
    public const TextFormatFlags Center = Base | TextFormatFlags.HorizontalCenter;
}

/// <summary>Small rounded status badges ("Completed", "Cash", "Service" ...).</summary>
internal static class Pills
{
    /// <summary>Draws a badge centred in <paramref name="cell"/>.</summary>
    public static void DrawCentered(Graphics g, string text, Color fore, Color fill, Color border,
        Rectangle cell, float pt = 7.5f, int height = 20)
    {
        var font = Theme.Face(pt, FontStyle.Bold);
        int w = Math.Min(Math.Max(10, cell.Width - 4), TextRenderer.MeasureText(text, font).Width + 18);
        var rect = new Rectangle(cell.X + (cell.Width - w) / 2, cell.Y + (cell.Height - height) / 2, w, height);
        Draw(g, text, font, fore, fill, border, rect);
    }

    /// <summary>Draws a badge starting at <paramref name="x"/> and returns its width.</summary>
    public static int DrawAt(Graphics g, string text, Color fore, Color fill, Color border,
        int x, int centerY, int maxWidth, float pt = 7.5f, int height = 22)
    {
        var font = Theme.Face(pt, FontStyle.Bold);
        int w = Math.Max(10, Math.Min(maxWidth, TextRenderer.MeasureText(text, font).Width + 18));
        Draw(g, text, font, fore, fill, border, new Rectangle(x, centerY - height / 2, w, height));
        return w;
    }

    private static void Draw(Graphics g, string text, Font font, Color fore, Color fill, Color border, Rectangle rect)
    {
        var old = g.SmoothingMode;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using (var path = Ui.RoundRect(new RectangleF(rect.X + 0.5f, rect.Y + 0.5f, rect.Width - 1, rect.Height - 1), rect.Height / 2f))
        using (var b = new SolidBrush(fill))
        using (var pen = new Pen(border))
        {
            g.FillPath(b, path);
            g.DrawPath(pen, path);
        }
        g.SmoothingMode = old;
        TextRenderer.DrawText(g, text, font, rect, fore, Tf.Center);
    }
}

internal enum ButtonKind { Outline, Soft, Danger, Primary }

/// <summary>
/// Rounded button with an optional icon. <see cref="Kind"/> picks the look: Outline (white), Soft (pale blue),
/// Danger (pale red) or Primary (solid blue). <see cref="Selected"/> highlights an Outline button as chosen.
/// </summary>
internal sealed class OutlineButton : Control
{
    private ButtonKind _kind = ButtonKind.Outline;
    private string _glyph = "";
    private bool _selected;
    private bool _hover;
    private bool _down;

    public OutlineButton()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        Font = Theme.Face(8.5f);
        BackColor = Color.White;
        Cursor = Cursors.Hand;
        Size = new Size(120, 34);
    }

    [Category("Pawfect"), Description("Visual style of the button.")]
    [DefaultValue(ButtonKind.Outline)]
    public ButtonKind Kind { get => _kind; set { _kind = value; Invalidate(); } }

    [Category("Pawfect"), Description("Icon character from the Segoe MDL2 Assets font.")]
    [DefaultValue("")]
    public string Glyph { get => _glyph; set { _glyph = value; Invalidate(); } }

    [Category("Pawfect"), Description("Show the button as the chosen option (Outline kind).")]
    [DefaultValue(false)]
    public bool Selected { get => _selected; set { _selected = value; Invalidate(); } }

    protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }
    protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }
    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; _down = false; Invalidate(); base.OnMouseLeave(e); }
    protected override void OnMouseDown(MouseEventArgs e) { _down = true; Invalidate(); base.OnMouseDown(e); }
    protected override void OnMouseUp(MouseEventArgs e) { _down = false; Invalidate(); base.OnMouseUp(e); }

    private (Color Fill, Color Border, Color Fore, Color Icon) Look()
    {
        if (!Enabled)
            return (Theme.Subtle, Theme.Border, Color.FromArgb(190, 200, 214), Color.FromArgb(190, 200, 214));

        switch (_kind)
        {
            case ButtonKind.Primary:
                var p = _down ? Color.FromArgb(40, 98, 178) : _hover ? Color.FromArgb(47, 107, 192) : Theme.Primary;
                return (p, p, Color.White, Color.White);
            case ButtonKind.Soft:
                var s = _hover ? Color.FromArgb(214, 230, 250) : Theme.PrimarySoft;
                return (s, s, Theme.Primary, Theme.Primary);
            case ButtonKind.Danger:
                return (_hover ? Color.FromArgb(252, 226, 226) : Color.FromArgb(253, 238, 238),
                    Color.FromArgb(246, 192, 192), Theme.Danger, Theme.Danger);
            default:
                if (_selected)
                    return (Theme.PrimarySoft, Theme.Primary, Theme.Primary, Theme.Primary);
                return (_hover ? Theme.Subtle : Color.White, Theme.Border, Theme.TextSoft, Theme.Primary);
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var (fill, border, fore, icon) = Look();

        using (var path = Ui.RoundRect(new RectangleF(0.5f, 0.5f, Width - 2, Height - 2), Height < 30 ? 6 : 8))
        using (var b = new SolidBrush(fill))
        using (var pen = new Pen(border))
        {
            g.FillPath(b, path);
            g.DrawPath(pen, path);
        }

        var font = _selected || _kind == ButtonKind.Primary ? new Font(Font, FontStyle.Bold) : Font;
        try
        {
            int glyphW = _glyph.Length > 0 ? 18 : 0;
            int textW = Text.Length > 0
                ? TextRenderer.MeasureText(g, Text, font, new Size(int.MaxValue, Height), Tf.Left).Width
                : 0;
            int gap = glyphW > 0 && textW > 0 ? 6 : 0;
            int total = Math.Min(Width - 8, glyphW + gap + textW);
            int x = (Width - total) / 2;

            if (glyphW > 0)
                TextRenderer.DrawText(g, _glyph, Theme.GlyphFont(Math.Max(8f, Font.SizeInPoints)),
                    new Rectangle(x, 0, glyphW, Height), icon, Tf.Center);
            if (textW > 0)
                TextRenderer.DrawText(g, Text, font,
                    new Rectangle(x + glyphW + gap, 0, Math.Max(0, total - glyphW - gap), Height), fore, Tf.Left);
        }
        finally
        {
            if (!ReferenceEquals(font, Font)) font.Dispose();
        }
    }
}

/// <summary>Pill-shaped switch with one segment per entry in <see cref="Segments"/> (Cash / E-Wallet, Retail / Services ...).</summary>
internal sealed class SegmentedControl : Control
{
    private string[] _segments = { "Option 1", "Option 2" };
    private int _index;
    private int _hover = -1;

    public SegmentedControl()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        Font = Theme.Face(8.5f, FontStyle.Bold);
        BackColor = Color.White;
        Cursor = Cursors.Hand;
        Size = new Size(250, 36);
    }

    public event EventHandler? SelectedIndexChanged;

    [Category("Pawfect"), Description("The label of each segment, in order.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string[] Segments
    {
        get => _segments;
        set
        {
            _segments = value ?? Array.Empty<string>();
            if (_index >= _segments.Length) _index = Math.Max(0, _segments.Length - 1);
            Invalidate();
        }
    }

    [Category("Pawfect"), Description("Which segment is selected.")]
    [DefaultValue(0)]
    public int SelectedIndex
    {
        get => _index;
        set
        {
            int v = _segments.Length == 0 ? 0 : Math.Clamp(value, 0, _segments.Length - 1);
            if (v == _index) return;
            _index = v;
            Invalidate();
            SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private RectangleF SegmentRect(int i)
    {
        float w = (Width - 5f) / Math.Max(1, _segments.Length);
        return new RectangleF(2 + i * w, 2, w, Height - 5);
    }

    private int IndexAt(Point p)
    {
        for (int i = 0; i < _segments.Length; i++)
            if (SegmentRect(i).Contains(p)) return i;
        return -1;
    }

    protected override void OnMouseLeave(EventArgs e) { _hover = -1; Invalidate(); base.OnMouseLeave(e); }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        int i = IndexAt(e.Location);
        if (i != _hover) { _hover = i; Invalidate(); }
        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (e.Button != MouseButtons.Left) return;
        int i = IndexAt(e.Location);
        if (i >= 0) SelectedIndex = i;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var track = Ui.RoundRect(new RectangleF(0.5f, 0.5f, Width - 2, Height - 2), 8))
        using (var b = new SolidBrush(Theme.Track))
            g.FillPath(b, track);

        for (int i = 0; i < _segments.Length; i++)
        {
            var r = SegmentRect(i);
            bool active = i == _index;

            if (active || i == _hover)
            {
                using var path = Ui.RoundRect(r, 6);
                using var b = new SolidBrush(active ? Theme.Primary : Theme.SubtleHover);
                g.FillPath(b, path);
            }

            TextRenderer.DrawText(g, _segments[i], Font, Rectangle.Round(r),
                active ? Color.White : Theme.TextSoft, Tf.Center);
        }
    }
}

/// <summary>Text box that can show a grey hint while empty, including in multi-line mode (where PlaceholderText doesn't work).</summary>
internal sealed class HintTextBox : TextBox
{
    private const int WmPaint = 0x000F;

    [Category("Pawfect"), Description("Grey hint shown while the box is empty.")]
    [DefaultValue("")]
    public string Hint { get; set; } = "";

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);
        if (m.Msg == WmPaint && Text.Length == 0 && Hint.Length > 0 && IsHandleCreated)
        {
            using var g = CreateGraphics();
            TextRenderer.DrawText(g, Hint, Font, new Rectangle(1, 0, Math.Max(0, ClientSize.Width - 2), ClientSize.Height),
                Theme.Muted, TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
        }
    }
}

/// <summary>
/// One row in the "Add Items / Services" list (and in the appointment search results):
/// icon, title, subtitle, optional price and an action button.
/// </summary>
internal sealed class CatalogRowView : Control
{
    public const int RowHeight = 48;

    private readonly string _glyph;
    private readonly string _title;
    private readonly string _subtitle;
    private readonly string _price;
    private readonly string _button;
    private bool _hover;
    private bool _hoverButton;

    public CatalogRowView(object source, string glyph, string title, string subtitle, string priceText, string buttonText)
    {
        Source = source;
        _glyph = glyph;
        _title = title;
        _subtitle = subtitle;
        _price = priceText;
        _button = buttonText;
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        Height = RowHeight;
        TabStop = false;
    }

    public object Source { get; }

    public event EventHandler? ActionClicked;

    private Rectangle ButtonRect
    {
        get
        {
            int w = Math.Max(44, TextRenderer.MeasureText(_button, Theme.Face(8.5f, FontStyle.Bold)).Width + 22);
            return new Rectangle(Width - 4 - w, (Height - 24) / 2, w, 24);
        }
    }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }

    protected override void OnMouseLeave(EventArgs e)
    {
        _hover = false;
        _hoverButton = false;
        Cursor = Cursors.Default;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        bool over = ButtonRect.Contains(e.Location);
        if (over != _hoverButton)
        {
            _hoverButton = over;
            Cursor = over ? Cursors.Hand : Cursors.Default;
            Invalidate();
        }
        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (e.Button == MouseButtons.Left && ButtonRect.Contains(e.Location))
            ActionClicked?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var bg = new SolidBrush(_hover ? Theme.Subtle : Color.White))
            g.FillRectangle(bg, ClientRectangle);

        // icon tile
        var tile = new RectangleF(4, (Height - 34) / 2f, 34, 34);
        using (var path = Ui.RoundRect(tile, 8))
        using (var b = new SolidBrush(Theme.PrimarySoft))
            g.FillPath(b, path);
        TextRenderer.DrawText(g, _glyph, Theme.GlyphFont(10f), Rectangle.Round(tile), Theme.Primary, Tf.Center);

        var button = ButtonRect;
        int right = button.X - 8;

        if (_price.Length > 0)
        {
            var priceRect = new Rectangle(right - 64, 0, 64, Height);
            TextRenderer.DrawText(g, _price, Theme.Face(8.5f, FontStyle.Bold), priceRect, Theme.Text, Tf.Right);
            right = priceRect.X - 6;
        }

        int textX = 46;
        int textW = Math.Max(10, right - textX);
        if (_subtitle.Length > 0)
        {
            TextRenderer.DrawText(g, _title, Theme.Face(8.5f, FontStyle.Bold), new Rectangle(textX, 7, textW, 18), Theme.Text, Tf.Left);
            TextRenderer.DrawText(g, _subtitle, Theme.Face(7.5f), new Rectangle(textX, 25, textW, 16), Theme.Muted, Tf.Left);
        }
        else
        {
            TextRenderer.DrawText(g, _title, Theme.Face(8.5f, FontStyle.Bold), new Rectangle(textX, 0, textW, Height), Theme.Text, Tf.Left);
        }

        using (var path = Ui.RoundRect(new RectangleF(button.X, button.Y, button.Width - 1, button.Height - 1), 6))
        using (var b = new SolidBrush(_hoverButton ? Color.FromArgb(214, 230, 250) : Theme.PrimarySoft))
            g.FillPath(b, path);
        TextRenderer.DrawText(g, _button, Theme.Face(8.5f, FontStyle.Bold), button, Theme.Primary, Tf.Center);
    }
}

/// <summary>One line of the transaction cart. Column widths mirror the header row on the page.</summary>
internal sealed class CartRowView : Control
{
    public const int DeleteColumnWidth = 28;
    public const int RowHeight = 50;

    /// <summary>Share of the remaining width for: item, type, price, quantity, subtotal (must add up to 100).</summary>
    public static readonly float[] ColumnPercents = { 30f, 15f, 14f, 16f, 25f };

    private int _hoverPart;   // 0 none, 1 minus, 2 plus, 3 delete
    private bool _hover;

    public CartRowView(CartLine line)
    {
        Line = line;
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        Height = RowHeight;
        TabStop = false;
    }

    public CartLine Line { get; }

    /// <summary>Raised with +1 or -1 when the quantity buttons are used.</summary>
    public event EventHandler<int>? QuantityStepped;
    public event EventHandler? RemoveClicked;

    private Rectangle Col(int i)
    {
        float avail = Math.Max(0, Width - DeleteColumnWidth);
        float x = 0;
        for (int j = 0; j < i; j++) x += avail * ColumnPercents[j] / 100f;
        return new Rectangle((int)x, 0, (int)(avail * ColumnPercents[i] / 100f), Height);
    }

    private Rectangle QtyBox
    {
        get
        {
            var c = Col(3);
            return new Rectangle(c.X + (c.Width - 66) / 2, (Height - 22) / 2, 66, 22);
        }
    }

    private Rectangle MinusRect => new(QtyBox.X, QtyBox.Y, 20, QtyBox.Height);
    private Rectangle PlusRect => new(QtyBox.Right - 20, QtyBox.Y, 20, QtyBox.Height);
    private Rectangle DeleteRect => new(Width - DeleteColumnWidth, 0, DeleteColumnWidth, Height);

    private bool HasStepper => Line.Kind == CartLineKind.Product;

    private int PartAt(Point p)
    {
        if (HasStepper && MinusRect.Contains(p)) return 1;
        if (HasStepper && PlusRect.Contains(p)) return 2;
        if (DeleteRect.Contains(p)) return 3;
        return 0;
    }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }

    protected override void OnMouseLeave(EventArgs e)
    {
        _hover = false;
        _hoverPart = 0;
        Cursor = Cursors.Default;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        int part = PartAt(e.Location);
        if (part != _hoverPart)
        {
            _hoverPart = part;
            Cursor = part > 0 ? Cursors.Hand : Cursors.Default;
            Invalidate();
        }
        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (e.Button != MouseButtons.Left) return;
        switch (PartAt(e.Location))
        {
            case 1: if (Line.Quantity > 1) QuantityStepped?.Invoke(this, -1); break;
            case 2: QuantityStepped?.Invoke(this, 1); break;
            case 3: RemoveClicked?.Invoke(this, EventArgs.Empty); break;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var bg = new SolidBrush(_hover ? Theme.Subtle : Color.White))
            g.FillRectangle(bg, ClientRectangle);
        using (var line = new Pen(Theme.Border))
            g.DrawLine(line, 0, Height - 1, Width, Height - 1);

        // Item / service
        var c0 = Col(0);
        if (Line.Detail.Length > 0)
        {
            TextRenderer.DrawText(g, Line.Name, Theme.Face(8.5f, FontStyle.Bold),
                new Rectangle(c0.X + 4, 8, c0.Width - 8, 18), Theme.Text, Tf.Left);
            TextRenderer.DrawText(g, "(" + Line.Detail + ")", Theme.Face(7.5f),
                new Rectangle(c0.X + 4, 26, c0.Width - 8, 16), Theme.Muted, Tf.Left);
        }
        else
        {
            TextRenderer.DrawText(g, Line.Name, Theme.Face(8.5f, FontStyle.Bold),
                new Rectangle(c0.X + 4, 0, c0.Width - 8, Height), Theme.Text, Tf.Left);
        }

        // Type badge
        if (Line.Kind == CartLineKind.Service)
            Pills.DrawCentered(g, "Service", Theme.Primary, Theme.PrimarySoft, Color.FromArgb(200, 220, 247), Col(1), 7.5f, 18);
        else
            Pills.DrawCentered(g, "Product", Theme.Green, Theme.GreenSoft, Color.FromArgb(191, 229, 206), Col(1), 7.5f, 18);

        // Unit price
        TextRenderer.DrawText(g, Money.Format(Line.UnitPrice), Theme.Face(8.5f), Col(2), Theme.TextSoft, Tf.Center);

        // Quantity
        if (HasStepper)
        {
            var box = QtyBox;
            using (var path = Ui.RoundRect(new RectangleF(box.X + 0.5f, box.Y + 0.5f, box.Width - 1, box.Height - 1), 5))
            using (var pen = new Pen(Theme.Border))
                g.DrawPath(pen, path);

            bool canMinus = Line.Quantity > 1;
            TextRenderer.DrawText(g, "-", Theme.Face(9f, FontStyle.Bold), MinusRect,
                !canMinus ? Color.FromArgb(197, 207, 222) : _hoverPart == 1 ? Theme.Primary : Theme.TextSoft, Tf.Center);
            TextRenderer.DrawText(g, Line.Quantity.ToString(CultureInfo.CurrentCulture), Theme.Face(8.5f, FontStyle.Bold),
                new Rectangle(box.X + 20, box.Y, box.Width - 40, box.Height), Theme.Text, Tf.Center);
            TextRenderer.DrawText(g, "+", Theme.Face(9f, FontStyle.Bold), PlusRect,
                _hoverPart == 2 ? Theme.Primary : Theme.TextSoft, Tf.Center);
        }
        else
        {
            TextRenderer.DrawText(g, Line.Quantity.ToString(CultureInfo.CurrentCulture), Theme.Face(8.5f), Col(3), Theme.TextSoft, Tf.Center);
        }

        // Subtotal
        var c4 = Col(4);
        TextRenderer.DrawText(g, Money.Format(Line.Subtotal), Theme.Face(8.5f, FontStyle.Bold),
            new Rectangle(c4.X, 0, c4.Width - 6, Height), Theme.Text, Tf.Right);

        // Remove
        if (_hoverPart == 3)
        {
            using var b = new SolidBrush(Color.FromArgb(253, 232, 232));
            g.FillEllipse(b, DeleteRect.X + 2, (Height - 24) / 2, 24, 24);
        }
        TextRenderer.DrawText(g, "\uE74D", Theme.GlyphFont(9f), DeleteRect, Theme.Danger, Tf.Center);
    }
}

/// <summary>One row of the "Recent Transactions" table. Column widths mirror the header row on the page.</summary>
internal sealed class TransactionRowView : Control
{
    public const int RowHeight = 46;

    /// <summary>Share of the width for: receipt, date, customer, items, total, method, status, actions (must add up to 100).</summary>
    public static readonly float[] ColumnPercents = { 15f, 17f, 14f, 16f, 11f, 11f, 9f, 7f };

    private bool _hover;
    private bool _hoverView;

    public TransactionRowView(TransactionItem item)
    {
        Item = item;
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        Height = RowHeight;
        TabStop = false;
    }

    public TransactionItem Item { get; }

    public event EventHandler? ViewClicked;

    private Rectangle Col(int i)
    {
        float x = 0;
        for (int j = 0; j < i; j++) x += Width * ColumnPercents[j] / 100f;
        return new Rectangle((int)x, 0, (int)(Width * ColumnPercents[i] / 100f), Height);
    }

    private Rectangle ViewRect
    {
        get
        {
            var c = Col(7);
            int w = Math.Max(30, Math.Min(48, c.Width - 4));
            return new Rectangle(c.X + (c.Width - w) / 2, (Height - 24) / 2, w, 24);
        }
    }

    private static string Summary(TransactionItem t)
    {
        var parts = new List<string>();
        if (t.ServiceCount > 0) parts.Add($"{t.ServiceCount} {(t.ServiceCount == 1 ? "service" : "services")}");
        if (t.ItemCount > 0) parts.Add($"{t.ItemCount} {(t.ItemCount == 1 ? "item" : "items")}");
        return parts.Count == 0 ? "-" : string.Join(", ", parts);
    }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }

    protected override void OnMouseLeave(EventArgs e)
    {
        _hover = false;
        _hoverView = false;
        Cursor = Cursors.Default;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        bool over = ViewRect.Contains(e.Location);
        if (over != _hoverView)
        {
            _hoverView = over;
            Cursor = over ? Cursors.Hand : Cursors.Default;
            Invalidate();
        }
        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (e.Button == MouseButtons.Left && ViewRect.Contains(e.Location))
            ViewClicked?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var culture = CultureInfo.CurrentCulture;

        using (var bg = new SolidBrush(_hover ? Theme.Subtle : Color.White))
            g.FillRectangle(bg, ClientRectangle);
        using (var line = new Pen(Theme.Border))
            g.DrawLine(line, 0, Height - 1, Width, Height - 1);

        var textFont = Theme.Face(8.5f);
        var boldFont = Theme.Face(8.5f, FontStyle.Bold);

        Rectangle Pad(Rectangle r) => new(r.X + 4, r.Y, Math.Max(0, r.Width - 8), r.Height);

        TextRenderer.DrawText(g, Item.ReceiptNumber, textFont, Pad(Col(0)), Theme.Primary, Tf.Left);
        TextRenderer.DrawText(g, Item.When.ToString("MMM d, yyyy h:mm tt", culture), textFont, Pad(Col(1)), Theme.TextSoft, Tf.Left);
        TextRenderer.DrawText(g, Item.CustomerName, boldFont, Pad(Col(2)), Theme.Text, Tf.Left);
        TextRenderer.DrawText(g, Summary(Item), textFont, Pad(Col(3)), Theme.TextSoft, Tf.Left);
        TextRenderer.DrawText(g, Money.Format(Item.Total), boldFont, Pad(Col(4)), Theme.Text, Tf.Left);

        if (Item.PaymentMethod.Length > 0)
        {
            bool cash = string.Equals(Item.PaymentMethod, "Cash", StringComparison.OrdinalIgnoreCase);
            Pills.DrawCentered(g, Item.PaymentMethod,
                cash ? Theme.Green : Theme.Primary,
                cash ? Theme.GreenSoft : Theme.PrimarySoft,
                cash ? Color.FromArgb(191, 229, 206) : Color.FromArgb(200, 220, 247), Col(5), 7.5f, 20);
        }

        var (fore, fill, border) = TransactionStatusStyle.Pill(Item.Status);
        Pills.DrawCentered(g, TransactionStatusStyle.Label(Item.Status), fore, fill, border, Col(6), 7.5f, 20);

        var view = ViewRect;
        using (var path = Ui.RoundRect(new RectangleF(view.X + 0.5f, view.Y + 0.5f, view.Width - 1, view.Height - 1), 6))
        using (var b = new SolidBrush(_hoverView ? Theme.PrimarySoft : Color.White))
        using (var pen = new Pen(_hoverView ? Theme.Primary : Theme.Border))
        {
            g.FillPath(b, path);
            g.DrawPath(pen, path);
        }
        TextRenderer.DrawText(g, "View", Theme.Face(8f, FontStyle.Bold), view, Theme.Primary, Tf.Center);
    }
}

/// <summary>The appointment picked for this sale: customer, pet, service and status.</summary>
internal sealed class CustomerCardView : Control
{
    private AppointmentItem? _item;
    private int _hoverPart;   // 0 none, 1 view profile, 2 clear

    public CustomerCardView()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        Size = new Size(248, 128);
        TabStop = false;
    }

    public event EventHandler? ViewProfileClicked;
    public event EventHandler? ClearClicked;

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public AppointmentItem? Item
    {
        get => _item;
        set { _item = value; Invalidate(); }
    }

    private Rectangle ViewRect => new(Width - 12 - 22 - 6 - 70, 12, 70, 22);
    private Rectangle ClearRect => new(Width - 12 - 20, 11, 20, 24);

    private int PartAt(Point p) => ViewRect.Contains(p) ? 1 : ClearRect.Contains(p) ? 2 : 0;

    protected override void OnMouseLeave(EventArgs e) { _hoverPart = 0; Cursor = Cursors.Default; Invalidate(); base.OnMouseLeave(e); }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        int part = _item is null ? 0 : PartAt(e.Location);
        if (part != _hoverPart)
        {
            _hoverPart = part;
            Cursor = part > 0 ? Cursors.Hand : Cursors.Default;
            Invalidate();
        }
        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (e.Button != MouseButtons.Left || _item is null) return;
        switch (PartAt(e.Location))
        {
            case 1: ViewProfileClicked?.Invoke(this, EventArgs.Empty); break;
            case 2: ClearClicked?.Invoke(this, EventArgs.Empty); break;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        if (_item is null) return;
        var item = _item;

        using (var path = Ui.RoundRect(new RectangleF(0.5f, 0.5f, Width - 2, Height - 2), 8))
        using (var fill = new SolidBrush(Color.FromArgb(243, 248, 254)))
        using (var pen = new Pen(Color.FromArgb(207, 224, 247)))
        {
            g.FillPath(fill, path);
            g.DrawPath(pen, path);
        }

        // avatar
        using (var b = new SolidBrush(Theme.PrimarySoft))
            g.FillEllipse(b, 12, 10, 34, 34);
        TextRenderer.DrawText(g, "\uE77B", Theme.GlyphFont(11f), new Rectangle(12, 10, 34, 34), Theme.Primary, Tf.Center);

        var view = ViewRect;
        int nameW = Math.Max(10, view.X - 56 - 4);
        TextRenderer.DrawText(g, item.CustomerName, Theme.Face(9f, FontStyle.Bold), new Rectangle(54, 10, nameW, 18), Theme.Text, Tf.Left);
        TextRenderer.DrawText(g, item.CustomerPhone, Theme.Face(8f), new Rectangle(54, 28, nameW, 16), Theme.Muted, Tf.Left);

        // View Profile
        using (var path = Ui.RoundRect(new RectangleF(view.X + 0.5f, view.Y + 0.5f, view.Width - 1, view.Height - 1), 6))
        using (var b = new SolidBrush(_hoverPart == 1 ? Theme.PrimarySoft : Color.White))
        using (var pen = new Pen(_hoverPart == 1 ? Theme.Primary : Color.FromArgb(197, 207, 222)))
        {
            g.FillPath(b, path);
            g.DrawPath(pen, path);
        }
        TextRenderer.DrawText(g, "View Profile", Theme.Face(7.5f, FontStyle.Bold), view, Theme.Primary, Tf.Center);

        // Clear
        TextRenderer.DrawText(g, "\uE711", Theme.GlyphFont(8f), ClearRect,
            _hoverPart == 2 ? Theme.Danger : Theme.Muted, Tf.Center);

        using (var line = new Pen(Color.FromArgb(214, 228, 247)))
            g.DrawLine(line, 12, 54, Width - 12, 54);

        // details
        string pet = item.PetBreed.Length > 0 ? $"{item.PetName} ({item.PetBreed})" : item.PetName;
        TextRenderer.DrawText(g, "Pet:", Theme.Face(8f), new Rectangle(12, 60, 50, 18), Theme.Muted, Tf.Left);
        TextRenderer.DrawText(g, pet, Theme.Face(8f, FontStyle.Bold), new Rectangle(60, 60, Width - 72, 18), Theme.Text, Tf.Right);
        TextRenderer.DrawText(g, "Service:", Theme.Face(8f), new Rectangle(12, 80, 60, 18), Theme.Muted, Tf.Left);
        TextRenderer.DrawText(g, item.Service, Theme.Face(8f, FontStyle.Bold), new Rectangle(72, 80, Width - 84, 18), Theme.Text, Tf.Right);

        var (fore, back, border) = AppointmentStatusStyle.Pill(item.Status);
        string when = item.Start.ToString("h:mm tt", CultureInfo.CurrentCulture);
        Pills.DrawAt(g, $"{when} - {AppointmentStatusStyle.Label(item.Status)}", fore, back, border,
            12, 112, Width - 24, 7.5f, 20);
    }
}

/// <summary>Dashed-border receipt preview. Shows only the values it is given; blanks show a dash.</summary>
internal sealed class ReceiptPreview : Control
{
    private string _storeName = "Pawfect";
    private string _tagline = "Pet Care. Made Simple.";
    private string _address = "";
    private string _phone = "";
    private string _receiptNumber = "";
    private DateTime? _date;
    private string _cashier = "";

    public ReceiptPreview()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        Size = new Size(268, 170);
        TabStop = false;
    }

    [Category("Pawfect"), DefaultValue("Pawfect")]
    public string StoreName { get => _storeName; set { _storeName = value; Invalidate(); } }

    [Category("Pawfect"), DefaultValue("Pet Care. Made Simple.")]
    public string Tagline { get => _tagline; set { _tagline = value; Invalidate(); } }

    [Category("Pawfect"), Description("Shop address line. Hidden while empty.")]
    [DefaultValue("")]
    public string Address { get => _address; set { _address = value; Invalidate(); } }

    [Category("Pawfect"), Description("Shop phone line. Hidden while empty.")]
    [DefaultValue("")]
    public string Phone { get => _phone; set { _phone = value; Invalidate(); } }

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string ReceiptNumber { get => _receiptNumber; set { _receiptNumber = value; Invalidate(); } }

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DateTime? ReceiptDate { get => _date; set { _date = value; Invalidate(); } }

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string Cashier { get => _cashier; set { _cashier = value; Invalidate(); } }

    private static void DrawPaw(Graphics g, float x, float y, float s, Color color)
    {
        using var brush = new SolidBrush(color);
        void Blob(float cx, float cy, float w, float h)
            => g.FillEllipse(brush, x + (cx - w / 2f) * s, y + (cy - h / 2f) * s, w * s, h * s);
        Blob(0.50f, 0.70f, 0.52f, 0.42f);
        Blob(0.12f, 0.43f, 0.20f, 0.26f);
        Blob(0.36f, 0.20f, 0.20f, 0.28f);
        Blob(0.64f, 0.20f, 0.20f, 0.28f);
        Blob(0.88f, 0.43f, 0.20f, 0.26f);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var path = Ui.RoundRect(new RectangleF(0.5f, 0.5f, Width - 2, Height - 2), 8))
        using (var fill = new SolidBrush(Color.FromArgb(250, 251, 253)))
        using (var pen = new Pen(Color.FromArgb(197, 207, 222)) { DashStyle = DashStyle.Dash })
        {
            g.FillPath(fill, path);
            g.DrawPath(pen, path);
        }

        int y = 12;

        var nameFont = Theme.Face(8.5f, FontStyle.Bold);
        int nameW = TextRenderer.MeasureText(_storeName, nameFont).Width;
        int total = 14 + 4 + nameW;
        int x0 = (Width - total) / 2;
        DrawPaw(g, x0, y + 1, 14, Theme.Primary);
        TextRenderer.DrawText(g, _storeName, nameFont, new Rectangle(x0 + 18, y, nameW + 4, 16), Theme.Text, Tf.Left);
        y += 20;

        var small = Theme.Face(7.5f);
        void Centered(string text)
        {
            if (text.Length == 0) return;
            TextRenderer.DrawText(g, text, small, new Rectangle(8, y, Width - 16, 14), Theme.Muted, Tf.Center);
            y += 15;
        }
        Centered(_tagline);
        Centered(_address);
        Centered(_phone);

        void Divider()
        {
            using var pen = new Pen(Color.FromArgb(197, 207, 222)) { DashStyle = DashStyle.Dash };
            g.DrawLine(pen, 12, y + 4, Width - 12, y + 4);
            y += 10;
        }

        y += 2;
        Divider();
        TextRenderer.DrawText(g, "OFFICIAL RECEIPT", Theme.Face(7.5f, FontStyle.Bold),
            new Rectangle(8, y, Width - 16, 16), Theme.Text, Tf.Center);
        y += 18;
        Divider();

        void Row(string label, string value)
        {
            TextRenderer.DrawText(g, label, small, new Rectangle(14, y, 70, 16), Theme.Muted, Tf.Left);
            TextRenderer.DrawText(g, value.Length > 0 ? value : "-", Theme.Face(7.5f, FontStyle.Bold),
                new Rectangle(84, y, Width - 98, 16), Theme.Text, Tf.Right);
            y += 17;
        }

        Row("Receipt No.:", _receiptNumber);
        Row("Date:", _date?.ToString("MMM d, yyyy h:mm tt", CultureInfo.CurrentCulture) ?? "");
        Row("Cashier:", _cashier);
    }
}

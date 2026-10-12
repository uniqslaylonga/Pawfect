using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace MyApp.Controls;

/// <summary>Small rounded tick box (used in the table header and in every row).</summary>
internal sealed class CheckMark : Control
{
    private bool _checked;
    private bool _hover;

    public CheckMark()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        Cursor = Cursors.Hand;
        Size = new Size(18, 18);
        TabStop = false;
    }

    public event EventHandler? CheckedChanged;

    [Category("Pawfect"), Description("Whether the box is ticked.")]
    [DefaultValue(false)]
    public bool Checked
    {
        get => _checked;
        set
        {
            if (_checked == value) return;
            _checked = value;
            Invalidate();
            CheckedChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }
    protected override void OnClick(EventArgs e) { Checked = !Checked; base.OnClick(e); }

    protected override void OnPaint(PaintEventArgs e) => Draw(e.Graphics, ClientRectangle, _checked, _hover);

    /// <summary>Draws the box centred inside <paramref name="bounds"/>.</summary>
    public static void Draw(Graphics g, Rectangle bounds, bool isChecked, bool hover)
    {
        var old = g.SmoothingMode;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        var r = new Rectangle(bounds.X + (bounds.Width - 16) / 2, bounds.Y + (bounds.Height - 16) / 2, 16, 16);
        using (var path = Ui.RoundRect(new RectangleF(r.X + 0.5f, r.Y + 0.5f, r.Width - 1, r.Height - 1), 4))
        using (var fill = new SolidBrush(isChecked ? Theme.Primary : Color.White))
        using (var pen = new Pen(isChecked || hover ? Theme.Primary : Color.FromArgb(197, 207, 222)))
        {
            g.FillPath(fill, path);
            g.DrawPath(pen, path);
        }
        g.SmoothingMode = old;

        if (isChecked)
        {
            TextRenderer.DrawText(g, "\uE73E", Theme.GlyphFont(7f), r, Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
        }
    }
}

/// <summary>Rounded drop-down used by the filter bar ("All Statuses", "All Services").</summary>
internal sealed class SelectField : Control
{
    private readonly List<string> _items = new();
    private ContextMenuStrip? _menu;
    private int _index = -1;
    private bool _hover;

    public SelectField()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        Font = Theme.Face(9f);
        BackColor = Color.White;
        Cursor = Cursors.Hand;
        Size = new Size(120, 36);
    }

    public event EventHandler? SelectedIndexChanged;

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IReadOnlyList<string> Items => _items;

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int SelectedIndex
    {
        get => _index;
        set
        {
            int v = _items.Count == 0 ? -1 : Math.Clamp(value, 0, _items.Count - 1);
            if (v == _index) return;
            _index = v;
            Invalidate();
            SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string? SelectedItem => _index >= 0 && _index < _items.Count ? _items[_index] : null;

    /// <summary>Replaces the list. Keeps the current choice when it still exists, otherwise selects the first entry.</summary>
    public void SetItems(IEnumerable<string> items)
    {
        string? previous = SelectedItem;
        _items.Clear();
        _items.AddRange(items);
        _index = _items.Count == 0 ? -1 : 0;
        if (previous is not null)
        {
            int found = _items.FindIndex(i => string.Equals(i, previous, StringComparison.OrdinalIgnoreCase));
            if (found >= 0) _index = found;
        }
        Invalidate();
    }

    protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }
    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

    protected override void OnClick(EventArgs e)
    {
        base.OnClick(e);
        if (_items.Count == 0) return;

        _menu?.Dispose();
        _menu = new ContextMenuStrip { ShowImageMargin = false, Font = Theme.Face(9f), MinimumSize = new Size(Width, 0) };
        for (int i = 0; i < _items.Count; i++)
        {
            int idx = i;
            var entry = new ToolStripMenuItem(_items[i]) { Checked = i == _index };
            entry.Click += (_, _) => SelectedIndex = idx;
            _menu.Items.Add(entry);
        }
        _menu.Show(this, new Point(0, Height + 2));
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var path = Ui.RoundRect(new RectangleF(0.5f, 0.5f, Width - 2, Height - 2), 8))
        using (var fill = new SolidBrush(_hover ? Theme.Subtle : Color.White))
        using (var pen = new Pen(Theme.Border))
        {
            g.FillPath(fill, path);
            g.DrawPath(pen, path);
        }

        string text = SelectedItem ?? Text;
        TextRenderer.DrawText(g, text, Font, new Rectangle(12, 0, Width - 34, Height), Theme.TextSoft,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
            TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);

        TextRenderer.DrawText(g, "\uE70D", Theme.GlyphFont(7f), new Rectangle(Width - 26, 0, 18, Height), Theme.Muted,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
    }
}

/// <summary>Date-range chip with previous / next arrows. Set <see cref="Control.Text"/> to the range to show.</summary>
internal sealed class DateRangeField : Control
{
    private bool _hover;
    private int _hoverPart = -1; // 0 = text, 1 = previous, 2 = next

    public DateRangeField()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        Font = Theme.Face(8.5f);
        BackColor = Color.White;
        Cursor = Cursors.Hand;
        Size = new Size(190, 36);
    }

    public event EventHandler? PreviousClicked;
    public event EventHandler? NextClicked;
    public event EventHandler? TextClicked;

    private Rectangle PrevRect => new(Width - 50, 5, 22, Height - 10);
    private Rectangle NextRect => new(Width - 28, 5, 22, Height - 10);

    private int PartAt(Point p) => PrevRect.Contains(p) ? 1 : NextRect.Contains(p) ? 2 : 0;

    protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }
    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; _hoverPart = -1; Invalidate(); base.OnMouseLeave(e); }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        int part = PartAt(e.Location);
        if (part != _hoverPart) { _hoverPart = part; Invalidate(); }
        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (e.Button != MouseButtons.Left) return;
        switch (PartAt(e.Location))
        {
            case 1: PreviousClicked?.Invoke(this, EventArgs.Empty); break;
            case 2: NextClicked?.Invoke(this, EventArgs.Empty); break;
            default: TextClicked?.Invoke(this, EventArgs.Empty); break;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var path = Ui.RoundRect(new RectangleF(0.5f, 0.5f, Width - 2, Height - 2), 8))
        using (var fill = new SolidBrush(_hover ? Theme.Subtle : Color.White))
        using (var pen = new Pen(Theme.Border))
        {
            g.FillPath(fill, path);
            g.DrawPath(pen, path);
        }

        const TextFormatFlags glyphFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

        TextRenderer.DrawText(g, "\uE787", Theme.GlyphFont(9f), new Rectangle(8, 0, 22, Height), Theme.Muted, glyphFlags);

        TextRenderer.DrawText(g, Text, Font, new Rectangle(32, 0, Math.Max(10, Width - 32 - 54), Height), Theme.TextSoft,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
            TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);

        TextRenderer.DrawText(g, "\uE76B", Theme.GlyphFont(7f), PrevRect,
            _hoverPart == 1 ? Theme.Primary : Theme.Muted, glyphFlags);
        TextRenderer.DrawText(g, "\uE76C", Theme.GlyphFont(7f), NextRect,
            _hoverPart == 2 ? Theme.Primary : Theme.Muted, glyphFlags);
    }
}

/// <summary>One row of the appointments table. Column widths mirror the header row on the page.</summary>
internal sealed class AppointmentRowView : Control
{
    public const int CheckColumnWidth = 40;
    public const int RowHeight = 46;

    /// <summary>Share of the remaining width for: date, customer, pet, service, status, actions (must add up to 100).</summary>
    public static readonly float[] ColumnPercents = { 15f, 22f, 18f, 14f, 17f, 14f };

    private const TextFormatFlags LineFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
        TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;

    private const TextFormatFlags CenterFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
        TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

    private static readonly string[] ActionGlyphs = { "\uE7B3", "\uE70F", "\uE712" };

    private bool _hover;
    private bool _checked;
    private int _hoverAction = -1;

    public AppointmentRowView(AppointmentItem item)
    {
        Item = item;
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        Height = RowHeight;
        TabStop = false;
    }

    public AppointmentItem Item { get; }

    public event EventHandler? CheckedChanged;
    public event EventHandler? ViewClicked;
    public event EventHandler? EditClicked;
    public event EventHandler? MoreClicked;

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool Checked
    {
        get => _checked;
        set
        {
            if (_checked == value) return;
            _checked = value;
            Invalidate();
            CheckedChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private Rectangle Col(int i)
    {
        float avail = Math.Max(0, Width - CheckColumnWidth);
        float x = CheckColumnWidth;
        for (int j = 0; j < i; j++) x += avail * ColumnPercents[j] / 100f;
        return new Rectangle((int)x, 0, (int)(avail * ColumnPercents[i] / 100f), Height);
    }

    private Rectangle ActionRect(int i)
    {
        var col = Col(5);
        int total = ActionGlyphs.Length * 22 + (ActionGlyphs.Length - 1) * 2;
        int start = Math.Max(col.X, col.Right - 8 - total);
        return new Rectangle(start + i * 24, (Height - 22) / 2, 22, 22);
    }

    private int ActionAt(Point p)
    {
        for (int i = 0; i < ActionGlyphs.Length; i++)
            if (ActionRect(i).Contains(p)) return i;
        return -1;
    }

    private bool OnCheck(Point p) => p.X < CheckColumnWidth;

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }

    protected override void OnMouseLeave(EventArgs e)
    {
        _hover = false;
        _hoverAction = -1;
        Cursor = Cursors.Default;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        int action = ActionAt(e.Location);
        if (action != _hoverAction)
        {
            _hoverAction = action;
            Invalidate();
        }
        Cursor = action >= 0 || OnCheck(e.Location) ? Cursors.Hand : Cursors.Default;
        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (e.Button != MouseButtons.Left) return;

        if (OnCheck(e.Location))
        {
            Checked = !Checked;
            return;
        }

        switch (ActionAt(e.Location))
        {
            case 0: ViewClicked?.Invoke(this, EventArgs.Empty); break;
            case 1: EditClicked?.Invoke(this, EventArgs.Empty); break;
            case 2: MoreClicked?.Invoke(this, EventArgs.Empty); break;
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

        CheckMark.Draw(g, new Rectangle(0, 0, CheckColumnWidth, Height), _checked, false);

        var nameFont = Theme.Face(8.5f, FontStyle.Bold);
        var subFont = Theme.Face(8f);
        var textFont = Theme.Face(8.5f);

        // Date & time
        var c0 = Col(0);
        TextRenderer.DrawText(g, Item.Start.ToString("MMM d, yyyy", CultureInfo.CurrentCulture), nameFont,
            new Rectangle(c0.X + 4, 6, c0.Width - 8, 18), Theme.Text, LineFlags);
        TextRenderer.DrawText(g, Item.Start.ToString("h:mm tt", CultureInfo.CurrentCulture), subFont,
            new Rectangle(c0.X + 4, 24, c0.Width - 8, 16), Theme.Muted, LineFlags);

        // Customer
        var c1 = Col(1);
        int avatarY = (Height - 28) / 2;
        using (var b = new SolidBrush(Theme.PrimarySoft))
            g.FillEllipse(b, c1.X + 4, avatarY, 28, 28);
        TextRenderer.DrawText(g, "\uE77B", Theme.GlyphFont(9f), new Rectangle(c1.X + 4, avatarY, 28, 28), Theme.Primary, CenterFlags);
        TextRenderer.DrawText(g, Item.CustomerName, nameFont,
            new Rectangle(c1.X + 40, 6, c1.Width - 44, 18), Theme.Text, LineFlags);
        TextRenderer.DrawText(g, Item.CustomerPhone, subFont,
            new Rectangle(c1.X + 40, 24, c1.Width - 44, 16), Theme.Muted, LineFlags);

        // Pet
        var c2 = Col(2);
        using (var b = new SolidBrush(Color.FromArgb(254, 243, 224)))
            g.FillEllipse(b, c2.X + 4, avatarY, 28, 28);
        string initial = Item.PetName.Trim().Length > 0 ? Item.PetName.Trim()[..1].ToUpperInvariant() : "";
        TextRenderer.DrawText(g, initial, Theme.Face(9f, FontStyle.Bold), new Rectangle(c2.X + 4, avatarY, 28, 28),
            Color.FromArgb(199, 122, 10), CenterFlags);
        TextRenderer.DrawText(g, Item.PetName, nameFont,
            new Rectangle(c2.X + 40, 6, c2.Width - 44, 18), Theme.Text, LineFlags);
        TextRenderer.DrawText(g, Item.PetBreed, subFont,
            new Rectangle(c2.X + 40, 24, c2.Width - 44, 16), Theme.Muted, LineFlags);

        // Service
        var c3 = Col(3);
        TextRenderer.DrawText(g, Item.Service, textFont,
            new Rectangle(c3.X + 4, 0, c3.Width - 8, Height), Theme.TextSoft, LineFlags);

        // Status
        var c4 = Col(4);
        AppointmentStatusStyle.DrawPill(g, Item.Status, c4.X + 4, Height / 2, c4.Width - 8);

        // Actions
        for (int i = 0; i < ActionGlyphs.Length; i++)
        {
            var r = ActionRect(i);
            bool hot = i == _hoverAction;
            if (hot)
            {
                using var b = new SolidBrush(Theme.PrimarySoft);
                g.FillEllipse(b, r);
            }
            TextRenderer.DrawText(g, ActionGlyphs[i], Theme.GlyphFont(9f), r, hot ? Theme.Primary : Theme.Muted, CenterFlags);
        }
    }
}

/// <summary>One line of the "Upcoming Today" list: time, customer &amp; pet, service and status.</summary>
internal sealed class UpcomingItemView : Control
{
    public const int ItemHeight = 46;

    private const TextFormatFlags LineFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
        TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;

    public UpcomingItemView(AppointmentItem item)
    {
        Item = item;
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        Height = ItemHeight;
        TabStop = false;
    }

    public AppointmentItem Item { get; }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        TextRenderer.DrawText(g, Item.Start.ToString("h:mm tt", CultureInfo.CurrentCulture), Theme.Face(8.5f, FontStyle.Bold),
            new Rectangle(0, 0, 62, Height), Theme.Primary, LineFlags);

        // Reserve room for the status badge on the right.
        var font = Theme.Face(8f, FontStyle.Bold);
        int pillW = TextRenderer.MeasureText(AppointmentStatusStyle.Label(Item.Status), font).Width + 18;
        int textW = Math.Max(20, Width - 66 - pillW - 8);

        TextRenderer.DrawText(g, $"{Item.CustomerName} & {Item.PetName}", Theme.Face(8.5f, FontStyle.Bold),
            new Rectangle(66, 6, textW, 18), Theme.Text, LineFlags);
        TextRenderer.DrawText(g, Item.Service, Theme.Face(8f),
            new Rectangle(66, 24, textW, 16), Theme.Muted, LineFlags);

        AppointmentStatusStyle.DrawPill(g, Item.Status, Width - pillW, Height / 2, pillW);
    }
}

/// <summary>Month grid (Sunday first) with a dot under each day that has appointments.</summary>
internal sealed class MonthCalendarView : Control
{
    private const int NavHeight = 30;
    private const int HeadHeight = 22;

    private readonly Dictionary<DateTime, List<Color>> _markers = new();
    private DateTime _month = new(DateTime.Today.Year, DateTime.Today.Month, 1);
    private DateTime? _selected;

    public MonthCalendarView()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        Font = Theme.Face(8.5f);
        BackColor = Color.White;
        Cursor = Cursors.Default;
        Size = new Size(256, 200);
        TabStop = false;
    }

    public event EventHandler? MonthChanged;
    public event EventHandler<DateTime>? DateClicked;

    /// <summary>First day of the month being shown.</summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DateTime Month
    {
        get => _month;
        set
        {
            var first = new DateTime(value.Year, value.Month, 1);
            if (first == _month) return;
            _month = first;
            Invalidate();
            MonthChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>Day shown with a ring around it (the day the user picked).</summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DateTime? SelectedDate
    {
        get => _selected;
        set { _selected = value?.Date; Invalidate(); }
    }

    /// <summary>Puts a coloured dot under every day that has an appointment.</summary>
    public void SetMarkers(IEnumerable<AppointmentItem> items)
    {
        _markers.Clear();
        foreach (var day in items.GroupBy(i => i.Start.Date))
        {
            _markers[day.Key] = day
                .Select(i => i.Status)
                .Distinct()
                .Select(AppointmentStatusStyle.Marker)
                .Distinct()
                .Take(4)
                .ToList();
        }
        Invalidate();
    }

    private Rectangle PrevRect => new(0, 2, 30, NavHeight - 4);
    private Rectangle NextRect => new(Width - 30, 2, 30, NavHeight - 4);

    private float CellW => Width / 7f;
    private float CellH => Math.Max(20f, (Height - NavHeight - HeadHeight) / 6f);

    private DateTime GridStart => _month.AddDays(-(int)_month.DayOfWeek);

    protected override void OnMouseMove(MouseEventArgs e)
    {
        bool hand = PrevRect.Contains(e.Location) || NextRect.Contains(e.Location) ||
                    e.Y >= NavHeight + HeadHeight;
        Cursor = hand ? Cursors.Hand : Cursors.Default;
        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (e.Button != MouseButtons.Left) return;

        if (PrevRect.Contains(e.Location)) { Month = _month.AddMonths(-1); return; }
        if (NextRect.Contains(e.Location)) { Month = _month.AddMonths(1); return; }

        if (e.Y < NavHeight + HeadHeight) return;
        int row = (int)((e.Y - NavHeight - HeadHeight) / CellH);
        int col = (int)(e.X / CellW);
        if (row < 0 || row > 5 || col < 0 || col > 6) return;

        var date = GridStart.AddDays(row * 7 + col);
        _selected = date;
        Invalidate();
        DateClicked?.Invoke(this, date);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        const TextFormatFlags center = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

        // Month name and arrows
        TextRenderer.DrawText(g, _month.ToString("MMMM yyyy", CultureInfo.CurrentCulture), Theme.Face(9f, FontStyle.Bold),
            new Rectangle(30, 0, Width - 60, NavHeight), Theme.Text, center);
        TextRenderer.DrawText(g, "\uE76B", Theme.GlyphFont(8f), PrevRect, Theme.Muted, center);
        TextRenderer.DrawText(g, "\uE76C", Theme.GlyphFont(8f), NextRect, Theme.Muted, center);

        // Weekday names
        var names = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
        for (int c = 0; c < 7; c++)
        {
            var r = new Rectangle((int)(c * CellW), NavHeight, (int)CellW, HeadHeight);
            TextRenderer.DrawText(g, names[c], Theme.Face(7.5f), r, Theme.Muted, center);
        }

        // Days
        var start = GridStart;
        var today = DateTime.Today;
        float ch = CellH;
        float cw = CellW;
        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 7; col++)
            {
                var date = start.AddDays(row * 7 + col);
                float x = col * cw;
                float y = NavHeight + HeadHeight + row * ch;
                float cx = x + cw / 2f;

                var circle = new RectangleF(cx - 10, y, 20, 20);
                bool isToday = date == today;
                bool isSelected = _selected.HasValue && date == _selected.Value;

                if (isToday)
                {
                    using var b = new SolidBrush(Theme.Primary);
                    g.FillEllipse(b, circle);
                }
                else if (isSelected)
                {
                    using var pen = new Pen(Theme.Primary, 1.5f);
                    g.DrawEllipse(pen, circle.X + 0.75f, circle.Y + 0.75f, circle.Width - 1.5f, circle.Height - 1.5f);
                }

                bool inMonth = date.Month == _month.Month;
                var fore = isToday ? Color.White : inMonth ? Theme.TextSoft : Color.FromArgb(184, 194, 208);
                var font = isToday ? Theme.Face(8.5f, FontStyle.Bold) : Theme.Face(8.5f);
                TextRenderer.DrawText(g, date.Day.ToString(CultureInfo.InvariantCulture), font,
                    Rectangle.Round(circle), fore, center);

                if (_markers.TryGetValue(date, out var dots))
                {
                    float total = dots.Count * 4 + (dots.Count - 1) * 2;
                    float dx = cx - total / 2f;
                    foreach (var color in dots)
                    {
                        using var b = new SolidBrush(color);
                        g.FillEllipse(b, dx, y + 21, 4, 4);
                        dx += 6;
                    }
                }
            }
        }
    }
}

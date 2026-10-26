using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace MyApp.Controls;

// Form controls that match the Pawfect look (rounded, light border, blue focus ring).
// They replace the plain Windows ComboBox / DateTimePicker / Button / CheckBox inside dialogs.

internal enum PawButtonKind { Primary, Outline, Danger }

/// <summary>
/// Rounded button for dialogs. It is still a real <see cref="Button"/>, so AcceptButton, CancelButton and
/// DialogResult keep working.
/// </summary>
internal sealed class PawButton : Button
{
    private PawButtonKind _kind = PawButtonKind.Outline;
    private bool _hover;
    private bool _down;

    public PawButton()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        Cursor = Cursors.Hand;
        Font = Theme.Face(9f, FontStyle.Bold);
        Size = new Size(100, 36);
        UseVisualStyleBackColor = false;
    }

    [Category("Pawfect"), DefaultValue(PawButtonKind.Outline)]
    public PawButtonKind Kind { get => _kind; set { _kind = value; Invalidate(); } }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; _down = false; Invalidate(); base.OnMouseLeave(e); }
    protected override void OnMouseDown(MouseEventArgs e) { _down = true; Invalidate(); base.OnMouseDown(e); }
    protected override void OnMouseUp(MouseEventArgs e) { _down = false; Invalidate(); base.OnMouseUp(e); }
    protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }
    protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
    protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }

    private (Color Fill, Color Border, Color Fore) Look()
    {
        if (!Enabled)
            return (Theme.Subtle, Theme.Border, Color.FromArgb(190, 200, 214));

        switch (_kind)
        {
            case PawButtonKind.Primary:
                var p = _down ? Color.FromArgb(40, 98, 178) : _hover ? Color.FromArgb(47, 107, 192) : Theme.Primary;
                return (p, p, Color.White);
            case PawButtonKind.Danger:
                var d = _down ? Color.FromArgb(200, 40, 40) : _hover ? Color.FromArgb(220, 52, 52) : Theme.Danger;
                return (d, d, Color.White);
            default:
                return (_down ? Theme.SubtleHover : _hover ? Theme.Subtle : Color.White,
                    _hover ? Color.FromArgb(197, 210, 228) : Theme.Border, Theme.TextSoft);
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using (var back = new SolidBrush(Parent?.BackColor ?? Color.White))
            g.FillRectangle(back, ClientRectangle);

        var (fill, border, fore) = Look();
        using (var path = Ui.RoundRect(new RectangleF(0.5f, 0.5f, Width - 2, Height - 2), 8))
        using (var b = new SolidBrush(fill))
        using (var pen = new Pen(border))
        {
            g.FillPath(b, path);
            g.DrawPath(pen, path);
        }

        // Keyboard focus ring (only when tabbing, so mouse clicks stay clean).
        if (Focused && ShowFocusCues && Enabled)
        {
            using var ring = Ui.RoundRect(new RectangleF(2.5f, 2.5f, Width - 6, Height - 6), 6);
            using var pen = new Pen(_kind == PawButtonKind.Outline ? Theme.Primary : Color.FromArgb(200, Color.White));
            g.DrawPath(pen, ring);
        }

        TextRenderer.DrawText(g, Text, Font, ClientRectangle, fore,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
    }
}

/// <summary>Check box drawn as a rounded blue square with a tick.</summary>
internal sealed class PawCheckBox : CheckBox
{
    public PawCheckBox()
    {
        FlatStyle = FlatStyle.Flat;
        Cursor = Cursors.Hand;
        Font = Theme.Face(9f);
        ForeColor = Theme.Text;
        AutoSize = false;
        Height = 28;
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
    }

    protected override void OnCheckedChanged(EventArgs e) { Invalidate(); base.OnCheckedChanged(e); }
    protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
    protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }
    protected override void OnMouseEnter(EventArgs e) { Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { Invalidate(); base.OnMouseLeave(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using (var back = new SolidBrush(BackColor.A == 255 ? BackColor : Parent is { BackColor.A: 255 } ? Parent.BackColor : Color.White))
            g.FillRectangle(back, ClientRectangle);

        // The box follows the font (18 px at 9 pt / 100 %), so it grows with the text on a scaled or full-screen layout.
        int box = Math.Max(18, (int)Math.Round(Font.Height * 1.2));
        int gap = (int)Math.Round(box * 0.55);
        var r = new RectangleF(1.5f, (Height - box) / 2f, box - 1, box - 1);
        using (var path = Ui.RoundRect(r, Math.Max(5, box * 5 / 18)))
        {
            if (Checked)
            {
                using var fill = new SolidBrush(Theme.Primary);
                g.FillPath(fill, path);
            }
            else
            {
                using var fill = new SolidBrush(Color.White);
                g.FillPath(fill, path);
            }
            using var pen = new Pen(Checked || Focused ? Theme.Primary : Color.FromArgb(180, 194, 214), 1.5f);
            g.DrawPath(pen, path);
        }

        if (Checked)
        {
            TextRenderer.DrawText(g, "", Theme.GlyphFont(FullScreen.Pt(8f * box / 18f)), Rectangle.Round(r), Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
        }

        TextRenderer.DrawText(g, Text, Font, new Rectangle(box + gap, 0, Math.Max(0, Width - box - gap), Height), ForeColor,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
            TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
    }
}

/// <summary>Rounded white box around a text box (or similar editor). Border turns blue while it has focus.</summary>
internal static class FieldFrame
{
    /// <summary>Puts <paramref name="inner"/> inside a Pawfect-style rounded frame and returns the frame.</summary>
    public static CardPanel Wrap(Control inner, int height, Padding padding)
    {
        var fill = inner.BackColor == SystemColors.Control || inner.BackColor.A == 0 ? Color.White : inner.BackColor;
        var frame = new CardPanel
        {
            Height = height,
            Radius = 8,
            FillColor = fill,
            CornerColor = Color.White,
            BorderColor = Theme.Border,
            Padding = padding,
            Cursor = Cursors.IBeam,
        };

        inner.BackColor = fill;
        inner.Dock = DockStyle.Fill;
        inner.Margin = Padding.Empty;
        inner.Font = Theme.Face(9f);
        if (inner is TextBox tb) tb.BorderStyle = BorderStyle.None;
        if (inner is NumericUpDown nud) nud.BorderStyle = BorderStyle.None;

        frame.Controls.Add(inner);
        inner.Enter += (_, _) => frame.BorderColor = Theme.Primary;
        inner.Leave += (_, _) => frame.BorderColor = Theme.Border;
        frame.Click += (_, _) => inner.Focus();
        return frame;
    }

    /// <summary>Single-line editors get a 36px frame; multi-line ones grow with their height.</summary>
    public static CardPanel ForEditor(Control inner)
    {
        bool multi = inner is TextBox { Multiline: true };
        return multi
            ? Wrap(inner, Math.Max(inner.Height, 56) + 12, new Padding(10, 6, 6, 6))
            : Wrap(inner, 36, new Padding(10, 9, 8, 0));
    }
}

/// <summary>
/// Base for a field that looks like a text box and opens a small rounded popup when clicked
/// (the drop-down list, the calendar, the time list).
/// </summary>
internal abstract class PawDropField : Control
{
    private const TextFormatFlags LeftFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
        TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;
    private const TextFormatFlags CenterFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
        TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

    private bool _hover;
    private bool _open;
    private long _closedAt;
    private string _glyph = "";
    private string _placeholder = "Select...";

    protected PawDropField()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                 ControlStyles.Selectable | ControlStyles.StandardClick, true);
        TabStop = true;
        Font = Theme.Face(9f);
        BackColor = Color.White;
        ForeColor = Theme.Text;
        Cursor = Cursors.Hand;
        Size = new Size(200, 36);
    }

    [Category("Pawfect"), Description("Icon (Segoe MDL2 Assets) shown at the left of the field.")]
    [DefaultValue("")]
    public string Glyph { get => _glyph; set { _glyph = value; Invalidate(); } }

    [Category("Pawfect"), Description("Grey hint shown while nothing is chosen.")]
    [DefaultValue("Select...")]
    public string Placeholder { get => _placeholder; set { _placeholder = value; Invalidate(); } }

    /// <summary>Text shown in the field ("" = nothing chosen, the placeholder is shown instead).</summary>
    protected abstract string DisplayText { get; }

    /// <summary>Builds what appears inside the popup. <paramref name="width"/> is the width available.</summary>
    protected abstract Control BuildPopup(int width, Action close);

    /// <summary>Arrow keys while the popup is closed (-1 = up, +1 = down).</summary>
    protected virtual void Step(int direction) { }

    /// <summary>True when typing a letter on the focused (closed) field should open the popup with that letter typed in.</summary>
    protected virtual bool OpensOnTyping => false;

    private string _seed = "";

    /// <summary>The letter that opened the popup (empty when it was opened another way). Reading it clears it.</summary>
    protected string TakeSeed()
    {
        var s = _seed;
        _seed = "";
        return s;
    }

    /// <summary>Called once the popup is on screen; the default puts the keyboard focus on its content.</summary>
    protected virtual void OnPopupOpened(Control content) => content.Focus();

    protected override void OnKeyPress(KeyPressEventArgs e)
    {
        base.OnKeyPress(e);
        if (e.Handled || _open || !Enabled || !OpensOnTyping || char.IsControl(e.KeyChar) || e.KeyChar == ' ') return;
        _seed = e.KeyChar.ToString();
        e.Handled = true;
        Open();
    }

    protected override bool IsInputKey(Keys keyData)
        => (keyData & ~Keys.Shift) is Keys.Up or Keys.Down || base.IsInputKey(keyData);

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }
    protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
    protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }
    protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button != MouseButtons.Left) return;
        Focus();
        // Clicking the field while its popup is open closes the popup first; don't instantly reopen it.
        if (Environment.TickCount64 - _closedAt < 250) return;
        Open();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.KeyCode is Keys.Space or Keys.F4 || (e.Alt && e.KeyCode == Keys.Down))
        {
            Open();
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Down) { Step(1); e.Handled = true; }
        else if (e.KeyCode == Keys.Up) { Step(-1); e.Handled = true; }
    }

    private void Open()
    {
        if (_open || !Enabled) return;

        int width = Math.Max(Width, 120);
        ToolStripDropDown drop = null!;
        void Close() => drop.Close();

        var content = BuildPopup(width - 12, Close);
        var card = new Panel
        {
            BackColor = Color.White,
            Size = new Size(Math.Max(width, content.Width + 12), content.Height + 12),
        };
        content.Location = new Point((card.Width - content.Width) / 2, 6);
        card.Controls.Add(content);

        var host = new ToolStripControlHost(card)
        {
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            AutoSize = false,
            Size = card.Size,
        };

        drop = new ToolStripDropDown
        {
            Padding = Padding.Empty,
            DropShadowEnabled = false,
            BackColor = Color.White,
            Renderer = new PawMenuRenderer(),
        };
        drop.Items.Add(host);

        void Round()
        {
            if (drop.Width > 0 && drop.Height > 0)
                drop.Region = new Region(Ui.RoundRect(new RectangleF(0, 0, drop.Width, drop.Height), 12));
        }
        drop.Opened += (_, _) => { Round(); OnPopupOpened(content); };
        // The content may change height while open (a searchable list shrinks as it is filtered).
        content.SizeChanged += (_, _) =>
        {
            card.Height = content.Height + 12;
            host.Size = card.Size;
            drop.PerformLayout();
        };
        drop.SizeChanged += (_, _) => Round();
        drop.Closed += (_, _) =>
        {
            _open = false;
            _closedAt = Environment.TickCount64;
            if (!IsDisposed) { Invalidate(); Focus(); }
            if (!IsDisposed) BeginInvoke(drop.Dispose);
        };

        _open = true;
        Invalidate();
        drop.Show(this, new Point(0, Height + 4));
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using (var back = new SolidBrush(BackColor.A == 255 ? BackColor : Parent is { BackColor.A: 255 } ? Parent.BackColor : Color.White))
            g.FillRectangle(back, ClientRectangle);

        bool active = Enabled && (_open || Focused);
        var border = active ? Theme.Primary : _hover && Enabled ? Color.FromArgb(197, 210, 228) : Theme.Border;
        using (var path = Ui.RoundRect(new RectangleF(0.5f, 0.5f, Width - 2, Height - 2), 8))
        using (var fill = new SolidBrush(Enabled ? Color.White : Theme.Subtle))
        using (var pen = new Pen(border))
        {
            g.FillPath(fill, path);
            g.DrawPath(pen, path);
        }

        int left = 12;
        if (_glyph.Length > 0)
        {
            TextRenderer.DrawText(g, _glyph, Theme.GlyphFont(10f), new Rectangle(8, 0, 24, Height),
                active ? Theme.Primary : Theme.Muted, CenterFlags);
            left = 36;
        }

        string text = DisplayText;
        bool has = text.Length > 0;
        TextRenderer.DrawText(g, has ? text : _placeholder, Font, new Rectangle(left, 0, Math.Max(0, Width - left - 34), Height),
            has && Enabled ? ForeColor : Theme.Muted, LeftFlags);

        TextRenderer.DrawText(g, _open ? "" : "", Theme.GlyphFont(8f), new Rectangle(Width - 30, 0, 22, Height),
            _open ? Theme.Primary : Theme.Muted, CenterFlags);
    }
}

/// <summary>Collection used by <see cref="PawSelect"/>; same Add/AddRange feel as ComboBox.Items.</summary>
internal sealed class PawItems
{
    private readonly List<string> _list = new();
    private readonly Action _changed;

    public PawItems(Action changed) { _changed = changed; }

    public int Count => _list.Count;
    public string this[int index] => _list[index];

    public void Add(object item) { _list.Add(item.ToString() ?? ""); _changed(); }

    public void AddRange(IEnumerable<object> items)
    {
        foreach (var i in items) _list.Add(i.ToString() ?? "");
        _changed();
    }

    public void Clear() { _list.Clear(); _changed(); }

    public IReadOnlyList<string> All => _list;
}

/// <summary>Drop-down list in the Pawfect style (replaces ComboBox with DropDownList style).</summary>
internal class PawSelect : PawDropField
{
    private int _index = -1;

    public PawSelect()
    {
        Items = new PawItems(() =>
        {
            if (_index >= Items!.Count) _index = -1;
            Invalidate();
        });
    }

    public PawItems Items { get; }

    public event EventHandler? SelectedIndexChanged;

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int SelectedIndex
    {
        get => _index;
        set
        {
            int v = value < 0 || value >= Items.Count ? -1 : value;
            if (v == _index) return;
            _index = v;
            Invalidate();
            SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string? SelectedItem => _index >= 0 ? Items[_index] : null;

    protected override string DisplayText => _index >= 0 ? Items[_index] : "";

    protected override void Step(int direction)
    {
        if (Items.Count == 0) return;
        SelectedIndex = Math.Clamp(_index < 0 ? (direction > 0 ? 0 : Items.Count - 1) : _index + direction, 0, Items.Count - 1);
    }

    protected virtual int VisibleRows => 8;

    [Category("Pawfect"), DefaultValue(false)]
    [Description("Shows a search box in the list: typing narrows the choices. Only an item from the list can be picked, never typed text.")]
    public bool Searchable { get; set; }

    [Category("Pawfect"), DefaultValue("Search\u2026")]
    [Description("Hint inside the search box.")]
    public string SearchPlaceholder { get; set; } = "Search\u2026";

    [Category("Pawfect"), DefaultValue("Nothing to choose from")]
    [Description("Shown in the list when it has no items at all.")]
    public string EmptyText { get; set; } = "Nothing to choose from";

    protected override bool OpensOnTyping => Searchable;

    protected override Control BuildPopup(int width, Action close)
        => new PawSelectPopup(Items.All, _index, width, VisibleRows, Searchable, SearchPlaceholder, EmptyText,
            TakeSeed(), i => SelectedIndex = i, close);

    protected override void OnPopupOpened(Control content)
    {
        if (content is PawSelectPopup popup) popup.FocusInput();
        else base.OnPopupOpened(content);
    }
}

/// <summary>Time of day picked from a list of 15-minute slots.</summary>
internal sealed class PawTimePicker : PawSelect
{
    /// <summary>Service hours set in Service Catalog Management > Set Time Slots (default 8:00 AM up to 8:00 PM).</summary>
    public static TimeSpan Opens { get { ServiceHours.EnsureLoaded(); return ServiceHours.Opens; } }
    public static TimeSpan Closes { get { ServiceHours.EnsureLoaded(); return ServiceHours.Closes; } }

    private readonly List<TimeSpan> _slots = new();

    public PawTimePicker(int stepMinutes = 0)
    {
        ServiceHours.EnsureLoaded();
        if (stepMinutes <= 0) stepMinutes = ServiceHours.StepMinutes;
        // Typing narrows the list (for example "7:4" leaves 7:45 am and 7:45 pm), but only a time from the list can be picked, never typed text.
        Searchable = true;
        SearchPlaceholder = "Type to find a time, e.g. 7:45 pm";
        EmptyText = "No times available";
        Glyph = "";
        for (var t = ServiceHours.Opens; t <= ServiceHours.Closes; t += TimeSpan.FromMinutes(stepMinutes))
            _slots.Add(t);
        Rebuild();
        Value = ServiceHours.Opens + TimeSpan.FromHours(1);
    }

    protected override int VisibleRows => 7;

    private static string Format(TimeSpan t) => (DateTime.Today + t).ToString("h:mm tt", CultureInfo.CurrentCulture);

    private void Rebuild()
    {
        Items.Clear();
        Items.AddRange(_slots.Select(s => (object)Format(s)));
    }

    /// <summary>The chosen time of day. A time outside service hours or off the 15-minute grid snaps to the nearest time in the list.</summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TimeSpan Value
    {
        get => SelectedIndex >= 0 && SelectedIndex < _slots.Count ? _slots[SelectedIndex] : TimeSpan.Zero;
        set
        {
            var v = new TimeSpan(value.Hours, value.Minutes, 0);
            int nearest = 0;
            for (int i = 1; i < _slots.Count; i++)
                if (Math.Abs((_slots[i] - v).Ticks) < Math.Abs((_slots[nearest] - v).Ticks)) nearest = i;
            SelectedIndex = nearest;
        }
    }
}

/// <summary>Date field that opens a month calendar (replaces DateTimePicker).</summary>
internal sealed class PawDatePicker : PawDropField
{
    private DateTime _value = DateTime.Today;
    private DateTime _min = new(1900, 1, 1);
    private DateTime _max = new(2100, 12, 31);
    private bool _checked = true;
    private bool _allowEmpty;
    private HashSet<DayOfWeek>? _allowedDays;

    public PawDatePicker()
    {
        Glyph = "";
        Placeholder = "Select date";
    }

    public event EventHandler? ValueChanged;

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DateTime Value
    {
        get => _value;
        set
        {
            var v = Snap(Clamp(value.Date));
            if (v == _value && _checked) return;
            _value = v;
            _checked = true;
            Invalidate();
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>When AllowEmpty is on, false means "no date chosen".</summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool Checked
    {
        get => _checked || !_allowEmpty;
        set
        {
            if (!_allowEmpty || _checked == value) return;
            _checked = value;
            Invalidate();
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>Show a "Clear" link so the date can be left empty (e.g. an unknown birthday).</summary>
    public bool AllowEmpty
    {
        get => _allowEmpty;
        set { _allowEmpty = value; if (!value) _checked = true; Invalidate(); }
    }

    public DateTime MinDate
    {
        get => _min;
        set { _min = value.Date; _value = Snap(Clamp(_value)); Invalidate(); }
    }

    public DateTime MaxDate
    {
        get => _max;
        set { _max = value.Date; _value = Snap(Clamp(_value)); Invalidate(); }
    }

    /// <summary>
    /// When set, only dates that fall on these weekdays can be picked (the others are greyed out in the calendar).
    /// Null, empty or all seven days = any day.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IReadOnlyCollection<DayOfWeek>? AllowedDays
    {
        get => _allowedDays;
        set
        {
            _allowedDays = value is { Count: > 0 and < 7 } ? new HashSet<DayOfWeek>(value) : null;
            _value = Snap(Clamp(_value));
            Invalidate();
        }
    }

    /// <summary>True when <paramref name="d"/> is inside the date range and on an allowed weekday.</summary>
    public bool IsSelectable(DateTime d)
        => d.Date >= _min && d.Date <= _max && (_allowedDays is null || _allowedDays.Contains(d.DayOfWeek));

    private DateTime Clamp(DateTime d) => d < _min ? _min : d > _max ? _max : d;

    /// <summary>The nearest selectable date on or after <paramref name="d"/> (or before it when nothing later fits).</summary>
    private DateTime Snap(DateTime d)
    {
        if (_allowedDays is null || IsSelectable(d)) return d;
        for (int i = 1; i <= 14; i++)
            if (IsSelectable(d.AddDays(i))) return d.AddDays(i);
        for (int i = 1; i <= 14; i++)
            if (IsSelectable(d.AddDays(-i))) return d.AddDays(-i);
        return d;
    }

    protected override string DisplayText
        => Checked ? _value.ToString("ddd, MMM d, yyyy", CultureInfo.CurrentCulture) : "";

    protected override void Step(int direction)
    {
        for (int i = 1; i <= 400; i++)
        {
            var d = _value.AddDays(direction * i);
            if (d < _min || d > _max) return;
            if (IsSelectable(d)) { Value = d; return; }
        }
    }

    protected override Control BuildPopup(int width, Action close)
    {
        // Drawn by hand (not the system MonthCalendar): that control sized itself before it had a window, so at high DPI the
        // popup cut it off after "Thu"; this one is sized from the field's own width and can grey out days that aren't allowed.
        int cell = Math.Max(38, width / 7);
        var cal = new PawCalendar(cell, Clamp(_value), _min, _max, IsSelectable, highlightSelectable: _allowedDays is not null);
        cal.DateChosen += (_, d) => { Value = d; close(); };
        var panel = new Panel { BackColor = Color.White, Size = new Size(cal.Width, cal.Height + 34) };
        panel.Controls.Add(cal);

        Label Link(string text, int x, Action act)
        {
            var l = Ui.Text(text, 8.5f, Theme.Primary, Color.White, FontStyle.Bold, ContentAlignment.MiddleCenter);
            l.SetBounds(x, cal.Height + 4, 70, 26);
            l.Cursor = Cursors.Hand;
            l.Click += (_, _) => { act(); close(); };
            return l;
        }

        if (IsSelectable(DateTime.Today))
            panel.Controls.Add(Link("Today", 4, () => Value = DateTime.Today));
        if (_allowEmpty)
            panel.Controls.Add(Link("Clear", panel.Width - 74, () => Checked = false));
        return panel;
    }
}

/// <summary>
/// Month grid for <see cref="PawDatePicker"/>, drawn by hand. Days outside the allowed range (or not on an allowed weekday)
/// are greyed out and can't be clicked; when only some weekdays are allowed, those days are tinted so they stand out.
/// </summary>
internal sealed class PawCalendar : Control
{
    private const int HeaderH = 44, WeekH = 28, RowH = 36;
    private const TextFormatFlags Center = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
        TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

    private static readonly Color Disabled = Color.FromArgb(200, 210, 224);

    private readonly int _cell;
    private readonly DateTime _min, _max, _selected;
    private readonly Func<DateTime, bool> _selectable;
    private readonly bool _tint;
    private DateTime _month;
    private DateTime? _hotDay;
    private int _hotNav;            // -1 previous month, +1 next month, 0 none

    public event EventHandler<DateTime>? DateChosen;

    public PawCalendar(int cellWidth, DateTime selected, DateTime min, DateTime max, Func<DateTime, bool> selectable, bool highlightSelectable)
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        Font = Theme.Face(9f);
        Cursor = Cursors.Hand;
        _cell = cellWidth;
        _selected = selected.Date;
        _min = min.Date;
        _max = max.Date;
        _selectable = selectable;
        _tint = highlightSelectable;
        _month = new DateTime(_selected.Year, _selected.Month, 1);
        Size = new Size(cellWidth * 7, HeaderH + WeekH + RowH * 6 + 4);
    }

    private static DateTime MonthOf(DateTime d) => new(d.Year, d.Month, 1);
    private bool CanPrev => _month > MonthOf(_min);
    private bool CanNext => _month < MonthOf(_max);
    private Rectangle PrevRect => new(6, 6, 32, 32);
    private Rectangle NextRect => new(Width - 38, 6, 32, 32);
    private int Offset => (int)_month.DayOfWeek;

    private Rectangle CellRect(int index)
        => new((index % 7) * _cell, HeaderH + WeekH + (index / 7) * RowH, _cell, RowH);

    private DateTime? DayAt(Point p)
    {
        if (p.Y < HeaderH + WeekH || p.X < 0 || p.X >= _cell * 7) return null;
        int col = p.X / _cell, row = (p.Y - HeaderH - WeekH) / RowH;
        int day = row * 7 + col - Offset + 1;
        return row < 6 && day >= 1 && day <= DateTime.DaysInMonth(_month.Year, _month.Month)
            ? new DateTime(_month.Year, _month.Month, day) : null;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        var day = DayAt(e.Location);
        int nav = PrevRect.Contains(e.Location) && CanPrev ? -1 : NextRect.Contains(e.Location) && CanNext ? 1 : 0;
        if (day is { } d && !_selectable(d)) day = null;
        if (day != _hotDay || nav != _hotNav)
        {
            _hotDay = day;
            _hotNav = nav;
            Cursor = day is not null || nav != 0 ? Cursors.Hand : Cursors.Default;
            Invalidate();
        }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        if (_hotDay is not null || _hotNav != 0) { _hotDay = null; _hotNav = 0; Invalidate(); }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (e.Button != MouseButtons.Left) return;
        if (PrevRect.Contains(e.Location)) { if (CanPrev) { _month = _month.AddMonths(-1); Invalidate(); } return; }
        if (NextRect.Contains(e.Location)) { if (CanNext) { _month = _month.AddMonths(1); Invalidate(); } return; }
        if (DayAt(e.Location) is { } d && _selectable(d)) DateChosen?.Invoke(this, d);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var culture = CultureInfo.CurrentCulture;

        TextRenderer.DrawText(g, _month.ToString("MMMM yyyy", culture), Theme.Face(10f, FontStyle.Bold),
            new Rectangle(40, 0, Width - 80, HeaderH), Theme.Text, Center);
        DrawArrow(g, PrevRect, "\uE76B", CanPrev, _hotNav == -1);
        DrawArrow(g, NextRect, "\uE76C", CanNext, _hotNav == 1);

        var names = culture.DateTimeFormat.ShortestDayNames;
        for (int c = 0; c < 7; c++)
            TextRenderer.DrawText(g, names[c], Theme.Face(8.5f, FontStyle.Bold),
                new Rectangle(c * _cell, HeaderH, _cell, WeekH), Theme.Muted, Center);

        int days = DateTime.DaysInMonth(_month.Year, _month.Month);
        for (int day = 1; day <= days; day++)
        {
            var date = new DateTime(_month.Year, _month.Month, day);
            var cell = Rectangle.Inflate(CellRect(Offset + day - 1), -3, -2);
            bool ok = _selectable(date);
            bool chosen = date == _selected;
            bool hot = _hotDay == date;

            Color text = !ok ? Disabled : chosen ? Color.White : _tint ? Theme.Primary : Theme.Text;
            var weight = chosen || (ok && _tint) ? FontStyle.Bold : FontStyle.Regular;

            if (chosen || (ok && (hot || _tint)))
            {
                using var path = Ui.RoundRect(cell, 8);
                using var fill = new SolidBrush(chosen ? Theme.Primary : hot ? Color.FromArgb(214, 229, 249) : Theme.PrimarySoft);
                g.FillPath(fill, path);
            }
            if (date == DateTime.Today && !chosen)
            {
                using var path = Ui.RoundRect(cell, 8);
                using var pen = new Pen(ok ? Theme.Primary : Disabled);
                g.DrawPath(pen, path);
            }
            TextRenderer.DrawText(g, day.ToString(culture), Theme.Face(9f, weight), cell, text, Center);
        }
    }

    private static void DrawArrow(Graphics g, Rectangle r, string glyph, bool enabled, bool hot)
    {
        if (hot)
        {
            using var path = Ui.RoundRect(r, 8);
            using var fill = new SolidBrush(Theme.Subtle);
            g.FillPath(fill, path);
        }
        TextRenderer.DrawText(g, glyph, Theme.GlyphFont(9f), r, enabled ? Theme.Text : Disabled, Center);
    }
}
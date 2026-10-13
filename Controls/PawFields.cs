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
        using (var back = new SolidBrush(Parent?.BackColor ?? Color.White))
            g.FillRectangle(back, ClientRectangle);

        const int box = 18;
        var r = new RectangleF(1.5f, (Height - box) / 2f, box - 1, box - 1);
        using (var path = Ui.RoundRect(r, 5))
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
            TextRenderer.DrawText(g, "", Theme.GlyphFont(8f), Rectangle.Round(r), Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
        }

        TextRenderer.DrawText(g, Text, Font, new Rectangle(box + 10, 0, Width - box - 10, Height), ForeColor,
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
        drop.Opened += (_, _) => { Round(); content.Focus(); };
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
        using (var back = new SolidBrush(Parent?.BackColor ?? Color.White))
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

    protected override Control BuildPopup(int width, Action close)
    {
        const int rowH = 30;
        int rows = Math.Clamp(Items.Count, 1, VisibleRows);
        var list = new ListBox
        {
            BorderStyle = BorderStyle.None,
            DrawMode = DrawMode.OwnerDrawFixed,
            ItemHeight = rowH,
            IntegralHeight = false,
            BackColor = Color.White,
            Font = Font,
            Size = new Size(width, rows * rowH),
            Cursor = Cursors.Hand,
        };
        list.Items.AddRange(Items.All.Cast<object>().ToArray());
        if (_index >= 0) list.SelectedIndex = _index;

        int hot = -1;
        list.HandleCreated += (_, _) =>
        {
            if (_index >= 0) list.TopIndex = Math.Max(0, _index - rows / 2);
        };
        list.MouseMove += (_, e) =>
        {
            int i = list.IndexFromPoint(e.Location);
            if (i != hot) { hot = i; list.Invalidate(); }
        };
        list.MouseLeave += (_, _) => { hot = -1; list.Invalidate(); };
        list.MouseUp += (_, e) =>
        {
            if (e.Button != MouseButtons.Left) return;
            int i = list.IndexFromPoint(e.Location);
            if (i < 0) return;
            SelectedIndex = i;
            close();
        };
        list.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter && list.SelectedIndex >= 0) { SelectedIndex = list.SelectedIndex; close(); e.Handled = true; }
            else if (e.KeyCode == Keys.Escape) { close(); e.Handled = true; }
        };
        list.DrawItem += (_, e) =>
        {
            if (e.Index < 0 || e.Index >= list.Items.Count) return;
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var bg = new SolidBrush(Color.White))
                g.FillRectangle(bg, e.Bounds);

            bool chosen = e.Index == _index;
            bool hover = e.Index == hot || (hot < 0 && (e.State & DrawItemState.Selected) != 0 && !chosen);
            var r = Rectangle.Inflate(e.Bounds, -2, -2);
            if (chosen || hover)
            {
                using var path = Ui.RoundRect(r, 6);
                using var b = new SolidBrush(chosen ? Theme.PrimarySoft : Theme.Subtle);
                g.FillPath(b, path);
            }

            TextRenderer.DrawText(g, list.Items[e.Index]?.ToString() ?? "", Font,
                new Rectangle(r.X + 8, r.Y, r.Width - 36, r.Height), chosen ? Theme.Primary : Theme.Text,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
                TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
            if (chosen)
                TextRenderer.DrawText(g, "", Theme.GlyphFont(8f), new Rectangle(r.Right - 26, r.Y, 22, r.Height), Theme.Primary,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding |
                    TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
        };
        return list;
    }
}

/// <summary>Time of day picked from a list of 15-minute slots.</summary>
internal sealed class PawTimePicker : PawSelect
{
    private readonly List<TimeSpan> _slots = new();

    public PawTimePicker(int fromHour = 6, int toHour = 23, int stepMinutes = 15)
    {
        Glyph = "";
        for (var t = TimeSpan.FromHours(fromHour); t <= new TimeSpan(toHour, 60 - stepMinutes, 0); t += TimeSpan.FromMinutes(stepMinutes))
            _slots.Add(t);
        Rebuild();
        Value = TimeSpan.FromHours(9);
    }

    protected override int VisibleRows => 7;

    private static string Format(TimeSpan t) => (DateTime.Today + t).ToString("h:mm tt", CultureInfo.CurrentCulture);

    private void Rebuild()
    {
        Items.Clear();
        Items.AddRange(_slots.Select(s => (object)Format(s)));
    }

    /// <summary>The chosen time of day. A time that isn't on the 15-minute grid is added to the list.</summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TimeSpan Value
    {
        get => SelectedIndex >= 0 && SelectedIndex < _slots.Count ? _slots[SelectedIndex] : TimeSpan.Zero;
        set
        {
            var v = new TimeSpan(value.Hours, value.Minutes, 0);
            int i = _slots.IndexOf(v);
            if (i < 0)
            {
                _slots.Add(v);
                _slots.Sort();
                Rebuild();
                i = _slots.IndexOf(v);
            }
            SelectedIndex = i;
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
            var v = Clamp(value.Date);
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
        set { _min = value.Date; _value = Clamp(_value); Invalidate(); }
    }

    public DateTime MaxDate
    {
        get => _max;
        set { _max = value.Date; _value = Clamp(_value); Invalidate(); }
    }

    private DateTime Clamp(DateTime d) => d < _min ? _min : d > _max ? _max : d;

    protected override string DisplayText
        => Checked ? _value.ToString("ddd, MMM d, yyyy", CultureInfo.CurrentCulture) : "";

    protected override void Step(int direction)
    {
        Value = _value.AddDays(direction);
    }

    protected override Control BuildPopup(int width, Action close)
    {
        var cal = new MonthCalendar
        {
            MaxSelectionCount = 1,
            ShowToday = false,
            ShowTodayCircle = true,
            MinDate = _min,
            MaxDate = _max,
            BackColor = Color.White,
            ForeColor = Theme.Text,
            TitleBackColor = Theme.Primary,
            TitleForeColor = Color.White,
            TrailingForeColor = Theme.Muted,
            Font = Theme.Face(9f),
            Location = new Point(0, 0),
        };
        cal.SetDate(Clamp(_value));
        cal.DateSelected += (_, e) => { Value = e.Start; close(); };

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

        if (DateTime.Today >= _min && DateTime.Today <= _max)
            panel.Controls.Add(Link("Today", 4, () => Value = DateTime.Today));
        if (_allowEmpty)
            panel.Controls.Add(Link("Clear", panel.Width - 74, () => Checked = false));
        return panel;
    }
}

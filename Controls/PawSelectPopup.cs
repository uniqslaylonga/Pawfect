using System.Drawing.Drawing2D;

namespace MyApp.Controls;

// Popup parts for PawSelect: a hand-drawn list (thin rounded scroll bar instead of the grey system one),
// a search box, and the panel that puts them together and filters the list as the user types.
// The user can only ever pick an item that is in the list; whatever they type just narrows it down.

/// <summary>Scrollable list drawn in the Pawfect style. Rows are plain text; one can be the current choice, one is highlighted.</summary>
internal sealed class PawListView : Control
{
    public const int RowH = 32;
    private const int BarZone = 14;     // width reserved at the right for the scroll bar when the list scrolls

    private const TextFormatFlags LeftFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
        TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;
    private const TextFormatFlags CenterFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
        TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

    private IReadOnlyList<string> _items = Array.Empty<string>();
    private int _chosen = -1;           // row that is the current selection
    private int _hot = -1;              // row under the mouse / reached with the arrow keys
    private int _scroll;                // pixels scrolled from the top
    private bool _dragBar;
    private int _dragOffset;
    private bool _hoverBar;

    public event EventHandler<int>? RowChosen;

    public PawListView()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        Font = Theme.Face(9f);
        Cursor = Cursors.Hand;
    }

    /// <summary>Text shown in the middle when there are no rows.</summary>
    public string EmptyText { get; set; } = "Nothing to choose from";

    /// <summary>False when a search box owns the keyboard (clicking the list then doesn't steal the focus).</summary>
    public bool Focusable
    {
        set { SetStyle(ControlStyles.Selectable, value); TabStop = value; }
    }

    public int RowCount => _items.Count;
    public int HotRow => _hot;

    private int ContentHeight => _items.Count * RowH;
    private int MaxScroll => Math.Max(0, ContentHeight - Height);
    private bool HasBar => ContentHeight > Height;

    /// <summary>Replaces the rows. <paramref name="hot"/> (if any) is highlighted and scrolled to the middle.</summary>
    public void SetRows(IReadOnlyList<string> items, int chosen, int hot)
    {
        _items = items;
        _chosen = chosen;
        _hot = hot >= 0 && hot < items.Count ? hot : -1;
        _scroll = _hot >= 0 ? Math.Max(0, _hot * RowH - (Height - RowH) / 2) : 0;
        _scroll = Math.Clamp(_scroll, 0, MaxScroll);
        Invalidate();
    }

    public void MoveHot(int delta)
    {
        if (_items.Count == 0) return;
        _hot = _hot < 0
            ? (delta > 0 ? 0 : _items.Count - 1)
            : Math.Clamp(_hot + delta, 0, _items.Count - 1);
        EnsureVisible(_hot);
    }

    public void JumpHot(bool toEnd)
    {
        if (_items.Count == 0) return;
        _hot = toEnd ? _items.Count - 1 : 0;
        EnsureVisible(_hot);
    }

    public void ChooseHot()
    {
        if (_hot >= 0 && _hot < _items.Count) RowChosen?.Invoke(this, _hot);
    }

    public void WheelScroll(int delta)
    {
        if (!HasBar) return;
        _scroll = Math.Clamp(_scroll - Math.Sign(delta) * RowH * 2, 0, MaxScroll);
        var p = PointToClient(Cursor.Position);
        if (ClientRectangle.Contains(p)) _hot = RowAt(p);
        Invalidate();
    }

    private void EnsureVisible(int row)
    {
        int top = row * RowH, bottom = top + RowH;
        if (top < _scroll) _scroll = top;
        else if (bottom > _scroll + Height) _scroll = bottom - Height;
        _scroll = Math.Clamp(_scroll, 0, MaxScroll);
        Invalidate();
    }

    private int RowAt(Point p)
    {
        if (p.Y < 0 || p.Y >= Height || (HasBar && p.X >= Width - BarZone)) return -1;
        int i = (p.Y + _scroll) / RowH;
        return i >= 0 && i < _items.Count ? i : -1;
    }

    private Rectangle Thumb()
    {
        if (!HasBar) return Rectangle.Empty;
        int track = Height - 8;
        int h = Math.Max(28, (int)((long)track * Height / ContentHeight));
        int y = 4 + (MaxScroll == 0 ? 0 : (int)((long)(track - h) * _scroll / MaxScroll));
        return new Rectangle(Width - 10, y, 6, h);
    }

    private void DragTo(int mouseY)
    {
        var t = Thumb();
        int range = (Height - 8) - t.Height;
        if (range <= 0) return;
        int y = Math.Clamp(mouseY - _dragOffset - 4, 0, range);
        _scroll = (int)((long)MaxScroll * y / range);
        Invalidate();
    }

    protected override bool IsInputKey(Keys keyData)
    {
        switch (keyData & Keys.KeyCode)
        {
            case Keys.Up:
            case Keys.Down:
            case Keys.PageUp:
            case Keys.PageDown:
            case Keys.Home:
            case Keys.End:
            case Keys.Enter:
                return true;
        }
        return base.IsInputKey(keyData);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        _scroll = Math.Clamp(_scroll, 0, MaxScroll);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (_dragBar) { DragTo(e.Y); return; }

        bool bar = HasBar && e.X >= Width - BarZone;
        int row = bar ? -1 : RowAt(e.Location);
        if (row != _hot || bar != _hoverBar)
        {
            _hot = row;
            _hoverBar = bar;
            Cursor = bar ? Cursors.Default : Cursors.Hand;
            Invalidate();
        }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        if (_dragBar) return;
        if (_hot != -1 || _hoverBar) { _hot = -1; _hoverBar = false; Invalidate(); }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button != MouseButtons.Left || !HasBar || e.X < Width - BarZone) return;
        var t = Thumb();
        _dragOffset = t.Contains(e.Location) ? e.Y - t.Y : t.Height / 2;
        _dragBar = true;
        DragTo(e.Y);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (_dragBar) { _dragBar = false; Invalidate(); return; }
        if (e.Button != MouseButtons.Left) return;
        int row = RowAt(e.Location);
        if (row >= 0) RowChosen?.Invoke(this, row);
    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
        base.OnMouseWheel(e);
        WheelScroll(e.Delta);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Color.White);

        if (_items.Count == 0)
        {
            TextRenderer.DrawText(g, EmptyText, Font, new Rectangle(0, 0, Width, Height), Theme.Muted, CenterFlags);
            return;
        }

        int right = HasBar ? BarZone : 0;
        int first = Math.Max(0, _scroll / RowH);
        int last = Math.Min(_items.Count - 1, (_scroll + Height) / RowH);
        for (int i = first; i <= last; i++)
        {
            var r = Rectangle.Inflate(new Rectangle(0, i * RowH - _scroll, Width - right, RowH), -2, -2);
            bool chosen = i == _chosen;
            bool hot = i == _hot;
            if (chosen || hot)
            {
                using var path = Ui.RoundRect(r, 6);
                using var b = new SolidBrush(chosen ? Theme.PrimarySoft : Theme.Subtle);
                g.FillPath(b, path);
            }

            TextRenderer.DrawText(g, _items[i], Font,
                new Rectangle(r.X + 10, r.Y, r.Width - (chosen ? 38 : 20), r.Height),
                chosen ? Theme.Primary : Theme.Text, LeftFlags);
            if (chosen)
                TextRenderer.DrawText(g, "\uE73E", Theme.GlyphFont(8f), new Rectangle(r.Right - 28, r.Y, 22, r.Height),
                    Theme.Primary, CenterFlags);
        }

        if (HasBar)
        {
            using var path = Ui.RoundRect(Thumb(), 3);
            using var b = new SolidBrush(_dragBar || _hoverBar ? Color.FromArgb(150, 165, 186) : Color.FromArgb(200, 210, 224));
            g.FillPath(b, path);
        }
    }
}

/// <summary>Text box that hands the list-navigation keys (arrows, Enter, Esc) to its owner before the pop-up window swallows them.</summary>
internal sealed class NavTextBox : TextBox
{
    public Func<Keys, bool>? Nav { get; set; }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (Nav is not null &&
            (keyData is Keys.Up or Keys.Down or Keys.PageUp or Keys.PageDown or Keys.Enter or Keys.Escape) &&
            Nav(keyData))
            return true;
        return base.ProcessCmdKey(ref msg, keyData);
    }
}

/// <summary>Rounded search field (magnifier on the left, clear button on the right) shown at the top of a searchable drop-down.</summary>
internal sealed class PawSearchBox : Control
{
    private const TextFormatFlags CenterFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
        TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

    private readonly NavTextBox _text = new();
    private bool _hoverClear;

    public PawSearchBox(string hint)
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        Font = Theme.Face(9f);
        Size = new Size(200, 36);

        _text.BorderStyle = BorderStyle.None;
        _text.BackColor = Color.White;
        _text.ForeColor = Theme.Text;
        _text.Font = Theme.Face(9f);
        _text.PlaceholderText = hint;
        _text.MaxLength = 60;
        _text.TextChanged += (_, _) => Invalidate();
        Controls.Add(_text);
        LayoutText();
    }

    public NavTextBox Inner => _text;

    /// <summary>What the user typed (may be empty).</summary>
    public string Query => _text.Text;

    private Rectangle ClearRect => new(Width - 32, 0, 28, Height);

    private void LayoutText()
        => _text.SetBounds(34, Math.Max(0, (Height - _text.Height) / 2), Math.Max(20, Width - 34 - 34), _text.Height);

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        LayoutText();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        bool over = _text.TextLength > 0 && ClearRect.Contains(e.Location);
        if (over != _hoverClear)
        {
            _hoverClear = over;
            Cursor = over ? Cursors.Hand : Cursors.Default;
            Invalidate();
        }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        if (_hoverClear) { _hoverClear = false; Cursor = Cursors.Default; Invalidate(); }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (e.Button == MouseButtons.Left && _text.TextLength > 0 && ClearRect.Contains(e.Location))
        {
            _text.Clear();
            _text.Focus();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Color.White);

        using (var path = Ui.RoundRect(new RectangleF(0.5f, 0.5f, Width - 2, Height - 2), 8))
        using (var fill = new SolidBrush(Color.White))
        using (var pen = new Pen(Theme.Primary))
        {
            g.FillPath(fill, path);
            g.DrawPath(pen, path);
        }

        TextRenderer.DrawText(g, "\uE721", Theme.GlyphFont(9f), new Rectangle(8, 0, 24, Height), Theme.Primary, CenterFlags);
        if (_text.TextLength > 0)
            TextRenderer.DrawText(g, "\uE711", Theme.GlyphFont(8f), ClearRect, _hoverClear ? Theme.Text : Theme.Muted, CenterFlags);
    }
}

/// <summary>
/// What <see cref="PawSelect"/> shows when opened: an optional search box on top of the list. Typing keeps only the items that
/// contain every typed word (any order, ignoring case); Enter picks the highlighted one. Nothing typed is ever kept as a value.
/// </summary>
internal sealed class PawSelectPopup : Panel
{
    private const int SearchH = 36, Gap = 6;

    private readonly List<string> _all;
    private readonly int _chosenAll;
    private readonly int _maxRows;
    private readonly Action<int> _pick;
    private readonly Action _close;
    private readonly PawListView _list = new();
    private readonly PawSearchBox? _search;
    private List<int> _map = new();      // row in the filtered list -> index in the full list

    public PawSelectPopup(IEnumerable<string> all, int chosen, int width, int maxRows, bool searchable,
        string searchHint, string emptyText, string seed, Action<int> pick, Action close)
    {
        _all = all.ToList();
        _chosenAll = chosen;
        _maxRows = Math.Max(1, maxRows);
        _pick = pick;
        _close = close;

        BackColor = Color.White;
        Width = width;

        int top = 0;
        if (searchable)
        {
            _search = new PawSearchBox(searchHint) { Bounds = new Rectangle(0, 0, width, SearchH) };
            _search.Inner.Nav = Nav;
            _search.Inner.MouseWheel += (_, e) => _list.WheelScroll(e.Delta);
            Controls.Add(_search);
            top = SearchH + Gap;
        }

        _list.Focusable = !searchable;
        _list.EmptyText = _all.Count == 0 ? emptyText : "No matches";
        _list.Location = new Point(0, top);
        _list.Width = width;
        _list.RowChosen += (_, row) => Pick(row);
        _list.KeyDown += (_, e) => { if (Nav(e.KeyCode)) e.Handled = true; };
        Controls.Add(_list);

        if (_search is not null && seed.Length > 0)
            _search.Inner.Text = seed;
        Refilter();
        if (_search is not null)
            _search.Inner.TextChanged += (_, _) => Refilter();
    }

    /// <summary>Puts the keyboard cursor where the user can type (search box) or navigate (plain list).</summary>
    public void FocusInput()
    {
        if (_search is null) { _list.Focus(); return; }
        _search.Inner.Focus();
        _search.Inner.SelectionStart = _search.Inner.TextLength;
    }

    private void Refilter()
    {
        var words = (_search?.Query ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries);
        _map = new List<int>();
        for (int i = 0; i < _all.Count; i++)
        {
            string text = _all[i];
            if (words.All(w => text.Contains(w, StringComparison.CurrentCultureIgnoreCase)))
                _map.Add(i);
        }

        var rows = _map.Select(i => _all[i]).ToList();
        int chosenRow = _map.IndexOf(_chosenAll);
        // While searching, the top match is highlighted so Enter takes it; otherwise start on the current choice.
        int hot = words.Length > 0 ? (rows.Count > 0 ? 0 : -1) : chosenRow;

        _list.Height = Math.Clamp(rows.Count, 1, _maxRows) * PawListView.RowH;
        _list.SetRows(rows, chosenRow, hot);
        Height = _list.Bottom;
    }

    private void Pick(int row)
    {
        if (row < 0 || row >= _map.Count) return;
        _pick(_map[row]);
        _close();
    }

    private bool Nav(Keys key)
    {
        switch (key)
        {
            case Keys.Down: _list.MoveHot(1); return true;
            case Keys.Up: _list.MoveHot(-1); return true;
            case Keys.PageDown: _list.MoveHot(_maxRows - 1); return true;
            case Keys.PageUp: _list.MoveHot(-(_maxRows - 1)); return true;
            case Keys.Home: _list.JumpHot(false); return true;
            case Keys.End: _list.JumpHot(true); return true;
            case Keys.Enter: Pick(_list.HotRow); return true;
            case Keys.Escape: _close(); return true;
        }
        return false;
    }
}

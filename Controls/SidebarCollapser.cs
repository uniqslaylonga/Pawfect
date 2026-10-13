namespace MyApp.Controls;

/// <summary>
/// Makes a sidebar collapse by itself. It rests as a narrow icon-only strip; when the mouse moves over it, it slides open
/// on top of the page (the page does not move), and when the mouse leaves it slides shut again. Hovering an icon in the
/// strip shows its name. Works for both the employee and the admin sidebar: create one right after InitializeComponent().
/// </summary>
internal sealed class SidebarCollapser
{
    private const int CollapsedWidthAt96Dpi = 72;
    private const int TickMs = 15;
    private const int CloseDelayTicks = 16;   // about a quarter of a second outside before it closes

    private readonly EdgePanel _sidebar;
    private readonly FlowLayoutPanel _nav;
    private readonly Control[] _brandTexts;
    private readonly List<NavItem> _flowItems = new();      // items in the menu list (their width follows the sidebar)
    private readonly List<NavItem> _allItems = new();       // plus items elsewhere, e.g. Log Out
    private readonly Dictionary<NavItem, int> _heights = new();
    private readonly ToolTip _tip = new();
    private readonly System.Windows.Forms.Timer _timer = new() { Interval = TickMs };

    private bool _ready;
    private int _expandedWidth;
    private int _collapsedWidth;
    private int _iconsOnlyBelow;   // while sliding, labels are hidden once the sidebar is narrower than this
    private int _target;
    private int _outsideTicks;
    private bool _iconsOnly;

    public SidebarCollapser(EdgePanel sidebar, FlowLayoutPanel navPanel, NavItem logoutNav, params Control[] brandTexts)
    {
        _sidebar = sidebar;
        _nav = navPanel;
        _brandTexts = brandTexts;

        _flowItems.AddRange(navPanel.Controls.OfType<NavItem>());
        _allItems.AddRange(_flowItems);
        _allItems.Add(logoutNav);

        _timer.Tick += (_, _) => Tick();
        sidebar.Disposed += (_, _) =>
        {
            _timer.Dispose();
            _tip.Dispose();
        };

        // The form scales its controls to the screen's DPI after the constructor, so wait until it is on screen.
        var form = sidebar.FindForm();
        if (form is not null)
            form.Shown += (_, _) => Setup();
        else
            sidebar.HandleCreated += (_, _) => sidebar.BeginInvoke(Setup);
    }

    private void Setup()
    {
        if (_ready) return;
        var parent = _sidebar.Parent;
        if (parent is null) return;
        _ready = true;

        _expandedWidth = _sidebar.Width;
        _collapsedWidth = (int)Math.Round(CollapsedWidthAt96Dpi * _sidebar.DeviceDpi / 96.0);
        _iconsOnlyBelow = (_expandedWidth + _collapsedWidth) / 2;
        foreach (var item in _allItems)
            _heights[item] = item.Height;

        // A thin spacer keeps the page where it is, so the sidebar can open over it instead of pushing it.
        var spacer = new Panel { Dock = DockStyle.Left, Width = _collapsedWidth, BackColor = Theme.Bg };

        parent.SuspendLayout();
        int index = parent.Controls.GetChildIndex(_sidebar);
        parent.Controls.Add(spacer);
        parent.Controls.SetChildIndex(spacer, index);

        _sidebar.Dock = DockStyle.None;
        _sidebar.Location = Point.Empty;
        _sidebar.Height = parent.ClientSize.Height;
        _sidebar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        _sidebar.BringToFront();
        parent.ResumeLayout(true);

        _target = _collapsedWidth;
        SetWidth(_collapsedWidth);
        _timer.Start();
    }

    private bool CursorInside()
    {
        var form = _sidebar.FindForm();
        if (form is null || !form.Visible || form.WindowState == FormWindowState.Minimized) return false;
        if (Form.ActiveForm != form) return false;   // another window or a dialog has focus
        return _sidebar.RectangleToScreen(_sidebar.ClientRectangle).Contains(Cursor.Position);
    }

    private void Tick()
    {
        if (CursorInside())
        {
            _outsideTicks = 0;
            _target = _expandedWidth;
        }
        else if (_outsideTicks < CloseDelayTicks && ++_outsideTicks >= CloseDelayTicks)
        {
            _target = _collapsedWidth;
        }

        int diff = _target - _sidebar.Width;
        if (diff == 0) return;

        int step = Math.Max(12, Math.Abs(diff) / 3);
        int next = Math.Abs(diff) <= step ? _target : _sidebar.Width + Math.Sign(diff) * step;
        SetWidth(next);
    }

    private void SetWidth(int width)
    {
        bool iconsOnly = width < _iconsOnlyBelow;

        _nav.SuspendLayout();
        _sidebar.Width = width;

        int itemWidth = Math.Max(10, width - _sidebar.Padding.Horizontal - _nav.Padding.Horizontal);
        foreach (var item in _flowItems)
            item.Width = itemWidth;

        if (iconsOnly != _iconsOnly)
        {
            _iconsOnly = iconsOnly;
            ApplyMode();
        }
        _nav.ResumeLayout(true);
    }

    private void ApplyMode()
    {
        int compactHeight = _heights.Values.Min();

        foreach (var item in _allItems)
        {
            item.Compact = _iconsOnly;
            if (_flowItems.Contains(item))
                item.Height = _iconsOnly ? compactHeight : _heights[item];   // long names wrap onto two lines only when there is room
            _tip.SetToolTip(item, _iconsOnly ? item.Text : string.Empty);
        }

        foreach (var text in _brandTexts)
            text.Visible = !_iconsOnly;
    }
}
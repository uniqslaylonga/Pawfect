using System.Drawing.Drawing2D;
using System.Globalization;
using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Room Management: its own page next to Dashboard, Appointments, Billing and Lookup. Staff can see every room and whether it is
/// vacant, in use (and by which groomer) or being sanitized, open a room's schedule, add a room, rename it, switch it off or delete it.
/// A room is "in use" from the start of a booking until the service ends, then "sanitizing" for
/// <see cref="RoomScheduleRepository.SanitizeMinutes"/> minutes (the same 30-minute gap the appointment form enforces).
/// Built from the same pieces as the other pages: Theme colours, CardPanel, PrimaryButton, IconBadge and DialogBase.
/// </summary>
internal sealed class RoomManagementPage : UserControl
{
    private readonly RoomPanel _rooms = new();

    // The status depends on the clock, so the list refreshes itself while the page is open.
    private readonly System.Windows.Forms.Timer _tick = new() { Interval = 30_000 };

    /// <summary>A room was added, renamed, switched on/off or deleted. The text is for the audit log.</summary>
    public event EventHandler<(string Action, string Detail)>? Changed;

    public RoomManagementPage()
    {
        DoubleBuffered = true;
        BackColor = Theme.Bg;
        Padding = new Padding(RoomsPx.S(this, 20));

        _rooms.Dock = DockStyle.Fill;
        Controls.Add(_rooms);

        _rooms.AddClicked += (_, _) => Add();
        _rooms.ScheduleClicked += (_, row) => ShowSchedule(row);
        _rooms.EditClicked += (_, row) => Edit(row);
        _rooms.ToggleClicked += (_, row) => Toggle(row);
        _rooms.DeleteClicked += (_, row) => Delete(row);

        _tick.Tick += (_, _) => Reload(quiet: true);
        VisibleChanged += (_, _) => _tick.Enabled = Visible;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _tick.Dispose();
        base.Dispose(disposing);
    }

    /// <summary>Reloads the list from the database (called every time the page is opened, and every 30 seconds after that).</summary>
    public void Reload(bool quiet = false)
    {
        try
        {
            _rooms.SetData(RoomScheduleRepository.GetRooms(DateTime.Now));
        }
        catch (SqlException ex)
        {
            if (quiet) return;
            Msg.Show(FindForm(), "Couldn't load the rooms.\n\nMake sure Sql/appointment_scheduling.sql has been run.\n\n" + ex.Message,
                "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ShowSchedule(RoomRow row)
    {
        using var dlg = new RoomScheduleDialog(row);
        dlg.ShowDialog(FindForm());
    }

    private void Add()
    {
        using var dlg = new RoomEditDialog();
        if (dlg.ShowDialog(FindForm()) != DialogResult.OK) return;
        Reload();
        Changed?.Invoke(this, ("Room Added", $"Added room {dlg.SavedName}"));
    }

    private void Edit(RoomRow row)
    {
        using var dlg = new RoomEditDialog(row);
        if (dlg.ShowDialog(FindForm()) != DialogResult.OK) return;
        Reload();
        Changed?.Invoke(this, ("Room Updated", $"Room \"{row.Name}\" is now \"{dlg.SavedName}\""));
    }

    private void Toggle(RoomRow row)
    {
        bool activate = !row.Active;

        if (!activate)
        {
            string extra = row.Upcoming > 0
                ? $"{row.Upcoming} upcoming appointment{(row.Upcoming == 1 ? "" : "s")} already use this room. They keep it, but it won't be offered for new bookings."
                : "It won't be offered when scheduling new appointments.";
            if (Msg.Show(FindForm(), $"Deactivate {row.Name}?\n\n{extra}", "Pawfect", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
        }

        try { RoomScheduleRepository.SetActive(row.Id, activate); }
        catch (SqlException ex)
        {
            Msg.Show(FindForm(), "Couldn't save.\n\n" + ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        Reload();
        Changed?.Invoke(this, ($"Room {(activate ? "Activated" : "Deactivated")}", $"Room \"{row.Name}\""));
    }

    private void Delete(RoomRow row)
    {
        // A room that still has bookings can't be deleted: they would be left without a room.
        if (row.Upcoming > 0 || row.State == RoomState.InUse)
        {
            Msg.Show(FindForm(),
                $"Can't delete {row.Name}.\n\nIt still has upcoming bookings or is being used right now. " +
                "Move those appointments to another room (or cancel them) first, then try again.",
                "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (Msg.Show(FindForm(),
                $"Delete {row.Name}?\n\nThe room is removed for good. Past appointments stay in the records but will no longer show a room. " +
                "If you only want to stop using it for new bookings, choose Deactivate instead.",
                "Pawfect", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        int blocking;
        try { blocking = RoomScheduleRepository.TryDelete(row.Id); }
        catch (SqlException ex)
        {
            Msg.Show(FindForm(), "Couldn't delete the room.\n\n" + ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        Reload();
        if (blocking > 0)
        {
            // Someone booked it between loading the list and clicking Delete.
            Msg.Show(FindForm(), $"Can't delete {row.Name}.\n\nA booking was just made for this room. Move it to another room first, then try again.",
                "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        Changed?.Invoke(this, ("Room Deleted", $"Deleted room \"{row.Name}\""));
    }
}

/// <summary>Scales the page's pixel sizes to the screen (this page is built in code, so WinForms doesn't do it).</summary>
internal static class RoomsPx
{
    public static int S(Control c, int v) => (int)Math.Round(v * Math.Max(1f, c.DeviceDpi / 96f));
}

/// <summary>Colours for the "sanitizing" state (Theme has no amber pair).</summary>
internal static class RoomColors
{
    public static readonly Color AmberText = Color.FromArgb(0xC7, 0x7A, 0x0A);
    public static readonly Color AmberSoft = Color.FromArgb(0xFE, 0xF3, 0xE0);
}

// ====================================================================== card with the list of rooms

/// <summary>One white card: title, status summary, "+ Add Room" button and the rows.</summary>
internal sealed class RoomPanel : CardPanel
{
    private readonly Label _title;
    private readonly Label _count;
    private readonly PrimaryButton _add;
    private readonly Panel _header = new() { Dock = DockStyle.Top, BackColor = Color.White };
    private readonly FlowLayoutPanel _list = new()
    {
        Dock = DockStyle.Fill,
        FlowDirection = FlowDirection.TopDown,
        WrapContents = false,
        AutoScroll = true,
        BackColor = Color.White,
        Margin = Padding.Empty,
        Padding = Padding.Empty,
    };
    private readonly Label _empty;
    private List<RoomRow>? _last;

    public event EventHandler? AddClicked;
    public event EventHandler<RoomRow>? ScheduleClicked;
    public event EventHandler<RoomRow>? EditClicked;
    public event EventHandler<RoomRow>? ToggleClicked;
    public event EventHandler<RoomRow>? DeleteClicked;

    public RoomPanel()
    {
        FillColor = Color.White;
        BorderColor = Theme.Border;
        CornerColor = Theme.Bg;
        Radius = 10;
        BackColor = Color.White;
        Padding = new Padding(1, 8, 1, 8);   // keeps the rows inside the rounded corners

        int s(int v) => RoomsPx.S(this, v);

        _title = Ui.Text("Rooms", 12f, Theme.Text, Color.White, FontStyle.Bold);
        _count = Ui.Text("", 8.5f, Theme.Muted, Color.White);
        _add = new PrimaryButton("+ Add Room")
        {
            BackColor = Color.White,
            Font = Theme.Face(9f, FontStyle.Bold),
            Size = new Size(s(130), s(34)),
        };
        _add.Click += (_, _) => AddClicked?.Invoke(this, EventArgs.Empty);

        _header.Height = s(64);
        _header.Controls.Add(_title);
        _header.Controls.Add(_count);
        _header.Controls.Add(_add);
        _header.Resize += (_, _) => LayoutHeader();
        _header.Paint += (_, e) =>
        {
            using var pen = new Pen(Theme.Border);
            e.Graphics.DrawLine(pen, 0, _header.Height - 1, _header.Width, _header.Height - 1);
        };

        _empty = Ui.Text("No rooms yet\nUse “+ Add Room” to create the first one.", 9f, Theme.Muted, Color.White,
            FontStyle.Regular, ContentAlignment.MiddleCenter);
        _empty.Dock = DockStyle.Fill;
        _empty.Visible = false;

        FlowLists.NoHorizontalScroll(_list);
        _list.Resize += (_, _) => FitRows();

        Controls.Add(_list);     // fills what the header leaves
        Controls.Add(_empty);
        Controls.Add(_header);
        LayoutHeader();
    }

    private void LayoutHeader()
    {
        int s(int v) => RoomsPx.S(this, v);
        _title.SetBounds(s(20), s(10), Math.Max(10, _header.Width - _add.Width - s(48)), s(26));
        _count.SetBounds(s(20), s(36), Math.Max(10, _header.Width - _add.Width - s(48)), s(18));
        _add.Location = new Point(_header.Width - _add.Width - s(20), (_header.Height - _add.Height) / 2);
    }

    public void SetData(List<RoomRow> rows)
    {
        // The page refreshes on a timer; don't rebuild (and lose the scroll position) when nothing changed.
        if (_last is not null && _last.SequenceEqual(rows)) return;
        _last = rows;

        _count.Text = Summary(rows);

        _list.SuspendLayout();
        foreach (var old in _list.Controls.Cast<Control>().ToList()) { _list.Controls.Remove(old); old.Dispose(); }
        foreach (var row in rows)
        {
            var view = new RoomRowView(row) { Margin = Padding.Empty };
            view.ScheduleClicked += (_, _) => ScheduleClicked?.Invoke(this, row);
            view.EditClicked += (_, _) => EditClicked?.Invoke(this, row);
            view.ToggleClicked += (_, _) => ToggleClicked?.Invoke(this, row);
            view.DeleteClicked += (_, _) => DeleteClicked?.Invoke(this, row);
            _list.Controls.Add(view);
        }
        FitRows();
        _list.ResumeLayout(true);

        _list.Visible = rows.Count > 0;
        _empty.Visible = rows.Count == 0;
    }

    private static string Summary(List<RoomRow> rows)
    {
        if (rows.Count == 0) return "Nothing added yet";
        var active = rows.Where(r => r.Active).ToList();
        var parts = new List<string>();
        int vacant = active.Count(r => r.State == RoomState.Vacant);
        int inUse = active.Count(r => r.State == RoomState.InUse);
        int cleaning = active.Count(r => r.State == RoomState.Sanitizing);
        int inactive = rows.Count - active.Count;
        if (vacant > 0) parts.Add($"{vacant} vacant");
        if (inUse > 0) parts.Add($"{inUse} in use");
        if (cleaning > 0) parts.Add($"{cleaning} sanitizing");
        if (inactive > 0) parts.Add($"{inactive} inactive");
        return string.Join("  ·  ", parts);
    }

    private void FitRows()
    {
        int w = Math.Max(10, _list.ClientSize.Width);
        foreach (Control c in _list.Controls)
            if (c.Width != w) c.Width = w;
    }
}

// ====================================================================== one row

/// <summary>A room: round icon, name, who is using it, status pill, and Schedule / Edit / Deactivate / Delete buttons.</summary>
internal sealed class RoomRowView : Control
{
    private const TextFormatFlags LeftText = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
        TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;
    private const TextFormatFlags Center = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
        TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

    private readonly RoomRow _row;
    private bool _hover;
    private int _hotButton;          // 1 = Schedule, 2 = Edit, 3 = Activate/Deactivate, 4 = Delete

    public event EventHandler? ScheduleClicked;
    public event EventHandler? EditClicked;
    public event EventHandler? ToggleClicked;
    public event EventHandler? DeleteClicked;

    public RoomRowView(RoomRow row)
    {
        _row = row;
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        TabStop = false;
        Height = RoomsPx.S(this, 64);
    }

    private int S(int v) => RoomsPx.S(this, v);

    /// <summary>The four buttons (Schedule, Edit, Activate/Deactivate, Delete) and the status pill, right to left.</summary>
    private (Rectangle[] Buttons, Rectangle Pill) GetLayout()
    {
        int bh = S(30), y = (Height - bh) / 2, gap = S(8), right = Width - S(16);
        var delete = new Rectangle(right - S(64), y, S(64), bh);
        var toggle = new Rectangle(delete.Left - gap - S(88), y, S(88), bh);
        var edit = new Rectangle(toggle.Left - gap - S(56), y, S(56), bh);
        var schedule = new Rectangle(edit.Left - gap - S(80), y, S(80), bh);
        var pill = new Rectangle(schedule.Left - S(12) - S(92), (Height - S(24)) / 2, S(92), S(24));
        return (new[] { schedule, edit, toggle, delete }, pill);
    }

    private int HitTest(Point p)
    {
        var (buttons, _) = GetLayout();
        for (int i = 0; i < buttons.Length; i++)
            if (buttons[i].Contains(p)) return i + 1;
        return 0;
    }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }

    protected override void OnMouseLeave(EventArgs e)
    {
        _hover = false;
        _hotButton = 0;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        int hot = HitTest(e.Location);
        if (hot == _hotButton) return;
        _hotButton = hot;
        Cursor = hot == 0 ? Cursors.Default : Cursors.Hand;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (e.Button != MouseButtons.Left) return;
        switch (HitTest(e.Location))
        {
            case 1: ScheduleClicked?.Invoke(this, EventArgs.Empty); break;
            case 2: EditClicked?.Invoke(this, EventArgs.Empty); break;
            case 3: ToggleClicked?.Invoke(this, EventArgs.Empty); break;
            case 4: DeleteClicked?.Invoke(this, EventArgs.Empty); break;
        }
    }

    /// <summary>Colours and wording for the current state.</summary>
    private (Color Fore, Color Soft, string Label) Look() => !_row.Active
        ? (Theme.Muted, Theme.Track, "Inactive")
        : _row.State switch
        {
            RoomState.InUse => (Theme.Primary, Theme.PrimarySoft, "In use"),
            RoomState.Sanitizing => (RoomColors.AmberText, RoomColors.AmberSoft, "Sanitizing"),
            _ => (Theme.Green, Theme.GreenSoft, "Vacant"),
        };

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        bool active = _row.Active;
        var (fore, soft, label) = Look();

        using (var back = new SolidBrush(_hover ? Theme.Subtle : Color.White))
            g.FillRectangle(back, ClientRectangle);
        using (var line = new Pen(Theme.Border))
            g.DrawLine(line, S(16), Height - 1, Width - S(16), Height - 1);

        // round icon
        var icon = new Rectangle(S(16), (Height - S(40)) / 2, S(40), S(40));
        using (var fill = new SolidBrush(soft))
            g.FillEllipse(fill, icon);
        TextRenderer.DrawText(g, "", Theme.GlyphFont(12f), icon, fore, Center);

        var (buttons, pill) = GetLayout();

        // name + details
        int tx = icon.Right + S(14);
        int tw = Math.Max(10, pill.Left - tx - S(12));
        TextRenderer.DrawText(g, _row.Name, Theme.Face(10f, FontStyle.Bold), new Rectangle(tx, S(11), tw, S(22)),
            active ? Theme.Text : Theme.Muted, LeftText);
        TextRenderer.DrawText(g, Details(), Theme.Face(8.5f), new Rectangle(tx, S(33), tw, S(20)), Theme.Muted, LeftText);

        // status pill
        using (var path = Ui.RoundRect(pill, pill.Height / 2f))
        using (var fill = new SolidBrush(soft))
            g.FillPath(fill, path);
        TextRenderer.DrawText(g, label, Theme.Face(8.5f, FontStyle.Bold), pill, fore, Center);

        DrawButton(g, buttons[0], "Schedule", _hotButton == 1, Theme.TextSoft, Theme.Primary, Theme.PrimarySoft);
        DrawButton(g, buttons[1], "Edit", _hotButton == 2, Theme.TextSoft, Theme.Primary, Theme.PrimarySoft);
        DrawButton(g, buttons[2], active ? "Deactivate" : "Activate", _hotButton == 3,
            active ? Theme.TextSoft : Theme.Green, active ? Theme.Danger : Theme.Green, active ? Color.FromArgb(253, 238, 238) : Theme.GreenSoft);
        DrawButton(g, buttons[3], "Delete", _hotButton == 4, Theme.TextSoft, Theme.Danger, Color.FromArgb(253, 238, 238));
    }

    private string Details()
    {
        static string T(DateTime d) => d.ToString("h:mm tt", CultureInfo.CurrentCulture);
        string who = _row.Groomer is { Length: > 0 } g ? g : "not assigned";

        if (!_row.Active)
            return _row.Upcoming > 0 ? $"Not offered for new bookings  ·  {_row.Upcoming} upcoming" : "Not offered for new bookings";

        switch (_row.State)
        {
            case RoomState.InUse:
                return $"Groomer: {who}  ·  until {T(_row.Until ?? DateTime.Now)}";
            case RoomState.Sanitizing:
                return $"Last groomer: {who}  ·  ready at {T(_row.Until ?? DateTime.Now)}";
            default:
                if (_row.Next is not { } next) return "No upcoming bookings";
                return $"Next: {next.ToString("MMM d, h:mm tt", CultureInfo.CurrentCulture)}  ·  {_row.Upcoming} upcoming";
        }
    }

    private void DrawButton(Graphics g, Rectangle r, string text, bool hot, Color idleText, Color hotText, Color hotFill)
    {
        using (var path = Ui.RoundRect(r, 8))
        {
            using var fill = new SolidBrush(hot ? hotFill : Color.White);
            using var pen = new Pen(hot ? hotText : Theme.Border);
            g.FillPath(fill, path);
            g.DrawPath(pen, path);
        }
        TextRenderer.DrawText(g, text, Theme.Face(8.5f, FontStyle.Bold), r, hot ? hotText : idleText, Center);
    }
}

// ====================================================================== add / rename dialog

internal sealed class RoomEditDialog : DialogBase
{
    private readonly RoomRow? _existing;
    private readonly TextBox nameBox = new();

    /// <summary>The name as saved, for the audit log.</summary>
    public string? SavedName { get; private set; }

    public RoomEditDialog(RoomRow? existing = null)
        : base(existing is null ? "Add Room" : "Edit Room", "Save", "",
               existing is null ? "Add a room appointments can use" : existing.Name)
    {
        _existing = existing;
        nameBox.MaxLength = 60;
        nameBox.PlaceholderText = "e.g. Grooming Room 3";
        if (existing is not null) nameBox.Text = existing.Name;

        AddRow("Room name *", nameBox);
        FitHeight();
        Shown += (_, _) => { nameBox.Focus(); nameBox.SelectAll(); };
    }

    protected override void OnSaveClicked()
    {
        string name = nameBox.Text.Trim();
        if (name.Length == 0)
        {
            ShowError("Enter the room's name.");
            return;
        }

        try
        {
            if (RoomScheduleRepository.NameTaken(name, _existing?.Id))
            {
                ShowError($"There is already a room called \"{name}\".");
                return;
            }

            if (_existing is null) RoomScheduleRepository.Add(name);
            else RoomScheduleRepository.Rename(_existing.Id, name);

            SavedName = name;
            DialogResult = DialogResult.OK;
        }
        catch (SqlException ex)
        {
            ShowError("Could not save: " + ex.Message);
        }
    }
}

// ====================================================================== schedule view

/// <summary>
/// One room's day: every booking with its time, pet, service and groomer, followed by the sanitizing block after it.
/// ‹ › move between days.
/// </summary>
internal sealed class RoomScheduleDialog : DialogBase
{
    private readonly RoomRow _room;
    private DateTime _day = DateTime.Today;
    private readonly Label _dayLabel = Ui.Text("", 10f, Theme.Text, Color.White, FontStyle.Bold);
    private readonly FlowLayoutPanel _list = new()
    {
        Dock = DockStyle.Fill,
        FlowDirection = FlowDirection.TopDown,
        WrapContents = false,
        AutoScroll = true,
        BackColor = Color.White,
        Margin = Padding.Empty,
        Padding = Padding.Empty,
    };
    private readonly Label _empty;

    public RoomScheduleDialog(RoomRow room)
        : base($"{room.Name} schedule", "Close", "", "Bookings, the groomer, and the sanitizing time after each one")
    {
        _room = room;

        // Read-only: only "Close" is needed.
        SaveButton.Visible = false;
        CancelButtonControl.Text = "Close";
        AcceptButton = CancelButtonControl;

        int s(int v) => RoomsPx.S(this, v);
        ClientSize = new Size(s(620), ClientSize.Height);

        // ---- day navigation
        var nav = new Panel { Height = 42, BackColor = Color.White, Dock = DockStyle.Fill, Margin = new Padding(0, 0, 0, 6) };
        var prev = new PawButton { Text = "‹", Kind = PawButtonKind.Outline, Size = new Size(40, 34) };
        var next = new PawButton { Text = "›", Kind = PawButtonKind.Outline, Size = new Size(40, 34) };
        var today = new PawButton { Text = "Today", Kind = PawButtonKind.Outline, Size = new Size(76, 34) };
        prev.Click += (_, _) => { _day = _day.AddDays(-1); LoadDay(); };
        next.Click += (_, _) => { _day = _day.AddDays(1); LoadDay(); };
        today.Click += (_, _) => { _day = DateTime.Today; LoadDay(); };
        nav.Controls.Add(prev);
        nav.Controls.Add(next);
        nav.Controls.Add(_dayLabel);
        nav.Controls.Add(today);
        nav.Resize += (_, _) =>
        {
            prev.Location = new Point(0, 4);
            next.Location = new Point(prev.Right + 6, 4);
            today.Location = new Point(nav.Width - today.Width, 4);
            _dayLabel.SetBounds(next.Right + 14, 4, Math.Max(10, today.Left - next.Right - 24), 34);
        };

        // ---- list of blocks (a fixed-height host, so the dialog can size itself)
        _empty = Ui.Text("No bookings on this day\nThis room is vacant.", 9f, Theme.Muted, Color.White,
            FontStyle.Regular, ContentAlignment.MiddleCenter);
        _empty.Dock = DockStyle.Fill;
        _empty.Visible = false;
        FlowLists.NoHorizontalScroll(_list);
        _list.Resize += (_, _) => FitRows();

        var host = new Panel
        {
            BackColor = Color.White,
            Dock = DockStyle.Fill,
            Size = new Size(10, s(320)),
            MinimumSize = new Size(0, s(320)),
            Margin = new Padding(0, 0, 0, 4),
        };
        host.Controls.Add(_list);
        host.Controls.Add(_empty);

        Grid.Controls.Add(nav, 0, 0);
        Grid.SetColumnSpan(nav, 2);
        Grid.Controls.Add(host, 0, 1);
        Grid.SetColumnSpan(host, 2);

        FitHeight();
        LoadDay();
    }

    protected override void OnSaveClicked() { }   // never shown

    private void FitRows()
    {
        int w = Math.Max(10, _list.ClientSize.Width);
        foreach (Control c in _list.Controls)
            if (c.Width != w) c.Width = w;
    }

    private static string StatusText(string status) => status switch
    {
        "CheckedIn" => "Checked in",
        _ => status,
    };

    private void LoadDay()
    {
        _dayLabel.Text = (_day == DateTime.Today ? "Today  ·  " : "") + _day.ToString("dddd, MMM d, yyyy", CultureInfo.CurrentCulture);

        List<RoomBooking> bookings;
        try { bookings = RoomScheduleRepository.GetBookings(_day, _day.AddDays(1), _room.Id); }
        catch (SqlException ex)
        {
            ShowError("Couldn't load the schedule: " + ex.Message);
            return;
        }
        ShowError("");

        DateTime now = DateTime.Now;
        bool roomBusy = bookings.Any(x => x.Status != "Completed" && now >= x.Start && now < x.End);

        _list.SuspendLayout();
        foreach (var old in _list.Controls.Cast<Control>().ToList()) { _list.Controls.Remove(old); old.Dispose(); }
        foreach (var b in bookings)
        {
            bool done = b.Status == "Completed";
            bool inUse = !done && now >= b.Start && now < b.End;
            string who = b.Groomer is { Length: > 0 } g ? g : "not assigned";
            _list.Controls.Add(new ScheduleBlockView(new ScheduleBlock(BlockKind.Booking, b.Start, b.End,
                $"{b.Pet}  ·  {b.Service}", $"Groomer: {who}", inUse ? "In use now" : StatusText(b.Status), inUse))
            { Margin = Padding.Empty });

            var cleanEnd = b.End.AddMinutes(RoomScheduleRepository.SanitizeMinutes);
            bool cleaning = !roomBusy && now >= (done ? b.Start : b.End) && now < cleanEnd;
            _list.Controls.Add(new ScheduleBlockView(new ScheduleBlock(BlockKind.Sanitize, b.End, cleanEnd,
                "Sanitizing", "", cleaning ? "Sanitizing now" : "", cleaning))
            { Margin = Padding.Empty });
        }
        FitRows();
        _list.ResumeLayout(true);

        _list.Visible = bookings.Count > 0;
        _empty.Visible = bookings.Count == 0;
    }
}

internal enum BlockKind { Booking, Sanitize }

/// <summary>One line of a room's day: a booking, or the sanitizing time after it. <paramref name="Now"/> highlights what is happening right now.</summary>
internal sealed record ScheduleBlock(BlockKind Kind, DateTime Start, DateTime End, string Title, string Sub, string Status, bool Now);

internal sealed class ScheduleBlockView : Control
{
    private const TextFormatFlags LeftText = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
        TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;
    private const TextFormatFlags Center = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
        TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

    private readonly ScheduleBlock _b;

    public ScheduleBlockView(ScheduleBlock block)
    {
        _b = block;
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        TabStop = false;
        Height = RoomsPx.S(this, block.Kind == BlockKind.Booking ? 62 : 40);
    }

    private int S(int v) => RoomsPx.S(this, v);

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        bool booking = _b.Kind == BlockKind.Booking;
        static string T(DateTime d) => d.ToString("h:mm tt", CultureInfo.CurrentCulture);

        var card = new Rectangle(S(2), S(3), Width - S(4) - 1, Height - S(6) - 1);
        Color fill = booking ? (_b.Now ? Theme.PrimarySoft : Theme.Subtle) : RoomColors.AmberSoft;
        Color edge = _b.Now ? (booking ? Theme.Primary : RoomColors.AmberText) : Theme.Border;
        using (var path = Ui.RoundRect(card, 8))
        using (var brush = new SolidBrush(fill))
        using (var pen = new Pen(edge))
        {
            g.FillPath(brush, path);
            g.DrawPath(pen, path);
        }

        Color accent = booking ? Theme.Primary : RoomColors.AmberText;
        int x = card.Left + S(14);
        int mid = card.Left + S(190);

        // pill on the right
        Rectangle pill = Rectangle.Empty;
        if (_b.Status.Length > 0)
        {
            pill = new Rectangle(card.Right - S(14) - S(104), card.Top + (card.Height - S(24)) / 2, S(104), S(24));
            var (pf, pb) = PillColors(_b.Status);
            using (var path = Ui.RoundRect(pill, pill.Height / 2f))
            using (var brush = new SolidBrush(pb))
                g.FillPath(brush, path);
            TextRenderer.DrawText(g, _b.Status, Theme.Face(8.5f, FontStyle.Bold), pill, pf, Center);
        }
        int right = (pill.IsEmpty ? card.Right - S(14) : pill.Left - S(10));

        string range = $"{T(_b.Start)} – {T(_b.End)}";
        if (booking)
        {
            TextRenderer.DrawText(g, range, Theme.Face(9.5f, FontStyle.Bold), new Rectangle(x, card.Top, mid - x - S(8), card.Height), accent, LeftText);
            TextRenderer.DrawText(g, _b.Title, Theme.Face(9.5f, FontStyle.Bold), new Rectangle(mid, card.Top + S(8), Math.Max(10, right - mid), S(22)), Theme.Text, LeftText);
            TextRenderer.DrawText(g, _b.Sub, Theme.Face(8.5f), new Rectangle(mid, card.Top + S(30), Math.Max(10, right - mid), S(20)), Theme.TextSoft, LeftText);
        }
        else
        {
            TextRenderer.DrawText(g, range, Theme.Face(9f, FontStyle.Bold), new Rectangle(x, card.Top, mid - x - S(8), card.Height), accent, LeftText);
            TextRenderer.DrawText(g, _b.Title + "  ·  room is being cleaned", Theme.Face(8.5f), new Rectangle(mid, card.Top, Math.Max(10, right - mid), card.Height), RoomColors.AmberText, LeftText);
        }
    }

    private static (Color Fore, Color Back) PillColors(string status) => status switch
    {
        "In use now" => (Theme.Primary, Color.White),
        "Sanitizing now" => (RoomColors.AmberText, Color.White),
        "Completed" => (Theme.Green, Theme.GreenSoft),
        _ => (Theme.TextSoft, Theme.Track),
    };
}

// ====================================================================== data

internal enum RoomState { Vacant, InUse, Sanitizing }

/// <summary>
/// A room with what is happening in it right now. <paramref name="Groomer"/> / <paramref name="Until"/> are filled while it is in use
/// (until the service ends) or sanitizing (until it is ready again). <paramref name="Upcoming"/> counts bookings still to come.
/// </summary>
internal sealed record RoomRow(int Id, string Name, bool Active, RoomState State, string? Groomer, DateTime? Until, int Upcoming, DateTime? Next);

/// <summary>An appointment that uses a room. <paramref name="End"/> is the start plus the service's duration.</summary>
internal sealed record RoomBooking(int AppointmentId, int RoomId, DateTime Start, DateTime End, string? Groomer, string Pet, string Service, string Status);

/// <summary>Reads and writes dbo.Rooms and works out each room's status from the appointments.</summary>
internal static class RoomScheduleRepository
{
    /// <summary>Minutes a room stays "sanitizing" after a booking ends. Same 30 minutes as the booking gap on the appointment form.</summary>
    public const int SanitizeMinutes = EmployeeData.BookingGapMinutes;

    private static SqlConnection Open()
    {
        var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        return conn;
    }

    public static List<RoomRow> GetRooms(DateTime now)
    {
        using var conn = Open();

        var rooms = new List<(int Id, string Name, bool Active)>();
        using (var cmd = new SqlCommand("SELECT RoomId, Name, IsActive FROM dbo.Rooms ORDER BY IsActive DESC, Name", conn))
        using (var reader = cmd.ExecuteReader())
            while (reader.Read())
                rooms.Add((reader.GetInt32(0), reader.GetString(1), reader.GetBoolean(2)));

        // From yesterday on: covers a booking that is running now and everything still to come.
        var bookings = QueryBookings(conn, now.AddDays(-1), null, null);

        var list = new List<RoomRow>();
        foreach (var (id, name, active) in rooms)
        {
            var mine = bookings.Where(b => b.RoomId == id).ToList();

            var current = mine.FirstOrDefault(b => b.Status != "Completed" && now >= b.Start && now < b.End);
            // A Completed booking means the service is over, so the room goes straight to sanitizing.
            var cleaning = current is not null ? null
                : mine.Where(b => now >= (b.Status == "Completed" ? b.Start : b.End) && now < b.End.AddMinutes(SanitizeMinutes))
                      .OrderByDescending(b => b.End).FirstOrDefault();
            var future = mine.Where(b => b.Start >= now && b.Status != "Completed").ToList();

            var state = current is not null ? RoomState.InUse : cleaning is not null ? RoomState.Sanitizing : RoomState.Vacant;
            var busy = current ?? cleaning;
            DateTime? until = current?.End ?? cleaning?.End.AddMinutes(SanitizeMinutes);

            list.Add(new RoomRow(id, name, active, state, busy?.Groomer, until, future.Count,
                future.Count > 0 ? future.Min(b => b.Start) : null));
        }
        return list;
    }

    /// <summary>Bookings in a room (or every room when <paramref name="roomId"/> is null) that start in [from, to). Cancelled ones are left out.</summary>
    public static List<RoomBooking> GetBookings(DateTime from, DateTime to, int roomId)
    {
        using var conn = Open();
        return QueryBookings(conn, from, to, roomId);
    }

    private static List<RoomBooking> QueryBookings(SqlConnection conn, DateTime from, DateTime? to, int? roomId)
    {
        const string sql = @"
SELECT a.AppointmentId, a.RoomId, a.StartTime, DATEADD(MINUTE, s.DurationMinutes, a.StartTime), g.Name, p.Name, s.Name, a.Status
FROM dbo.Appointments a
JOIN dbo.Services s ON s.ServiceId = a.ServiceId
JOIN dbo.Pets p ON p.PetId = a.PetId
LEFT JOIN dbo.Groomers g ON g.GroomerId = a.GroomerId
WHERE a.RoomId IS NOT NULL
  AND a.Status <> N'Cancelled'
  AND a.StartTime >= @from
  AND (@to IS NULL OR a.StartTime < @to)
  AND (@room IS NULL OR a.RoomId = @room)
ORDER BY a.StartTime";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add("@from", System.Data.SqlDbType.DateTime2).Value = from;
        cmd.Parameters.Add("@to", System.Data.SqlDbType.DateTime2).Value = to is DateTime t ? (object)t : DBNull.Value;
        cmd.Parameters.Add("@room", System.Data.SqlDbType.Int).Value = roomId is int r ? (object)r : DBNull.Value;

        using var reader = cmd.ExecuteReader();
        var list = new List<RoomBooking>();
        while (reader.Read())
            list.Add(new RoomBooking(reader.GetInt32(0), reader.GetInt32(1), reader.GetDateTime(2), reader.GetDateTime(3),
                reader.IsDBNull(4) ? null : reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetString(7)));
        return list;
    }

    public static bool NameTaken(string name, int? exceptId)
    {
        using var conn = Open();
        using var cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.Rooms WHERE Name = @n AND (@id IS NULL OR RoomId <> @id)", conn);
        cmd.Parameters.Add("@n", System.Data.SqlDbType.NVarChar, 60).Value = name.Trim();
        cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = exceptId is int v ? (object)v : DBNull.Value;
        return (int)cmd.ExecuteScalar()! > 0;
    }

    public static void Add(string name)
    {
        using var conn = Open();
        using var cmd = new SqlCommand("INSERT INTO dbo.Rooms (Name) VALUES (@n)", conn);
        cmd.Parameters.Add("@n", System.Data.SqlDbType.NVarChar, 60).Value = name.Trim();
        cmd.ExecuteNonQuery();
    }

    public static void Rename(int id, string name)
    {
        using var conn = Open();
        using var cmd = new SqlCommand("UPDATE dbo.Rooms SET Name = @n WHERE RoomId = @id", conn);
        cmd.Parameters.Add("@n", System.Data.SqlDbType.NVarChar, 60).Value = name.Trim();
        cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
        cmd.ExecuteNonQuery();
    }

    public static void SetActive(int id, bool active)
    {
        using var conn = Open();
        using var cmd = new SqlCommand("UPDATE dbo.Rooms SET IsActive = @a WHERE RoomId = @id", conn);
        cmd.Parameters.Add("@a", System.Data.SqlDbType.Bit).Value = active;
        cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Deletes a room. If an open appointment (upcoming, or still running) uses it, nothing is deleted and the number of such
    /// appointments is returned. Otherwise returns 0; finished and cancelled appointments keep existing but lose their room link.
    /// </summary>
    public static int TryDelete(int roomId)
    {
        using var conn = Open();
        using var tx = conn.BeginTransaction();

        const string checkSql = @"
SELECT COUNT(*)
FROM dbo.Appointments a
JOIN dbo.Services s ON s.ServiceId = a.ServiceId
WHERE a.RoomId = @id
  AND a.Status NOT IN (N'Cancelled', N'Completed')
  AND DATEADD(MINUTE, s.DurationMinutes, a.StartTime) > @now";

        int blocking;
        using (var cmd = new SqlCommand(checkSql, conn, tx))
        {
            cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = roomId;
            cmd.Parameters.Add("@now", System.Data.SqlDbType.DateTime2).Value = DateTime.Now;
            blocking = (int)cmd.ExecuteScalar()!;
        }
        if (blocking > 0)
        {
            tx.Rollback();
            return blocking;
        }

        using (var cmd = new SqlCommand("UPDATE dbo.Appointments SET RoomId = NULL WHERE RoomId = @id", conn, tx))
        {
            cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = roomId;
            cmd.ExecuteNonQuery();
        }
        using (var cmd = new SqlCommand("DELETE FROM dbo.Rooms WHERE RoomId = @id", conn, tx))
        {
            cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = roomId;
            cmd.ExecuteNonQuery();
        }
        tx.Commit();
        return 0;
    }
}

/// <summary>Helpers for the top-down FlowLayoutPanels that hold full-width rows.</summary>
internal static class FlowLists
{
    /// <summary>
    /// Keeps the vertical scrollbar but never shows a horizontal one. Rows are sized to the panel's client width, so when the
    /// vertical scrollbar appears (e.g. after the window is made smaller) the rows were briefly wider than the new client area
    /// and WinForms added a horizontal scrollbar under them.
    /// </summary>
    public static void NoHorizontalScroll(FlowLayoutPanel panel)
    {
        panel.AutoScroll = false;
        panel.HorizontalScroll.Enabled = false;
        panel.HorizontalScroll.Visible = false;
        panel.HorizontalScroll.Maximum = 0;
        panel.AutoScroll = true;
    }
}
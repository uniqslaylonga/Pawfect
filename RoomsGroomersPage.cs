using System.Drawing.Drawing2D;
using System.Globalization;
using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Rooms &amp; Groomers: its own page next to Dashboard, Appointments, Billing and Lookup. Staff can see every groomer and
/// room, how many bookings each one has coming up, add new ones, rename them, and switch them off (they then stop being
/// offered when an appointment is scheduled, but old appointments keep them).
/// Built from the same pieces as the other pages: Theme colours, CardPanel, PrimaryButton, IconBadge and DialogBase.
/// </summary>
internal sealed class RoomsGroomersPage : UserControl
{
    private readonly ResourcePanel _groomers = new("Groomers", "Groomer", "\uE77B");
    private readonly ResourcePanel _rooms = new("Rooms", "Room", "\uE80F");

    /// <summary>Something was added, renamed or switched on/off. The text is for the audit log.</summary>
    public event EventHandler<(string Action, string Detail)>? Changed;

    public RoomsGroomersPage()
    {
        DoubleBuffered = true;
        BackColor = Theme.Bg;
        Padding = new Padding(RoomsPx.S(this, 20));

        // The 30-minute rule, so staff can see it without having to hit the error first.
        var banner = new Panel { Dock = DockStyle.Top, Height = RoomsPx.S(this, 56), BackColor = Theme.Bg };
        banner.Paint += (_, e) => PaintBanner(e.Graphics, banner);

        var cards = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Theme.Bg,
            Margin = Padding.Empty,
        };
        cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        cards.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        int gap = RoomsPx.S(this, 8);
        _groomers.Dock = DockStyle.Fill;
        _groomers.Margin = new Padding(0, RoomsPx.S(this, 12), gap, 0);
        _rooms.Dock = DockStyle.Fill;
        _rooms.Margin = new Padding(gap, RoomsPx.S(this, 12), 0, 0);
        cards.Controls.Add(_groomers, 0, 0);
        cards.Controls.Add(_rooms, 1, 0);

        Controls.Add(cards);     // docked first = fills what the banner leaves
        Controls.Add(banner);

        _groomers.AddClicked += (_, _) => Add(ResourceKind.Groomer);
        _rooms.AddClicked += (_, _) => Add(ResourceKind.Room);
        _groomers.EditClicked += (_, row) => Edit(ResourceKind.Groomer, row);
        _rooms.EditClicked += (_, row) => Edit(ResourceKind.Room, row);
        _groomers.ToggleClicked += (_, row) => Toggle(ResourceKind.Groomer, row);
        _rooms.ToggleClicked += (_, row) => Toggle(ResourceKind.Room, row);
    }

    private void PaintBanner(Graphics g, Control banner)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var r = new Rectangle(0, 0, banner.Width - 1, banner.Height - 1);
        using (var path = Ui.RoundRect(r, 10))
        using (var fill = new SolidBrush(Theme.PrimarySoft))
        using (var pen = new Pen(Color.FromArgb(214, 229, 249)))
        {
            g.FillPath(fill, path);
            g.DrawPath(pen, path);
        }

        int s(int v) => RoomsPx.S(this, v);
        var badge = new Rectangle(s(14), (banner.Height - s(34)) / 2, s(34), s(34));
        using (var path = Ui.RoundRect(badge, 10))
        using (var fill = new SolidBrush(Color.White))
            g.FillPath(fill, path);
        TextRenderer.DrawText(g, "\uE121", Theme.GlyphFont(11f), badge, Theme.Primary,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);

        const TextFormatFlags left = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
                                     TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;
        int gapMin = EmployeeData.BookingGapMinutes;
        int x = badge.Right + s(12);
        int w = banner.Width - x - s(14);
        TextRenderer.DrawText(g, $"{gapMin}-minute spacing between bookings", Theme.Face(9.5f, FontStyle.Bold),
            new Rectangle(x, s(8), w, s(20)), Theme.Text, left);
        var example = DateTime.Today.AddHours(13);
        TextRenderer.DrawText(g,
            $"A groomer or room booked at {example.ToString("h:mm tt", CultureInfo.CurrentCulture)} can take the next booking at " +
            $"{example.AddMinutes(gapMin).ToString("h:mm tt", CultureInfo.CurrentCulture)}.",
            Theme.Face(8.5f), new Rectangle(x, s(28), w, s(20)), Theme.TextSoft, left);
    }

    /// <summary>Reloads both lists from the database (called every time the page is opened).</summary>
    public void Reload()
    {
        try
        {
            _groomers.SetData(ResourceRepository.GetAll(ResourceKind.Groomer));
            _rooms.SetData(ResourceRepository.GetAll(ResourceKind.Room));
        }
        catch (SqlException ex)
        {
            Msg.Show(FindForm(), "Couldn't load groomers and rooms.\n\nMake sure Sql/appointment_scheduling.sql has been run.\n\n" + ex.Message,
                "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void Add(ResourceKind kind)
    {
        using var dlg = new ResourceDialog(kind);
        if (dlg.ShowDialog(FindForm()) != DialogResult.OK) return;
        Reload();
        Changed?.Invoke(this, ($"{ResourceDialog.Noun(kind)} Added", $"Added {ResourceDialog.Noun(kind).ToLowerInvariant()} {dlg.SavedName}"));
    }

    private void Edit(ResourceKind kind, ResourceRow row)
    {
        using var dlg = new ResourceDialog(kind, row);
        if (dlg.ShowDialog(FindForm()) != DialogResult.OK) return;
        Reload();
        Changed?.Invoke(this, ($"{ResourceDialog.Noun(kind)} Updated", $"{ResourceDialog.Noun(kind)} \"{row.Name}\" is now \"{dlg.SavedName}\""));
    }

    private void Toggle(ResourceKind kind, ResourceRow row)
    {
        string noun = ResourceDialog.Noun(kind).ToLowerInvariant();
        bool activate = !row.Active;

        if (!activate)
        {
            string extra = row.Upcoming > 0
                ? $"{row.Upcoming} upcoming appointment{(row.Upcoming == 1 ? "" : "s")} already use this {noun}. They keep it, but it won't be offered for new bookings."
                : $"It won't be offered when scheduling new appointments.";
            if (Msg.Show(FindForm(), $"Deactivate {row.Name}?\n\n{extra}", "Pawfect", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
        }

        try { ResourceRepository.SetActive(kind, row.Id, activate); }
        catch (SqlException ex)
        {
            Msg.Show(FindForm(), "Couldn't save.\n\n" + ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        Reload();
        Changed?.Invoke(this, ($"{ResourceDialog.Noun(kind)} {(activate ? "Activated" : "Deactivated")}", $"{ResourceDialog.Noun(kind)} \"{row.Name}\""));
    }
}

/// <summary>Scales the page's pixel sizes to the screen (this page is built in code, so WinForms doesn't do it).</summary>
internal static class RoomsPx
{
    public static int S(Control c, int v) => (int)Math.Round(v * Math.Max(1f, c.DeviceDpi / 96f));
}

// ====================================================================== card with a list

/// <summary>One white card: title, count, "+ Add" button and the rows.</summary>
internal sealed class ResourcePanel : CardPanel
{
    private readonly string _noun;
    private readonly string _glyph;
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

    public event EventHandler? AddClicked;
    public event EventHandler<ResourceRow>? EditClicked;
    public event EventHandler<ResourceRow>? ToggleClicked;

    public ResourcePanel(string title, string noun, string glyph)
    {
        _noun = noun;
        _glyph = glyph;
        FillColor = Color.White;
        BorderColor = Theme.Border;
        CornerColor = Theme.Bg;
        Radius = 10;
        BackColor = Color.White;
        Padding = new Padding(1, 8, 1, 8);   // keeps the rows inside the rounded corners

        int s(int v) => RoomsPx.S(this, v);

        _title = Ui.Text(title, 12f, Theme.Text, Color.White, FontStyle.Bold);
        _count = Ui.Text("", 8.5f, Theme.Muted, Color.White);
        _add = new PrimaryButton($"+ Add {noun}")
        {
            BackColor = Color.White,
            Font = Theme.Face(9f, FontStyle.Bold),
            Size = new Size(s(Math.Max(120, 40 + noun.Length * 9)), s(34)),
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

        _empty = Ui.Text($"No {title.ToLowerInvariant()} yet\nUse “+ Add {noun}” to create the first one.", 9f, Theme.Muted, Color.White,
            FontStyle.Regular, ContentAlignment.MiddleCenter);
        _empty.Dock = DockStyle.Fill;
        _empty.Visible = false;

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

    public void SetData(IReadOnlyList<ResourceRow> rows)
    {
        int active = rows.Count(r => r.Active);
        _count.Text = rows.Count == 0
            ? "Nothing added yet"
            : $"{active} active" + (rows.Count > active ? $"  \u00B7  {rows.Count - active} inactive" : "");

        _list.SuspendLayout();
        foreach (var old in _list.Controls.Cast<Control>().ToList()) { _list.Controls.Remove(old); old.Dispose(); }
        foreach (var row in rows)
        {
            var view = new ResourceRowView(row, _glyph, _noun) { Margin = Padding.Empty };
            view.EditClicked += (_, _) => EditClicked?.Invoke(this, row);
            view.ToggleClicked += (_, _) => ToggleClicked?.Invoke(this, row);
            _list.Controls.Add(view);
        }
        FitRows();
        _list.ResumeLayout(true);

        _list.Visible = rows.Count > 0;
        _empty.Visible = rows.Count == 0;
    }

    private void FitRows()
    {
        int w = Math.Max(10, _list.ClientSize.Width);
        foreach (Control c in _list.Controls)
            if (c.Width != w) c.Width = w;
    }
}

// ====================================================================== one row

/// <summary>A groomer or room: round icon, name, "3 upcoming · Next ...", status pill, Edit and Deactivate buttons.</summary>
internal sealed class ResourceRowView : Control
{
    private const TextFormatFlags Left = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
        TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;
    private const TextFormatFlags Center = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
        TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

    private readonly ResourceRow _row;
    private readonly string _glyph;
    private bool _hover;
    private int _hotButton;          // 1 = Edit, 2 = Activate/Deactivate

    public event EventHandler? EditClicked;
    public event EventHandler? ToggleClicked;

    public ResourceRowView(ResourceRow row, string glyph, string noun)
    {
        _row = row;
        _glyph = glyph;
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        TabStop = false;
        Height = RoomsPx.S(this, 64);
    }

    private int S(int v) => RoomsPx.S(this, v);

    private (Rectangle Edit, Rectangle Toggle, Rectangle Pill) Layout()
    {
        int bh = S(30), y = (Height - bh) / 2, right = Width - S(16);
        var toggle = new Rectangle(right - S(88), y, S(88), bh);
        var edit = new Rectangle(toggle.Left - S(8) - S(56), y, S(56), bh);
        var pill = new Rectangle(edit.Left - S(12) - S(64), (Height - S(24)) / 2, S(64), S(24));
        return (edit, toggle, pill);
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
        var (edit, toggle, _) = Layout();
        int hot = edit.Contains(e.Location) ? 1 : toggle.Contains(e.Location) ? 2 : 0;
        if (hot == _hotButton) return;
        _hotButton = hot;
        Cursor = hot == 0 ? Cursors.Default : Cursors.Hand;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (e.Button != MouseButtons.Left) return;
        var (edit, toggle, _) = Layout();
        if (edit.Contains(e.Location)) EditClicked?.Invoke(this, EventArgs.Empty);
        else if (toggle.Contains(e.Location)) ToggleClicked?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        bool active = _row.Active;

        using (var back = new SolidBrush(_hover ? Theme.Subtle : Color.White))
            g.FillRectangle(back, ClientRectangle);
        using (var line = new Pen(Theme.Border))
            g.DrawLine(line, S(16), Height - 1, Width - S(16), Height - 1);

        // round icon
        var icon = new Rectangle(S(16), (Height - S(40)) / 2, S(40), S(40));
        using (var fill = new SolidBrush(active ? Theme.PrimarySoft : Theme.Track))
            g.FillEllipse(fill, icon);
        TextRenderer.DrawText(g, _glyph, Theme.GlyphFont(12f), icon, active ? Theme.Primary : Theme.Muted, Center);

        var (edit, toggle, pill) = Layout();

        // name + details
        int tx = icon.Right + S(14);
        int tw = Math.Max(10, pill.Left - tx - S(12));
        TextRenderer.DrawText(g, _row.Name, Theme.Face(10f, FontStyle.Bold), new Rectangle(tx, S(11), tw, S(22)),
            active ? Theme.Text : Theme.Muted, Left);
        TextRenderer.DrawText(g, Details(), Theme.Face(8.5f), new Rectangle(tx, S(33), tw, S(20)), Theme.Muted, Left);

        // status pill
        using (var path = Ui.RoundRect(pill, pill.Height / 2f))
        using (var fill = new SolidBrush(active ? Theme.GreenSoft : Theme.Track))
            g.FillPath(fill, path);
        TextRenderer.DrawText(g, active ? "Active" : "Inactive", Theme.Face(8.5f, FontStyle.Bold), pill,
            active ? Theme.Green : Theme.Muted, Center);

        DrawButton(g, edit, "Edit", _hotButton == 1, Theme.TextSoft, Theme.Primary, Theme.PrimarySoft);
        DrawButton(g, toggle, active ? "Deactivate" : "Activate", _hotButton == 2,
            active ? Theme.TextSoft : Theme.Green, active ? Theme.Danger : Theme.Green, active ? Color.FromArgb(253, 238, 238) : Theme.GreenSoft);
    }

    private string Details()
    {
        if (_row.Upcoming == 0) return "No upcoming bookings";
        string next = _row.Next is { } n ? "  \u00B7  Next: " + n.ToString("MMM d, h:mm tt", CultureInfo.CurrentCulture) : "";
        return $"{_row.Upcoming} upcoming{next}";
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

// ====================================================================== add / edit dialog

internal sealed class ResourceDialog : DialogBase
{
    private readonly ResourceKind _kind;
    private readonly ResourceRow? _existing;
    private readonly TextBox nameBox = new();

    /// <summary>The name as saved, for the audit log.</summary>
    public string? SavedName { get; private set; }

    public static string Noun(ResourceKind kind) => kind == ResourceKind.Groomer ? "Groomer" : "Room";

    public ResourceDialog(ResourceKind kind, ResourceRow? existing = null)
        : base(existing is null ? $"Add {Noun(kind)}" : $"Edit {Noun(kind)}", "Save",
               kind == ResourceKind.Groomer ? "\uE77B" : "\uE80F",
               existing is null
                   ? (kind == ResourceKind.Groomer ? "Add a groomer who can take appointments" : "Add a room appointments can use")
                   : existing.Name)
    {
        _kind = kind;
        _existing = existing;
        nameBox.MaxLength = kind == ResourceKind.Groomer ? 80 : 60;
        nameBox.PlaceholderText = kind == ResourceKind.Groomer ? "e.g. Anna Reyes" : "e.g. Grooming Room 3";
        if (existing is not null) nameBox.Text = existing.Name;

        AddRow(kind == ResourceKind.Groomer ? "Groomer name *" : "Room name *", nameBox);
        FitHeight();
        Shown += (_, _) => { nameBox.Focus(); nameBox.SelectAll(); };
    }

    protected override void OnSaveClicked()
    {
        string name = nameBox.Text.Trim();
        if (name.Length == 0)
        {
            ShowError(_kind == ResourceKind.Groomer ? "Enter the groomer's name." : "Enter the room's name.");
            return;
        }

        try
        {
            if (ResourceRepository.NameTaken(_kind, name, _existing?.Id))
            {
                ShowError($"There is already a {Noun(_kind).ToLowerInvariant()} called \"{name}\".");
                return;
            }

            if (_existing is null) ResourceRepository.Add(_kind, name);
            else ResourceRepository.Rename(_kind, _existing.Id, name);

            SavedName = name;
            DialogResult = DialogResult.OK;
        }
        catch (SqlException ex)
        {
            ShowError("Could not save: " + ex.Message);
        }
    }
}

// ====================================================================== data

internal enum ResourceKind { Groomer, Room }

/// <summary>A groomer or room with how many open appointments still use it.</summary>
internal sealed record ResourceRow(int Id, string Name, bool Active, int Upcoming, DateTime? Next);

/// <summary>Reads and writes dbo.Groomers and dbo.Rooms (they have the same shape). Nothing is ever deleted, only switched off.</summary>
internal static class ResourceRepository
{
    private static (string Table, string IdColumn, int NameLength) Meta(ResourceKind kind)
        => kind == ResourceKind.Groomer ? ("dbo.Groomers", "GroomerId", 80) : ("dbo.Rooms", "RoomId", 60);

    public static List<ResourceRow> GetAll(ResourceKind kind)
    {
        var (table, id, _) = Meta(kind);
        // Table / column names come from the constants above, never from the user.
        string sql = $@"
SELECT r.{id}, r.Name, r.IsActive,
       (SELECT COUNT(*)      FROM dbo.Appointments a WHERE a.{id} = r.{id} AND a.Status NOT IN (N'Cancelled', N'Completed') AND a.StartTime >= @now),
       (SELECT MIN(a.StartTime) FROM dbo.Appointments a WHERE a.{id} = r.{id} AND a.Status NOT IN (N'Cancelled', N'Completed') AND a.StartTime >= @now)
FROM {table} r
ORDER BY r.IsActive DESC, r.Name";

        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add("@now", System.Data.SqlDbType.DateTime2).Value = DateTime.Now;
        using var reader = cmd.ExecuteReader();
        var list = new List<ResourceRow>();
        while (reader.Read())
            list.Add(new ResourceRow(reader.GetInt32(0), reader.GetString(1), reader.GetBoolean(2), reader.GetInt32(3),
                reader.IsDBNull(4) ? null : reader.GetDateTime(4)));
        return list;
    }

    public static bool NameTaken(ResourceKind kind, string name, int? exceptId)
    {
        var (table, id, _) = Meta(kind);
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand($"SELECT COUNT(*) FROM {table} WHERE Name = @n AND (@id IS NULL OR {id} <> @id)", conn);
        cmd.Parameters.Add("@n", System.Data.SqlDbType.NVarChar, 80).Value = name.Trim();
        cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = exceptId is int v ? (object)v : DBNull.Value;
        return (int)cmd.ExecuteScalar()! > 0;
    }

    public static void Add(ResourceKind kind, string name)
    {
        var (table, _, length) = Meta(kind);
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand($"INSERT INTO {table} (Name) VALUES (@n)", conn);
        cmd.Parameters.Add("@n", System.Data.SqlDbType.NVarChar, length).Value = name.Trim();
        cmd.ExecuteNonQuery();
    }

    public static void Rename(ResourceKind kind, int id, string name)
    {
        var (table, idColumn, length) = Meta(kind);
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand($"UPDATE {table} SET Name = @n WHERE {idColumn} = @id", conn);
        cmd.Parameters.Add("@n", System.Data.SqlDbType.NVarChar, length).Value = name.Trim();
        cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
        cmd.ExecuteNonQuery();
    }

    public static void SetActive(ResourceKind kind, int id, bool active)
    {
        var (table, idColumn, _) = Meta(kind);
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand($"UPDATE {table} SET IsActive = @a WHERE {idColumn} = @id", conn);
        cmd.Parameters.Add("@a", System.Data.SqlDbType.Bit).Value = active;
        cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
        cmd.ExecuteNonQuery();
    }
}

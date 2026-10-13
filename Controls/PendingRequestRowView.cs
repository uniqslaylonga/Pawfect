using System.Drawing.Drawing2D;
using System.Globalization;

namespace MyApp.Controls;

/// <summary>
/// One row of the dashboard "Pending Requests" table: time, customer &amp; pet, request, status and actions.
/// The column widths match the header row on the card (TIME, CUSTOMER &amp; PET, REQUEST, STATUS, ACTIONS).
/// </summary>
internal sealed class PendingRequestRowView : Control
{
    public const int RowHeight = 58;

    /// <summary>Share of the width for each column; must match <c>pendingColumns</c> on the dashboard and add up to 100.</summary>
    public static readonly float[] ColumnPercents = { 14f, 30f, 24f, 16f, 16f };

    private const int CellPad = 14;   // the header labels are indented by the same amount

    private const TextFormatFlags LineFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
        TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;

    private const TextFormatFlags CenterFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
        TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

    private bool _hover;
    private bool _hoverView;
    private bool _hoverMore;

    public PendingRequestRowView(AppointmentItem item)
    {
        Item = item;
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        Height = RowHeight;
        TabStop = false;
    }

    public AppointmentItem Item { get; }

    public event EventHandler? ViewClicked;
    public event EventHandler? MoreClicked;

    private Rectangle Col(int i)
    {
        float x = 0;
        for (int j = 0; j < i; j++) x += Width * ColumnPercents[j] / 100f;
        return new Rectangle((int)x, 0, (int)(Width * ColumnPercents[i] / 100f), Height);
    }

    private Rectangle ViewRect()
    {
        var col = Col(4);
        return new Rectangle(col.X + CellPad, (Height - 28) / 2, 52, 28);
    }

    private Rectangle MoreRect()
    {
        var view = ViewRect();
        return new Rectangle(view.Right + 4, (Height - 28) / 2, 24, 28);
    }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }

    protected override void OnMouseLeave(EventArgs e)
    {
        _hover = _hoverView = _hoverMore = false;
        Cursor = Cursors.Default;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        bool view = ViewRect().Contains(e.Location);
        bool more = MoreRect().Contains(e.Location);
        if (view != _hoverView || more != _hoverMore)
        {
            _hoverView = view;
            _hoverMore = more;
            Invalidate();
        }
        Cursor = view || more ? Cursors.Hand : Cursors.Default;
        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (e.Button != MouseButtons.Left) return;

        if (ViewRect().Contains(e.Location)) ViewClicked?.Invoke(this, EventArgs.Empty);
        else if (MoreRect().Contains(e.Location)) MoreClicked?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var bg = new SolidBrush(_hover ? Theme.Subtle : Color.White))
            g.FillRectangle(bg, ClientRectangle);
        using (var line = new Pen(Theme.Border))
            g.DrawLine(line, 0, Height - 1, Width, Height - 1);

        var nameFont = Theme.Face(9f, FontStyle.Bold);
        var subFont = Theme.Face(8f);

        // TIME
        var c0 = Col(0);
        var today = DateTime.Today;
        string day = Item.Start.Date == today ? "Today"
            : Item.Start.Date == today.AddDays(1) ? "Tomorrow"
            : Item.Start.ToString(Item.Start.Year == today.Year ? "MMM d" : "MMM d, yyyy", CultureInfo.CurrentCulture);
        TextRenderer.DrawText(g, Item.Start.ToString("h:mm tt", CultureInfo.CurrentCulture), nameFont,
            new Rectangle(c0.X + CellPad, 10, c0.Width - CellPad, 18), Theme.Text, LineFlags);
        TextRenderer.DrawText(g, day, subFont,
            new Rectangle(c0.X + CellPad, 29, c0.Width - CellPad, 16), Item.Start.Date < today ? Theme.Danger : Theme.Muted, LineFlags);

        // CUSTOMER & PET: pet avatar, customer name, "Pet (Breed)"
        var c1 = Col(1);
        int avatar = 34;
        int avatarY = (Height - avatar) / 2;
        var avatarRect = new Rectangle(c1.X + CellPad, avatarY, avatar, avatar);
        using (var b = new SolidBrush(Color.FromArgb(254, 243, 224)))
            g.FillEllipse(b, avatarRect);
        string initial = Item.PetName.Trim().Length > 0 ? Item.PetName.Trim()[..1].ToUpperInvariant() : "";
        TextRenderer.DrawText(g, initial, Theme.Face(10f, FontStyle.Bold), avatarRect, Color.FromArgb(199, 122, 10), CenterFlags);

        int textX = avatarRect.Right + 10;
        int textW = Math.Max(20, c1.Right - textX - 6);
        string pet = string.IsNullOrWhiteSpace(Item.PetBreed) ? Item.PetName : $"{Item.PetName} ({Item.PetBreed})";
        TextRenderer.DrawText(g, Item.CustomerName, nameFont, new Rectangle(textX, 10, textW, 18), Theme.Text, LineFlags);
        TextRenderer.DrawText(g, pet, subFont, new Rectangle(textX, 29, textW, 16), Theme.Muted, LineFlags);

        // REQUEST: category on top, the service underneath (just the service when there is no category)
        var c2 = Col(2);
        int reqW = Math.Max(20, c2.Width - CellPad - 6);
        if (string.IsNullOrWhiteSpace(Item.Category))
        {
            TextRenderer.DrawText(g, Item.Service, nameFont, new Rectangle(c2.X + CellPad, 0, reqW, Height), Theme.Text, LineFlags);
        }
        else
        {
            TextRenderer.DrawText(g, Item.Category, nameFont, new Rectangle(c2.X + CellPad, 10, reqW, 18), Theme.Text, LineFlags);
            TextRenderer.DrawText(g, Item.Service, subFont, new Rectangle(c2.X + CellPad, 29, reqW, 16), Theme.Muted, LineFlags);
        }

        // STATUS
        var c3 = Col(3);
        DrawStatusPill(g, c3.X + CellPad, Height / 2, c3.Width - CellPad - 4);

        // ACTIONS: outlined "View" button and a "..." menu
        var view = ViewRect();
        using (var path = Ui.RoundRect(new RectangleF(view.X + 0.5f, view.Y + 0.5f, view.Width - 1, view.Height - 1), 6))
        {
            using (var fill = new SolidBrush(_hoverView ? Theme.PrimarySoft : Color.White))
                g.FillPath(fill, path);
            using var pen = new Pen(Theme.Primary);
            g.DrawPath(pen, path);
        }
        TextRenderer.DrawText(g, "View", Theme.Face(8.5f, FontStyle.Bold), view, Theme.Primary, CenterFlags);

        var more = MoreRect();
        if (_hoverMore)
        {
            using var b = new SolidBrush(Theme.PrimarySoft);
            g.FillEllipse(b, more.X, more.Y + 2, 24, 24);
        }
        TextRenderer.DrawText(g, "\uE712", Theme.GlyphFont(9f), more, _hoverMore ? Theme.Primary : Theme.Muted, CenterFlags);
    }

    /// <summary>Waiting requests get a soft blue badge; other statuses keep their usual colours.</summary>
    private void DrawStatusPill(Graphics g, int x, int centerY, int maxWidth)
    {
        bool waiting = Item.Status == AppointmentStatus.Pending;
        string text = waiting ? "Waiting" : AppointmentStatusStyle.Label(Item.Status);
        var (fore, fill, border) = AppointmentStatusStyle.Pill(waiting ? AppointmentStatus.Completed : Item.Status);

        var font = Theme.Face(8f, FontStyle.Bold);
        int w = Math.Min(Math.Max(20, maxWidth), TextRenderer.MeasureText(text, font).Width + 18);
        var rect = new Rectangle(x, centerY - 11, w, 22);

        using (var path = Ui.RoundRect(new RectangleF(rect.X + 0.5f, rect.Y + 0.5f, rect.Width - 1, rect.Height - 1), 11))
        using (var b = new SolidBrush(fill))
        using (var pen = new Pen(border))
        {
            g.FillPath(b, path);
            g.DrawPath(pen, path);
        }
        TextRenderer.DrawText(g, text, font, rect, fore, CenterFlags | TextFormatFlags.EndEllipsis);
    }
}

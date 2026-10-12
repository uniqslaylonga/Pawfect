using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace MyApp.Controls;

/// <summary>One row in the Customer &amp; Pet Lookup table: five text cells and a "Book" action.</summary>
internal sealed class LookupRowView : Control
{
    public const int CheckColumnWidth = 40;
    public const int RowHeight = 46;

    /// <summary>Share of the remaining width for: 5 text cells + actions. Matches the header in LookupPage.Designer.cs.</summary>
    private static readonly float[] ColumnPercents = { 20f, 16f, 15f, 20f, 11f, 18f };

    private const TextFormatFlags LineFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
        TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;

    private readonly string[] _cells;
    private bool _hover;
    private bool _hoverAction;
    private bool _hoverMore;
    private const int MoreWidth = 28;

    public LookupRowView(object source, string[] cells, string actionText)
    {
        Source = source;
        _cells = cells;
        ActionText = actionText;
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        Height = RowHeight;
        TabStop = false;
    }

    /// <summary>The CustomerRecord or PetRecord this row shows.</summary>
    public object Source { get; }

    public string ActionText { get; }

    public event EventHandler? ActionClicked;

    /// <summary>The "..." button was pressed, or the row was right-clicked.</summary>
    public event EventHandler? MoreClicked;

    private Rectangle Col(int i)
    {
        float avail = Math.Max(0, Width - CheckColumnWidth);
        float x = CheckColumnWidth;
        for (int j = 0; j < i; j++) x += avail * ColumnPercents[j] / 100f;
        return new Rectangle((int)x, 0, (int)(avail * ColumnPercents[i] / 100f), Height);
    }

    private Rectangle ActionRect
    {
        get
        {
            var c = Col(5);
            return new Rectangle(c.X, 10, Math.Max(10, c.Width - 8 - MoreWidth - 4), Height - 20);
        }
    }

    private Rectangle MoreRect
    {
        get
        {
            var a = ActionRect;
            return new Rectangle(a.Right + 4, 10, MoreWidth, Height - 20);
        }
    }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }

    protected override void OnMouseLeave(EventArgs e)
    {
        _hover = false;
        _hoverAction = false;
        _hoverMore = false;
        Cursor = Cursors.Default;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        bool over = ActionText.Length > 0 && ActionRect.Contains(e.Location);
        bool overMore = MoreRect.Contains(e.Location);
        if (over != _hoverAction || overMore != _hoverMore)
        {
            _hoverAction = over;
            _hoverMore = overMore;
            Cursor = over || overMore ? Cursors.Hand : Cursors.Default;
            Invalidate();
        }
        base.OnMouseMove(e);
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && ActionText.Length > 0 && ActionRect.Contains(e.Location))
            ActionClicked?.Invoke(this, EventArgs.Empty);
        else if (e.Button == MouseButtons.Right || (e.Button == MouseButtons.Left && MoreRect.Contains(e.Location)))
            MoreClicked?.Invoke(this, EventArgs.Empty);
        base.OnMouseClick(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        if (_hover) g.Clear(Theme.Subtle); else g.Clear(Color.White);
        using (var pen = new Pen(Theme.Border))
            g.DrawLine(pen, 0, Height - 1, Width, Height - 1);

        for (int i = 0; i < _cells.Length && i < 5; i++)
        {
            var rect = Col(i);
            rect.Inflate(-4, 0);
            bool first = i == 0;
            TextRenderer.DrawText(g, _cells[i], Theme.Face(first ? 9f : 8.5f, first ? FontStyle.Bold : FontStyle.Regular),
                rect, first ? Theme.Text : Theme.TextSoft, LineFlags);
        }

        if (ActionText.Length > 0)
        {
            var r = ActionRect;
            using var path = Ui.RoundRect(new RectangleF(r.X + 0.5f, r.Y + 0.5f, r.Width - 1, r.Height - 1), 6);
            using var fill = new SolidBrush(_hoverAction ? Theme.Primary : Theme.PrimarySoft);
            g.FillPath(fill, path);
            TextRenderer.DrawText(g, ActionText, Theme.Face(8f, FontStyle.Bold), r,
                _hoverAction ? Color.White : Theme.Primary,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding |
                TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
        }

        var m = MoreRect;
        using (var path = Ui.RoundRect(new RectangleF(m.X + 0.5f, m.Y + 0.5f, m.Width - 1, m.Height - 1), 6))
        using (var fill = new SolidBrush(_hoverMore ? Theme.PrimarySoft : Color.White))
        using (var pen = new Pen(_hoverMore ? Theme.Primary : Theme.Border))
        {
            g.FillPath(fill, path);
            g.DrawPath(pen, path);
        }
        TextRenderer.DrawText(g, "\uE712", Theme.GlyphFont(9f), m, Theme.TextSoft,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding |
            TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
    }
}

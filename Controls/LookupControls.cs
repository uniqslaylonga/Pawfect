using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace MyApp.Controls;

/// <summary>One row in the Customer &amp; Pet Lookup table: five text cells and a "Book" action.</summary>
internal sealed class LookupRowView : Control
{
    public const int CheckColumnWidth = 40;
    public const int RowHeight = 46;

    /// <summary>Share of the remaining width for: 5 text cells + actions. Matches the header in LookupPage.Designer.cs.</summary>
    public static readonly float[] ColumnPercents = { 18f, 14f, 13f, 18f, 10f, 27f };

    private const TextFormatFlags LineFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
        TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;

    private readonly string[] _cells;
    private bool _hover;
    private bool _hoverAction;
    private bool _hoverMore;
    private bool _hoverSecond;
    private const int MoreWidth = 28;

    /// <param name="secondActionText">Optional second button next to the action (e.g. "View Pets"). Empty means none.</param>
    public LookupRowView(object source, string[] cells, string actionText, string secondActionText = "")
    {
        Source = source;
        _cells = cells;
        ActionText = actionText;
        SecondActionText = secondActionText;
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        Height = RowHeight;
        TabStop = false;
    }

    /// <summary>The CustomerRecord or PetRecord this row shows.</summary>
    public object Source { get; }

    public string ActionText { get; }

    public string SecondActionText { get; }

    public event EventHandler? ActionClicked;

    /// <summary>The second button (e.g. "View Pets") was pressed.</summary>
    public event EventHandler? SecondActionClicked;

    /// <summary>The "..." button was pressed, or the row was right-clicked.</summary>
    public event EventHandler? MoreClicked;

    private Rectangle Col(int i)
    {
        float avail = Math.Max(0, Width - CheckColumnWidth);
        float x = CheckColumnWidth;
        for (int j = 0; j < i; j++) x += avail * ColumnPercents[j] / 100f;
        return new Rectangle((int)x, 0, (int)(avail * ColumnPercents[i] / 100f), Height);
    }

    // The last column holds [action] [second action] [...]; this is the width of the buttons before the "..." button.
    private int ButtonsWidth => Math.Max(10, Col(5).Width - 8 - MoreWidth - 4);

    private Rectangle ActionRect
    {
        get
        {
            var c = Col(5);
            int w = ButtonsWidth;
            if (SecondActionText.Length > 0) w = Math.Max(10, (int)((w - 6) * 0.36f));
            return new Rectangle(c.X, 10, w, Height - 20);
        }
    }

    private Rectangle SecondActionRect
    {
        get
        {
            var a = ActionRect;
            int left = a.Right + 6;
            return new Rectangle(left, 10, Math.Max(10, Col(5).X + ButtonsWidth - left), Height - 20);
        }
    }

    private Rectangle MoreRect => new(Col(5).X + ButtonsWidth + 4, 10, MoreWidth, Height - 20);

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }

    protected override void OnMouseLeave(EventArgs e)
    {
        _hover = false;
        _hoverAction = false;
        _hoverSecond = false;
        _hoverMore = false;
        Cursor = Cursors.Default;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        bool over = ActionText.Length > 0 && ActionRect.Contains(e.Location);
        bool overSecond = SecondActionText.Length > 0 && SecondActionRect.Contains(e.Location);
        bool overMore = MoreRect.Contains(e.Location);
        if (over != _hoverAction || overSecond != _hoverSecond || overMore != _hoverMore)
        {
            _hoverAction = over;
            _hoverSecond = overSecond;
            _hoverMore = overMore;
            Cursor = over || overSecond || overMore ? Cursors.Hand : Cursors.Default;
            Invalidate();
        }
        base.OnMouseMove(e);
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && ActionText.Length > 0 && ActionRect.Contains(e.Location))
            ActionClicked?.Invoke(this, EventArgs.Empty);
        else if (e.Button == MouseButtons.Left && SecondActionText.Length > 0 && SecondActionRect.Contains(e.Location))
            SecondActionClicked?.Invoke(this, EventArgs.Empty);
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

        if (SecondActionText.Length > 0)
        {
            var r = SecondActionRect;
            using var path = Ui.RoundRect(new RectangleF(r.X + 0.5f, r.Y + 0.5f, r.Width - 1, r.Height - 1), 6);
            using var fill = new SolidBrush(_hoverSecond ? Theme.Green : Theme.GreenSoft);
            g.FillPath(fill, path);
            TextRenderer.DrawText(g, SecondActionText, Theme.Face(8f, FontStyle.Bold), r,
                _hoverSecond ? Color.White : Theme.Green,
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

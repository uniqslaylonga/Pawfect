using System.Drawing.Drawing2D;
using System.Globalization;

namespace MyApp.Controls;

/// <summary>The little paw logo used on receipts.</summary>
internal static class PawIcon
{
    public static void Draw(Graphics g, float x, float y, float s, Color color)
    {
        using var brush = new SolidBrush(color);
        void Blob(float cx, float cy, float w, float h)
            => g.FillEllipse(brush, x + (cx - w / 2f) * s, y + (cy - h / 2f) * s, w * s, h * s);
        Blob(0.50f, 0.70f, 0.52f, 0.42f);
        Blob(0.12f, 0.43f, 0.20f, 0.26f);
        Blob(0.36f, 0.20f, 0.20f, 0.28f);
        Blob(0.64f, 0.20f, 0.20f, 0.28f);
        Blob(0.88f, 0.43f, 0.20f, 0.26f);
    }
}

/// <summary>
/// The full receipt drawn like a paper slip (dashed border, paw logo). Its height follows the number of items,
/// so put it in a scrolling panel. Shows only the values in <see cref="Data"/>.
/// </summary>
internal sealed class ReceiptPaper : Control
{
    public const int PaperWidth = 300;

    private ReceiptData? _data;

    public ReceiptPaper()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Theme.Bg;
        Width = PaperWidth;
        Height = 120;
        TabStop = false;
    }

    public ReceiptData? Data
    {
        get => _data;
        set
        {
            _data = value;
            Height = Render(null);
            Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var path = Ui.RoundRect(new RectangleF(0.5f, 0.5f, Width - 2, Height - 2), 8))
        using (var fill = new SolidBrush(Color.White))
        using (var pen = new Pen(Color.FromArgb(197, 207, 222)) { DashStyle = DashStyle.Dash })
        {
            g.FillPath(fill, path);
            g.DrawPath(pen, path);
        }

        Render(g);
    }

    /// <summary>Draws the receipt (when <paramref name="g"/> is given) and returns the height it needs.</summary>
    private int Render(Graphics? g)
    {
        var d = _data;
        if (d is null) return 120;

        int w = Width;
        int y = 16;
        var tiny = Theme.Face(7.5f);
        var tinyBold = Theme.Face(7.5f, FontStyle.Bold);
        var bold = Theme.Face(8.5f, FontStyle.Bold);
        var nameFont = Theme.Face(11f, FontStyle.Bold);

        void Text(string s, Font f, Color c, Rectangle r, TextFormatFlags fl)
        {
            if (g is not null) TextRenderer.DrawText(g, s, f, r, c, fl);
        }

        void Centered(string s, Font f, Color c, int h)
        {
            if (string.IsNullOrWhiteSpace(s)) return;
            Text(s, f, c, new Rectangle(10, y, w - 20, h), Tf.Center);
            y += h;
        }

        void Divider()
        {
            y += 4;
            if (g is not null)
            {
                using var pen = new Pen(Color.FromArgb(197, 207, 222)) { DashStyle = DashStyle.Dash };
                g.DrawLine(pen, 14, y, w - 14, y);
            }
            y += 8;
        }

        void Pair(string label, string value, Font valueFont, int h = 18, Font? labelFont = null)
        {
            Text(label, labelFont ?? tiny, labelFont is null ? Theme.Muted : Theme.Text, new Rectangle(16, y, w / 3 - 8, h), Tf.Left);
            Text(value, valueFont, Theme.Text, new Rectangle(w / 3, y, w - w / 3 - 16, h), Tf.Right);
            y += h;
        }

        // logo + store name
        int nameW = TextRenderer.MeasureText(d.StoreName, nameFont).Width;
        int x0 = (w - (22 + 6 + nameW)) / 2;
        if (g is not null) PawIcon.Draw(g, x0, y + 1, 22, Theme.Primary);
        Text(d.StoreName, nameFont, Theme.Text, new Rectangle(x0 + 28, y, nameW + 6, 24), Tf.Left);
        y += 28;

        Centered(d.Tagline, tiny, Theme.Muted, 15);
        Centered(d.Address, tiny, Theme.Muted, 15);
        Centered(d.Phone, tiny, Theme.Muted, 15);
        y += 2;
        Divider();
        Centered("OFFICIAL RECEIPT", tinyBold, Theme.Text, 18);
        Divider();

        Pair("Receipt No.:", d.ReceiptNumber, bold);
        Pair("Date:", d.When.ToString("MMM d, yyyy h:mm tt", CultureInfo.CurrentCulture), bold);
        if (!string.IsNullOrWhiteSpace(d.Cashier)) Pair("Cashier:", d.Cashier, bold);
        Pair("Customer:", d.Customer, bold);
        Divider();

        foreach (var l in d.Lines)
        {
            Text(l.Name, bold, Theme.Text, new Rectangle(16, y, w - 32, 18), Tf.Left);
            y += 18;
            if (l.Detail.Length > 0)
            {
                Text(l.Detail, tiny, Theme.Muted, new Rectangle(16, y, w - 32, 14), Tf.Left);
                y += 14;
            }
            Text($"{l.Quantity} x {Money.Format(l.UnitPrice)}", tiny, Theme.TextSoft, new Rectangle(24, y, w / 2, 16), Tf.Left);
            Text(Money.Format(l.Subtotal), bold, Theme.Text, new Rectangle(w / 2, y, w / 2 - 16, 16), Tf.Right);
            y += 22;
        }
        Divider();

        Pair("Subtotal", Money.Format(d.Subtotal), bold);
        if (d.Discount > 0) Pair("Discount", "-" + Money.Format(d.Discount), bold);
        Pair("TOTAL", Money.Format(d.Total), Theme.Face(11f, FontStyle.Bold), 26, tinyBold);
        Divider();

        Pair("Paid by", d.PaymentMethod, bold);
        if (d.AmountReceived is decimal received) Pair("Amount received", Money.Format(received), bold);
        if (d.Change is decimal change) Pair("Change", Money.Format(change), bold);

        if (!string.IsNullOrWhiteSpace(d.Notes))
        {
            Divider();
            Text("Notes: " + d.Notes, tiny, Theme.TextSoft, new Rectangle(16, y, w - 32, 30),
                TextFormatFlags.Left | TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
            y += 32;
        }

        Divider();
        Centered("Thank you!", bold, Theme.Text, 18);
        return y + 14;
    }
}

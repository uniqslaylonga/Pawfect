using System.Drawing.Drawing2D;
using System.Globalization;

namespace MyApp.Controls;

/// <summary>Rounded white card showing a saved sale: who, how it was paid, every item and the totals.</summary>
internal sealed class SaleSummaryView : Control
{
    private ReceiptData? _data;
    private string _message = "";

    public SaleSummaryView()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Theme.Bg;
        Size = new Size(420, 120);
        TabStop = false;
    }

    public ReceiptData? Data
    {
        get => _data;
        set { _data = value; Height = Render(null); Invalidate(); }
    }

    /// <summary>Shown instead of the sale while <see cref="Data"/> is empty (for example when it couldn't be loaded).</summary>
    public string Message
    {
        get => _message;
        set { _message = value; Invalidate(); }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var path = Ui.RoundRect(new RectangleF(0.5f, 0.5f, Width - 2, Height - 2), 10))
        using (var fill = new SolidBrush(Color.White))
        using (var pen = new Pen(Theme.Border))
        {
            g.FillPath(fill, path);
            g.DrawPath(pen, path);
        }

        Render(g);
    }

    private int Render(Graphics? g)
    {
        const int pad = 18;
        int w = Width;
        int y = 14;
        int inner = w - 2 * pad;
        var norm = Theme.Face(8.5f);
        var bold = Theme.Face(8.5f, FontStyle.Bold);
        var small = Theme.Face(7.5f);
        var head = Theme.Face(7.5f, FontStyle.Bold);

        void Text(string s, Font f, Color c, Rectangle r, TextFormatFlags fl)
        {
            if (g is not null) TextRenderer.DrawText(g, s, f, r, c, fl);
        }

        void Line(int yy)
        {
            if (g is null) return;
            using var pen = new Pen(Theme.Border);
            g.DrawLine(pen, pad, yy, w - pad, yy);
        }

        var d = _data;
        if (d is null)
        {
            Text(_message, norm, Theme.Muted, new Rectangle(pad, 40, inner, 40), Tf.Center);
            return 120;
        }

        void Info(string label, string value)
        {
            Text(label, norm, Theme.Muted, new Rectangle(pad, y, 100, 24), Tf.Left);
            Text(value, bold, Theme.Text, new Rectangle(pad + 100, y, inner - 100, 24), Tf.Right);
            y += 24;
        }

        Info("Customer", d.Customer);
        if (!string.IsNullOrWhiteSpace(d.Cashier)) Info("Cashier", d.Cashier);
        Info("Paid by", d.PaymentMethod);
        y += 8;
        Line(y);
        y += 12;

        // column header band
        int itemW = (int)(inner * 0.44);
        const int qtyW = 40;
        int priceW = (int)(inner * 0.22);
        int qtyX = pad + itemW;
        int priceX = qtyX + qtyW;
        int amountX = priceX + priceW;
        int amountW = pad + inner - amountX;

        if (g is not null)
        {
            using var band = new SolidBrush(Theme.Subtle);
            using var path = Ui.RoundRect(new RectangleF(pad, y, inner, 26), 6);
            g.FillPath(band, path);
        }
        Text("ITEM / SERVICE", head, Theme.Muted, new Rectangle(pad + 8, y, itemW - 8, 26), Tf.Left);
        Text("QTY", head, Theme.Muted, new Rectangle(qtyX, y, qtyW, 26), Tf.Center);
        Text("PRICE", head, Theme.Muted, new Rectangle(priceX, y, priceW - 8, 26), Tf.Right);
        Text("AMOUNT", head, Theme.Muted, new Rectangle(amountX, y, amountW - 8, 26), Tf.Right);
        y += 30;

        foreach (var l in d.Lines)
        {
            bool hasDetail = l.Detail.Length > 0;
            Text(l.Name, bold, Theme.Text, new Rectangle(pad + 8, hasDetail ? y + 5 : y + 13, itemW - 8, 18), Tf.Left);
            if (hasDetail)
                Text(l.Detail, small, Theme.Muted, new Rectangle(pad + 8, y + 23, itemW - 8, 16), Tf.Left);
            Text(l.Quantity.ToString(CultureInfo.CurrentCulture), norm, Theme.TextSoft, new Rectangle(qtyX, y, qtyW, 44), Tf.Center);
            Text(Money.Format(l.UnitPrice), norm, Theme.TextSoft, new Rectangle(priceX, y, priceW - 8, 44), Tf.Right);
            Text(Money.Format(l.Subtotal), bold, Theme.Text, new Rectangle(amountX, y, amountW - 8, 44), Tf.Right);
            y += 44;
            Line(y);
        }

        if (d.Lines.Count == 0)
        {
            Text("No items were saved with this sale.", norm, Theme.Muted, new Rectangle(pad, y, inner, 40), Tf.Center);
            y += 40;
            Line(y);
        }

        y += 10;

        void Pair(string label, string value, Font valueFont, Color valueColor, int h = 22)
        {
            Text(label, norm, Theme.TextSoft, new Rectangle(pad + 8, y, inner / 2, h), Tf.Left);
            Text(value, valueFont, valueColor, new Rectangle(pad + inner / 2, y, inner / 2 - 8, h), Tf.Right);
            y += h;
        }

        Pair("Subtotal", Money.Format(d.Subtotal), bold, Theme.Text);
        if (d.Discount > 0) Pair("Discount", "-" + Money.Format(d.Discount), bold, Theme.Text);

        y += 4;
        Text("Total Amount", bold, Theme.Text, new Rectangle(pad + 8, y, inner / 2, 30), Tf.Left);
        Text(Money.Format(d.Total), Theme.Face(13f, FontStyle.Bold), Theme.Primary,
            new Rectangle(pad + inner / 2, y, inner / 2 - 8, 30), Tf.Right);
        y += 32;

        if (d.AmountReceived is decimal received) Pair("Amount received", Money.Format(received), bold, Theme.Text);
        if (d.Change is decimal change) Pair("Change", Money.Format(change), bold, Theme.Green);

        if (!string.IsNullOrWhiteSpace(d.Notes))
        {
            y += 6;
            Line(y);
            y += 8;
            Text("Notes", head, Theme.Muted, new Rectangle(pad + 8, y, inner, 16), Tf.Left);
            y += 16;
            Text(d.Notes, norm, Theme.TextSoft, new Rectangle(pad + 8, y, inner - 16, 34),
                TextFormatFlags.Left | TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
            y += 36;
        }

        return y + 16;
    }
}

/// <summary>Column titles for the transaction list; widths match <see cref="TransactionRowView"/>.</summary>
internal sealed class TransactionColumnsHeader : Control
{
    private static readonly string[] Titles =
    {
        "Receipt No.", "Date & Time", "Customer", "Items / Services", "Total Amount", "Payment Method", "Status", "Actions",
    };

    public TransactionColumnsHeader()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Theme.Subtle;
        Height = 34;
        TabStop = false;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(Theme.Subtle);

        float x = 0;
        for (int i = 0; i < Titles.Length; i++)
        {
            float colW = Width * TransactionRowView.ColumnPercents[i] / 100f;
            var rect = new Rectangle((int)x + 4, 0, Math.Max(0, (int)colW - 8), Height);
            TextRenderer.DrawText(g, Titles[i], Theme.Face(8.5f, FontStyle.Bold), rect, Theme.TextSoft,
                i >= 5 ? Tf.Center : Tf.Left);
            x += colW;
        }

        using var pen = new Pen(Theme.Border);
        g.DrawLine(pen, 0, Height - 1, Width, Height - 1);
    }
}

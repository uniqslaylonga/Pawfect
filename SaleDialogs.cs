using System.ComponentModel;
using System.Globalization;
using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>The full receipt on screen, with a Print button. Opens after a sale and from the transaction details.</summary>
internal sealed class ReceiptForm : Form
{
    private readonly ReceiptData _data;
    private readonly ReceiptPaper _paper;
    private readonly Panel _body;

    public ReceiptForm(ReceiptData data)
    {
        Icon = AppIcon.Value;
        _data = data;

        Text = "Receipt " + data.ReceiptNumber;
        Font = Theme.Face(9f);
        BackColor = Theme.Bg;
        ForeColor = Theme.Text;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        KeyPreview = true;

        _paper = new ReceiptPaper { Data = data };
        _body = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Theme.Bg };
        _body.Controls.Add(_paper);
        _body.Resize += (_, _) => CenterPaper();

        var header = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.White };
        var badge = Ui.Glyph("\uE749", 12f, Theme.Primary, Theme.PrimarySoft);
        badge.SetBounds(18, 12, 36, 36);
        var title = Ui.Text("Receipt", 11f, Theme.Text, Color.White, FontStyle.Bold);
        title.SetBounds(64, 10, 250, 22);
        var number = Ui.Text(data.ReceiptNumber, 8.5f, Theme.Muted, Color.White);
        number.SetBounds(64, 32, 250, 18);
        header.Controls.AddRange(new Control[] { badge, title, number });
        header.Paint += (_, e) =>
        {
            using var pen = new Pen(Theme.Border);
            e.Graphics.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
        };

        var pdf = new OutlineButton { Kind = ButtonKind.Primary, Glyph = "\uE74E", Text = "Save as PDF", Size = new Size(140, 38) };
        var print = new OutlineButton { Glyph = "\uE749", Text = "Print", Size = new Size(100, 38) };
        var close = new OutlineButton { Text = "Close", Size = new Size(90, 38) };
        pdf.Click += (_, _) => SavePdfNow();
        print.Click += (_, _) => PrintNow();
        close.Click += (_, _) => Close();

        var bar = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 62,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(16, 12, 16, 8),
            BackColor = Color.White,
        };
        bar.Controls.Add(pdf);     // first added = right-most
        bar.Controls.Add(print);
        bar.Controls.Add(close);
        bar.Paint += (_, e) =>
        {
            using var pen = new Pen(Theme.Border);
            e.Graphics.DrawLine(pen, 0, 0, bar.Width, 0);
        };

        Controls.Add(_body);
        Controls.Add(header);
        Controls.Add(bar);

        ClientSize = new Size(Math.Max(ReceiptPaper.PaperWidth + 32 + SystemInformation.VerticalScrollBarWidth, 372),
            Math.Clamp(_paper.Height + 32 + 60 + 62, 440, 720));

        Load += (_, _) => CenterPaper();
        KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Escape) Close();
            else if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; SavePdfNow(); }
        };
    }

    private void CenterPaper()
    {
        _paper.Left = Math.Max(16, (_body.ClientSize.Width - _paper.Width) / 2);
        _paper.Top = 16;
    }

    /// <summary>Saves the receipt as a PDF document file and opens it.</summary>
    private void SavePdfNow()
    {
        try
        {
            ReceiptPrinter.SaveAsPdf(this, _data);
        }
        catch (Exception ex) when (ex is Win32Exception or InvalidOperationException or IOException or UnauthorizedAccessException)
        {
            Msg.Show(this, "The receipt could not be saved as a PDF.\n\n" + ex.Message, "Pawfect",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void PrintNow()
    {
        try
        {
            ReceiptPrinter.Print(this, _data);
        }
        catch (Exception ex) when (ex is Win32Exception or InvalidOperationException)
        {
            Msg.Show(this, "The receipt could not be printed.\n\n" + ex.Message, "Pawfect",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}

/// <summary>A saved sale with its items and totals. From here the receipt can be viewed or printed, and the sale refunded or voided (receipts are never deleted).</summary>
internal sealed class TransactionDetailsForm : Form
{
    private readonly TransactionItem _item;
    private readonly ReceiptData? _receipt;
    private readonly SaleSummaryView _summary = new();
    private readonly Label _error;

    /// <summary>Set to Refunded or Voided when the sale was changed.</summary>
    public TransactionStatus? NewStatus { get; private set; }

    public TransactionDetailsForm(TransactionItem item)
    {
        Icon = AppIcon.Value;
        _item = item;

        Text = "Transaction " + item.ReceiptNumber;
        Font = Theme.Face(9f);
        BackColor = Theme.Bg;
        ForeColor = Theme.Text;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        KeyPreview = true;
        ClientSize = new Size(480, 620);

        try { _receipt = EmployeeData.GetReceipt(item.Id); }
        catch (SqlException) { _receipt = null; }

        if (_receipt is null) _summary.Message = "This sale couldn't be loaded from the database.";
        else _summary.Data = _receipt;

        // header: receipt number, date and status pill
        var header = new Panel { Dock = DockStyle.Top, Height = 72, BackColor = Color.White };
        header.Paint += (_, e) => PaintHeader(e.Graphics, header);

        // body
        var body = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Theme.Bg };
        _summary.Location = new Point(16, 16);
        _summary.Width = ClientSize.Width - 32 - SystemInformation.VerticalScrollBarWidth;
        body.Controls.Add(_summary);

        _error = Ui.Text("", 8.5f, Theme.Danger, Color.White);
        _error.Dock = DockStyle.Bottom;
        _error.Height = 22;
        _error.Padding = new Padding(20, 0, 20, 0);

        // buttons
        var close = new OutlineButton { Text = "Close", Size = new Size(80, 38) };
        var viewReceipt = new OutlineButton { Kind = ButtonKind.Primary, Glyph = "\uE749", Text = "View Receipt", Size = new Size(140, 38), Enabled = _receipt is not null };
        var voidSale = new OutlineButton { Kind = ButtonKind.Danger, Text = "Void sale", Size = new Size(96, 38) };
        var refund = new OutlineButton { Kind = ButtonKind.Soft, Text = "Refund", Size = new Size(84, 38) };

        bool open = item.Status == TransactionStatus.Completed;
        voidSale.Enabled = open;
        refund.Enabled = open;

        close.Click += (_, _) => Close();
        viewReceipt.Click += (_, _) =>
        {
            if (_receipt is null) return;
            using var form = new ReceiptForm(_receipt);
            form.ShowDialog(this);
        };
        voidSale.Click += (_, _) => ChangeTo(TransactionStatus.Voided);
        refund.Click += (_, _) => ChangeTo(TransactionStatus.Refunded);

        var bar = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 62,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(16, 12, 16, 8),
            BackColor = Color.White,
        };
        bar.Controls.Add(close);
        bar.Controls.Add(viewReceipt);
        bar.Controls.Add(voidSale);
        bar.Controls.Add(refund);
        bar.Paint += (_, e) =>
        {
            using var pen = new Pen(Theme.Border);
            e.Graphics.DrawLine(pen, 0, 0, bar.Width, 0);
        };

        Controls.Add(body);
        Controls.Add(header);
        Controls.Add(_error);
        Controls.Add(bar);

        KeyDown += (_, e) => { if (e.KeyCode == Keys.Escape) Close(); };
    }

    private void PaintHeader(Graphics g, Control header)
    {
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        using (var b = new SolidBrush(Theme.PrimarySoft))
            g.FillEllipse(b, 18, 16, 40, 40);
        TextRenderer.DrawText(g, "\uE719", Theme.GlyphFont(12f), new Rectangle(18, 16, 40, 40), Theme.Primary, Tf.Center);

        TextRenderer.DrawText(g, _item.ReceiptNumber, Theme.Face(11f, FontStyle.Bold),
            new Rectangle(68, 14, header.Width - 68 - 130, 24), Theme.Text, Tf.Left);
        TextRenderer.DrawText(g, _item.When.ToString("MMM d, yyyy  h:mm tt", CultureInfo.CurrentCulture), Theme.Face(8.5f),
            new Rectangle(68, 38, header.Width - 68 - 130, 18), Theme.Muted, Tf.Left);

        var (fore, fill, border) = TransactionStatusStyle.Pill(_item.Status);
        string label = TransactionStatusStyle.Label(_item.Status);
        int pw = TextRenderer.MeasureText(label, Theme.Face(7.5f, FontStyle.Bold)).Width + 18;
        Pills.DrawAt(g, label, fore, fill, border, header.Width - 20 - pw, 36, pw + 4, 7.5f, 22);

        using var pen = new Pen(Theme.Border);
        g.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
    }

    private void ChangeTo(TransactionStatus status)
    {
        string verb = status == TransactionStatus.Voided ? "Void" : "Refund";
        if (Msg.Show(this, $"{verb} receipt {_item.ReceiptNumber} ({Money.Format(_item.Total)})?\n\nThis can't be undone.",
                "Pawfect", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        try
        {
            if (!EmployeeData.ChangeSaleStatus(_item.Id, status))
            {
                _error.Text = "Only completed sales can be voided or refunded.";
                return;
            }
            NewStatus = status;
            DialogResult = DialogResult.OK;
        }
        catch (SqlException ex)
        {
            Msg.Show(this, "The database could not be updated.\n\n" + ex.Message, "Pawfect",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

/// <summary>Every sale, searchable, in the same table style as "Recent Transactions". "View" opens the sale.</summary>
internal sealed class TransactionHistoryForm : Form
{
    private const int MaxRows = 100;

    private readonly InputField _search = new() { Glyph = "\uE721", Placeholder = "Search receipt or customer...", Size = new Size(300, 40), CornerColor = Color.White };
    private readonly Panel _rowsHost = new() { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White };
    private readonly EmptyState _empty = new() { Dock = DockStyle.Fill, Glyph = "\uE81C", Visible = false };
    private readonly Label _count;
    private List<TransactionItem> _all = new();

    /// <summary>True when a sale was refunded or voided while this window was open.</summary>
    public bool Changed { get; private set; }

    public TransactionHistoryForm()
    {
        Icon = AppIcon.Value;
        Text = "Transaction History";
        Font = Theme.Face(9f);
        BackColor = Theme.Bg;
        ForeColor = Theme.Text;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(940, 580);
        MinimumSize = new Size(780, 440);
        ShowInTaskbar = false;
        KeyPreview = true;

        // header
        var header = new Panel { Dock = DockStyle.Top, Height = 76, BackColor = Color.White };
        var title = Ui.Text("Transaction History", 14f, Theme.Text, Color.White, FontStyle.Bold);
        title.SetBounds(20, 12, 460, 28);
        var sub = Ui.Text("Every sale. Press View to see the receipt, print it, refund or void it.", 8.5f, Theme.Muted, Color.White);
        sub.SetBounds(20, 42, 520, 20);
        _search.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _search.Location = new Point(ClientSize.Width - 20 - _search.Width, 18);
        header.Controls.AddRange(new Control[] { title, sub, _search });
        header.Paint += (_, e) =>
        {
            using var pen = new Pen(Theme.Border);
            e.Graphics.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
        };

        // table card
        var card = new CardPanel { Dock = DockStyle.Fill, FillColor = Color.White, CornerColor = Theme.Bg, Padding = new Padding(1) };
        var columns = new TransactionColumnsHeader { Dock = DockStyle.Top };
        card.Controls.Add(_rowsHost);
        card.Controls.Add(_empty);
        card.Controls.Add(columns);

        var cardHost = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16), BackColor = Theme.Bg };
        cardHost.Controls.Add(card);

        // footer
        _count = Ui.Text("", 8.5f, Theme.Muted, Color.White);
        _count.SetBounds(20, 0, 600, 54);
        var close = new OutlineButton { Text = "Close", Size = new Size(96, 36), Anchor = AnchorStyles.Top | AnchorStyles.Right };
        close.Location = new Point(ClientSize.Width - 20 - close.Width, 9);
        close.Click += (_, _) => Close();
        var footer = new Panel { Dock = DockStyle.Bottom, Height = 54, BackColor = Color.White };
        footer.Controls.Add(_count);
        footer.Controls.Add(close);
        footer.Paint += (_, e) =>
        {
            using var pen = new Pen(Theme.Border);
            e.Graphics.DrawLine(pen, 0, 0, footer.Width, 0);
        };

        Controls.Add(cardHost);
        Controls.Add(header);
        Controls.Add(footer);

        _search.Box.TextChanged += (_, _) => Fill();
        KeyDown += (_, e) => { if (e.KeyCode == Keys.Escape) Close(); };

        Reload();
    }

    private void Reload()
    {
        _all = EmployeeData.GetRecentTransactions(1000);
        Fill();
    }

    private void Fill()
    {
        string q = _search.Value.Trim();
        var matches = _all.Where(x => q.Length == 0
                || x.ReceiptNumber.Contains(q, StringComparison.CurrentCultureIgnoreCase)
                || x.CustomerName.Contains(q, StringComparison.CurrentCultureIgnoreCase))
            .ToList();
        var shown = matches.Take(MaxRows).ToList();

        _rowsHost.SuspendLayout();
        foreach (var old in _rowsHost.Controls.OfType<TransactionRowView>().ToList())
        {
            _rowsHost.Controls.Remove(old);
            old.Dispose();
        }

        // Rows are docked to the top, so add them last-to-first to keep the first one on top.
        for (int i = shown.Count - 1; i >= 0; i--)
        {
            var item = shown[i];
            var row = new TransactionRowView(item) { Dock = DockStyle.Top };
            row.ViewClicked += (_, _) => BeginInvoke(() => OpenSale(item));   // let the click finish before rows are rebuilt
            _rowsHost.Controls.Add(row);
        }

        _empty.Title = _all.Count == 0 ? "No transactions yet" : "No matching transactions";
        _empty.Hint = _all.Count == 0
            ? "Sales will show up here once they are billed."
            : "Try a different receipt number or customer name.";
        _empty.Visible = shown.Count == 0;
        _rowsHost.Visible = shown.Count > 0;

        _count.Text = matches.Count > MaxRows
            ? $"Showing the latest {MaxRows} of {matches.Count} transactions. Search to narrow the list."
            : matches.Count == 1 ? "1 transaction" : $"{matches.Count} transactions";

        _rowsHost.AutoScrollPosition = Point.Empty;
        _rowsHost.ResumeLayout(true);
    }

    private void OpenSale(TransactionItem item)
    {
        using var form = new TransactionDetailsForm(item);
        if (form.ShowDialog(this) != DialogResult.OK) return;

        Changed = true;
        try { Reload(); }
        catch (SqlException ex)
        {
            Msg.Show(this, "Couldn't reload the list.\n\n" + ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

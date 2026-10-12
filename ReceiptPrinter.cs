using System.Diagnostics;
using System.Drawing.Printing;
using System.Globalization;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Prints a receipt on the default printer (a thermal roll or a normal page), or saves it as a PDF document file
/// that opens right away. If the PC has no usable printer, a print preview opens instead.
/// </summary>
internal static class ReceiptPrinter
{
    private const float ReceiptWidth = 280f;   // 1/100 inch (about 71 mm), fits an 80 mm roll

    private const string PdfPrinter = "Microsoft Print to PDF";   // built into Windows 10 / 11

    /// <summary>Builds the print job for one receipt. <paramref name="center"/> puts the receipt in the middle of the page (used for the PDF).</summary>
    private static PrintDocument BuildDocument(ReceiptData data, bool center)
    {
        var doc = new PrintDocument { DocumentName = data.ReceiptNumber };
        doc.DefaultPageSettings.Margins = center ? new Margins(50, 50, 50, 50) : new Margins(20, 20, 20, 20);

        int next = 0;
        int page = 0;
        bool linesDone = false;

        doc.BeginPrint += (_, _) => { next = 0; page = 0; linesDone = false; };
        doc.PrintPage += (_, e) => DrawPage(e, data, ref next, ref page, ref linesDone, center);
        return doc;
    }

    public static void Print(IWin32Window? owner, ReceiptData data)
    {
        using var doc = BuildDocument(data, center: false);

        bool hasPrinter;
        try { hasPrinter = doc.PrinterSettings.IsValid; }
        catch (InvalidPrinterException) { hasPrinter = false; }

        if (hasPrinter)
        {
            try
            {
                doc.Print();
                return;
            }
            catch (InvalidPrinterException) { /* fall through to the preview */ }
        }

        using var preview = new PrintPreviewDialog { Document = doc, Width = 520, Height = 760 };
        if (owner is null) preview.ShowDialog(); else preview.ShowDialog(owner);
    }

    /// <summary>
    /// Saves the receipt as a PDF document: asks where to save it, writes the file and opens it.
    /// Returns the saved path, or null if the person cancelled.
    /// </summary>
    public static string? SaveAsPdf(IWin32Window? owner, ReceiptData data)
    {
        bool hasPdfPrinter = false;
        foreach (string name in PrinterSettings.InstalledPrinters)
            if (string.Equals(name, PdfPrinter, StringComparison.OrdinalIgnoreCase)) { hasPdfPrinter = true; break; }

        if (!hasPdfPrinter)
        {
            MessageBox.Show(owner, "This PC doesn't have the \"Microsoft Print to PDF\" printer, so the receipt can't be saved as a PDF.\n\n" +
                "Turn it on under Windows Features, or use Print instead.", "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;
        }

        string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Pawfect Receipts");
        Directory.CreateDirectory(folder);

        string safe = string.Concat(data.ReceiptNumber.Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '-' : c));
        using var dialog = new SaveFileDialog
        {
            Title = "Save receipt as a document",
            Filter = "PDF document (*.pdf)|*.pdf",
            DefaultExt = "pdf",
            AddExtension = true,
            InitialDirectory = folder,
            FileName = $"Receipt {safe}.pdf",
            OverwritePrompt = true,
        };
        if ((owner is null ? dialog.ShowDialog() : dialog.ShowDialog(owner)) != DialogResult.OK) return null;

        using (var doc = BuildDocument(data, center: true))
        {
            doc.PrinterSettings.PrinterName = PdfPrinter;
            doc.PrinterSettings.PrintToFile = true;
            doc.PrinterSettings.PrintFileName = dialog.FileName;
            doc.PrintController = new StandardPrintController();   // no "Printing..." pop-up
            doc.Print();
        }

        // Open the finished document so it can be checked, printed or shared from the viewer.
        Process.Start(new ProcessStartInfo(dialog.FileName) { UseShellExecute = true });
        return dialog.FileName;
    }

    private static void DrawPage(PrintPageEventArgs e, ReceiptData d, ref int next, ref int page, ref bool linesDone, bool center)
    {
        var g = e.Graphics!;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        var bounds = e.MarginBounds;
        float w = Math.Min(ReceiptWidth, bounds.Width);
        float x = center ? bounds.Left + (bounds.Width - w) / 2f : bounds.Left;
        float y = bounds.Top;
        float bottom = bounds.Bottom;
        page++;

        using var normal = new Font("Segoe UI", 8f);
        using var small = new Font("Segoe UI", 7f);
        using var bold = new Font("Segoe UI", 8f, FontStyle.Bold);
        using var title = new Font("Segoe UI", 12f, FontStyle.Bold);
        using var big = new Font("Segoe UI", 10f, FontStyle.Bold);
        using var black = new SolidBrush(Color.Black);
        using var grey = new SolidBrush(Color.FromArgb(90, 90, 90));
        using var dashed = new Pen(Color.Black) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
        using var centre = new StringFormat { Alignment = StringAlignment.Center };
        using var right = new StringFormat { Alignment = StringAlignment.Far };
        using var left = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };

        // Locals can't capture ref parameters, so copy y into a field-like holder used by the helpers.
        float cursor = y;

        void Centered(string text, Font font, Brush brush)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            float h = font.GetHeight(g);
            g.DrawString(text, font, brush, new RectangleF(x, cursor, w, h), centre);
            cursor += h + 2;
        }

        void Divider()
        {
            cursor += 3;
            g.DrawLine(dashed, x, cursor, x + w, cursor);
            cursor += 5;
        }

        void Pair(string label, string value, Font font)
        {
            float h = font.GetHeight(g);
            g.DrawString(label, font, black, new RectangleF(x, cursor, w * 0.55f, h), left);
            g.DrawString(value, font, black, new RectangleF(x, cursor, w, h), right);
            cursor += h + 2;
        }

        if (page == 1)
        {
            // paw logo + store name
            PawIcon.Draw(g, x + (w - 30f) / 2f, cursor, 30f, Color.Black);
            cursor += 34;
            Centered(d.StoreName, title, black);
            Centered(d.Tagline, small, grey);
            Centered(d.Address, small, grey);
            Centered(d.Phone, small, grey);
            Divider();
            Centered("OFFICIAL RECEIPT", bold, black);
            Divider();
            Pair("Receipt No.:", d.ReceiptNumber, normal);
            Pair("Date:", d.When.ToString("MMM d, yyyy h:mm tt", CultureInfo.CurrentCulture), normal);
            if (!string.IsNullOrWhiteSpace(d.Cashier)) Pair("Cashier:", d.Cashier, normal);
            Pair("Customer:", d.Customer, normal);
            Divider();
        }

        var lines = d.Lines;
        while (next < lines.Count)
        {
            var line = lines[next];
            float rowH = bold.GetHeight(g) + normal.GetHeight(g) + 6 + (line.Detail.Length > 0 ? small.GetHeight(g) : 0);
            if (cursor + rowH > bottom - 10)
            {
                e.HasMorePages = true;
                return;
            }

            g.DrawString(line.Name, bold, black, new RectangleF(x, cursor, w, bold.GetHeight(g)), left);
            cursor += bold.GetHeight(g);
            if (line.Detail.Length > 0)
            {
                g.DrawString(line.Detail, small, grey, new RectangleF(x, cursor, w, small.GetHeight(g)), left);
                cursor += small.GetHeight(g);
            }

            g.DrawString($"{line.Quantity} x {Money.Format(line.UnitPrice)}", normal, black,
                new RectangleF(x + 8, cursor, w * 0.6f, normal.GetHeight(g)), left);
            g.DrawString(Money.Format(line.Subtotal), normal, black, new RectangleF(x, cursor, w, normal.GetHeight(g)), right);
            cursor += normal.GetHeight(g) + 4;
            next++;
        }

        // The totals block is kept together; if it doesn't fit it moves to the next page.
        float totalsH = 8 * (normal.GetHeight(g) + 2) + big.GetHeight(g) + 40;
        if (!linesDone && cursor + totalsH > bottom)
        {
            linesDone = true;
            e.HasMorePages = true;
            return;
        }

        Divider();
        Pair("Subtotal", Money.Format(d.Subtotal), normal);
        if (d.Discount > 0) Pair("Discount", "-" + Money.Format(d.Discount), normal);
        Pair("TOTAL", Money.Format(d.Total), big);
        Divider();
        Pair("Paid by", d.PaymentMethod, normal);
        if (d.AmountReceived is decimal received) Pair("Amount received", Money.Format(received), normal);
        if (d.Change is decimal change) Pair("Change", Money.Format(change), bold);

        if (!string.IsNullOrWhiteSpace(d.Notes))
        {
            Divider();
            float h = small.GetHeight(g) * 3;
            g.DrawString("Notes: " + d.Notes, small, grey, new RectangleF(x, cursor, w, h));
            cursor += h;
        }

        Divider();
        Centered("Thank you!", bold, black);
        e.HasMorePages = false;
    }
}

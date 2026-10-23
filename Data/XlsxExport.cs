using System.Globalization;
using System.IO.Compression;
using System.Text;

namespace MyApp;

/// <summary>
/// Writes the Reports &amp; Financial Auditing table as a real Excel workbook (.xlsx): bold blue header, frozen header row,
/// filter buttons, real dates and numbers (so Excel can sort and add them up), peso amounts, and coloured status cells.
/// It builds the file by hand with System.IO.Compression, so no extra NuGet package is needed.
/// </summary>
internal static class XlsxExport
{
    // cellXfs indexes in the stylesheet below
    private const int TitleCell = 1, SubtitleCell = 2, HeaderCell = 3, TextCell = 4, DateTimeCell = 5, DateCell = 6, IntCell = 7,
        PesoCell = 8, LabelCell = 9, PesoBold = 10, StatusGood = 11, StatusWarn = 12, StatusBad = 13, StatusRefund = 14, StatusInfo = 15;

    private const int HeaderRow = 9;

    public static void WriteAudit(string path, AuditTab tab, IReadOnlyList<AuditRow> rows, AuditTotals totals,
        string period, string preparedBy)
    {
        var inv = CultureInfo.InvariantCulture;
        var sheet = new StringBuilder(32 * 1024);

        string[] h = ReportsData.Headers(tab);
        string amountHeader = tab == AuditTab.Summary ? "NET SALES (PHP)" : tab == AuditTab.Movement ? "VALUE (PHP)" : "AMOUNT (PHP)";
        string[] headers = { h[0], h[1], h[2], h[3], amountHeader, h[5], h[6], "NOTES" };

        int lastRow = HeaderRow + Math.Max(rows.Count, 1);

        sheet.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
        sheet.Append("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">");
        sheet.Append("<sheetPr><pageSetUpPr fitToPage=\"1\"/></sheetPr>");
        sheet.Append("<sheetViews><sheetView workbookViewId=\"0\" showGridLines=\"0\">");
        sheet.Append($"<pane ySplit=\"{HeaderRow}\" topLeftCell=\"A{HeaderRow + 1}\" activePane=\"bottomLeft\" state=\"frozen\"/>");
        sheet.Append("</sheetView></sheetViews>");
        sheet.Append("<sheetFormatPr defaultRowHeight=\"18\"/>");
        sheet.Append("<cols>");
        double[] widths = { 21, 17, 38, 11, 17, 24, 15, 70 };
        for (int i = 0; i < widths.Length; i++)
            sheet.Append($"<col min=\"{i + 1}\" max=\"{i + 1}\" width=\"{widths[i].ToString(inv)}\" customWidth=\"1\"/>");
        sheet.Append("</cols>");
        sheet.Append("<sheetData>");

        // title block
        Row(sheet, 1, 26, Str("A", 1, "Pawfect  -  " + ReportsData.Title(tab), TitleCell));
        Row(sheet, 2, 18, Str("A", 2, $"Period: {period}   |   Generated {DateTime.Now.ToString("MMM d, yyyy h:mm tt", CultureInfo.CurrentCulture)}   |   Prepared by {preparedBy}", SubtitleCell));

        // summary block (the totals of the whole period shown on the cards)
        Row(sheet, 4, 18, Str("A", 4, "Total Sales (Audited)", LabelCell), Num("B", 4, totals.Sales, PesoBold, inv));
        Row(sheet, 5, 18, Str("A", 5, "Inventory Movement (items)", LabelCell), Num("B", 5, totals.Units, IntCell, inv));
        Row(sheet, 6, 18, Str("A", 6, "Discrepancies Found", LabelCell), Num("B", 6, totals.Discrepancies, IntCell, inv));
        Row(sheet, 7, 18, Str("A", 7, "Rows in this report", LabelCell), Num("B", 7, rows.Count, IntCell, inv));

        // table header
        var head = new StringBuilder();
        for (int c = 0; c < headers.Length; c++) head.Append(Str(Col(c), HeaderRow, headers[c], HeaderCell));
        Row(sheet, HeaderRow, 24, head.ToString());

        // table body
        for (int i = 0; i < rows.Count; i++)
        {
            var r = rows[i];
            int n = HeaderRow + 1 + i;
            bool dateOnly = tab == AuditTab.Summary || r.When.TimeOfDay == TimeSpan.Zero;

            var cells = new StringBuilder();
            cells.Append(Num("A", n, (decimal)r.When.ToOADate(), dateOnly ? DateCell : DateTimeCell, inv));
            cells.Append(Str("B", n, r.Type, TextCell));
            cells.Append(Str("C", n, r.Item, TextCell));
            cells.Append(r.QuantityValue is int q ? Num("D", n, q, IntCell, inv) : Blank("D", n, TextCell));
            cells.Append(r.AmountValue is decimal a ? Num("E", n, a, PesoCell, inv) : Blank("E", n, TextCell));
            cells.Append(Str("F", n, r.Reference, TextCell));
            cells.Append(Str("G", n, r.Status, StatusStyle(r.Badge)));
            cells.Append(Str("H", n, r.Note.Replace("\n", "  "), TextCell));
            Row(sheet, n, 20, cells.ToString());
        }

        if (rows.Count == 0)
            Row(sheet, HeaderRow + 1, 20, Str("A", HeaderRow + 1, "No rows for the selected filters.", TextCell));

        sheet.Append("</sheetData>");
        sheet.Append($"<autoFilter ref=\"A{HeaderRow}:H{lastRow}\"/>");
        sheet.Append("<pageMargins left=\"0.5\" right=\"0.5\" top=\"0.6\" bottom=\"0.6\" header=\"0.3\" footer=\"0.3\"/>");
        sheet.Append("<pageSetup orientation=\"landscape\" fitToHeight=\"0\"/>");
        sheet.Append("</worksheet>");

        string sheetName = Escape(ReportsData.Title(tab));

        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        using var zip = new ZipArchive(stream, ZipArchiveMode.Create);

        Add(zip, "[Content_Types].xml",
            "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
            "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">" +
            "<Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>" +
            "<Default Extension=\"xml\" ContentType=\"application/xml\"/>" +
            "<Override PartName=\"/xl/workbook.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml\"/>" +
            "<Override PartName=\"/xl/worksheets/sheet1.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/>" +
            "<Override PartName=\"/xl/styles.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml\"/>" +
            "</Types>");

        Add(zip, "_rels/.rels",
            "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
            "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
            "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\"/>" +
            "</Relationships>");

        Add(zip, "xl/workbook.xml",
            "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
            "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" " +
            "xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">" +
            $"<sheets><sheet name=\"{sheetName}\" sheetId=\"1\" r:id=\"rId1\"/></sheets>" +
            "</workbook>");

        Add(zip, "xl/_rels/workbook.xml.rels",
            "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
            "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
            "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet1.xml\"/>" +
            "<Relationship Id=\"rId2\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles\" Target=\"styles.xml\"/>" +
            "</Relationships>");

        Add(zip, "xl/styles.xml", Styles);
        Add(zip, "xl/worksheets/sheet1.xml", sheet.ToString());
    }

    /// <summary>Audit Trail / Activity Logs as a workbook: one row per entry with a real date/time column and filter buttons.</summary>
    public static void WriteActivityLog(string path, IReadOnlyList<AuditLog.Entry> rows, string period, string preparedBy)
    {
        var inv = CultureInfo.InvariantCulture;
        var sheet = new StringBuilder(32 * 1024);
        string[] headers = { "DATE & TIME", "USER", "ACTION", "MODULE", "DESCRIPTION", "REFERENCE", "STATUS" };
        const int header = 5;
        int lastRow = header + Math.Max(rows.Count, 1);

        sheet.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
        sheet.Append("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">");
        sheet.Append("<sheetPr><pageSetUpPr fitToPage=\"1\"/></sheetPr>");
        sheet.Append("<sheetViews><sheetView workbookViewId=\"0\" showGridLines=\"0\">");
        sheet.Append($"<pane ySplit=\"{header}\" topLeftCell=\"A{header + 1}\" activePane=\"bottomLeft\" state=\"frozen\"/>");
        sheet.Append("</sheetView></sheetViews>");
        sheet.Append("<sheetFormatPr defaultRowHeight=\"18\"/>");
        sheet.Append("<cols>");
        double[] widths = { 21, 24, 24, 16, 60, 20, 12 };
        for (int i = 0; i < widths.Length; i++)
            sheet.Append($"<col min=\"{i + 1}\" max=\"{i + 1}\" width=\"{widths[i].ToString(inv)}\" customWidth=\"1\"/>");
        sheet.Append("</cols>");
        sheet.Append("<sheetData>");

        Row(sheet, 1, 26, Str("A", 1, "Pawfect  -  Activity Logs", TitleCell));
        Row(sheet, 2, 18, Str("A", 2, $"Period: {period}   |   Generated {DateTime.Now.ToString("MMM d, yyyy h:mm tt", CultureInfo.CurrentCulture)}   |   Prepared by {preparedBy}", SubtitleCell));
        Row(sheet, 3, 18, Str("A", 3, "Entries in this report", LabelCell), Num("B", 3, rows.Count, IntCell, inv));

        var head = new StringBuilder();
        for (int c = 0; c < headers.Length; c++) head.Append(Str(Col(c), header, headers[c], HeaderCell));
        Row(sheet, header, 24, head.ToString());

        for (int i = 0; i < rows.Count; i++)
        {
            var r = rows[i];
            int n = header + 1 + i;
            var cells = new StringBuilder();
            cells.Append(Num("A", n, (decimal)r.When.ToOADate(), DateTimeCell, inv));
            cells.Append(Str("B", n, r.Actor, TextCell));
            cells.Append(Str("C", n, r.Action, TextCell));
            cells.Append(Str("D", n, AuditInfo.Module(r.Action), TextCell));
            cells.Append(Str("E", n, r.Details, TextCell));
            cells.Append(Str("F", n, AuditInfo.Reference(r.Details), TextCell));
            cells.Append(Str("G", n, AuditInfo.Status(r.Action), AuditInfo.IsFailed(r.Action) ? StatusBad : StatusGood));
            Row(sheet, n, 20, cells.ToString());
        }
        if (rows.Count == 0)
            Row(sheet, header + 1, 20, Str("A", header + 1, "No entries for the selected filters.", TextCell));

        sheet.Append("</sheetData>");
        sheet.Append($"<autoFilter ref=\"A{header}:G{lastRow}\"/>");
        sheet.Append("<pageMargins left=\"0.5\" right=\"0.5\" top=\"0.6\" bottom=\"0.6\" header=\"0.3\" footer=\"0.3\"/>");
        sheet.Append("<pageSetup orientation=\"landscape\" fitToHeight=\"0\"/>");
        sheet.Append("</worksheet>");

        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        using var zip = new ZipArchive(stream, ZipArchiveMode.Create);
        Add(zip, "[Content_Types].xml",
            "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
            "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">" +
            "<Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>" +
            "<Default Extension=\"xml\" ContentType=\"application/xml\"/>" +
            "<Override PartName=\"/xl/workbook.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml\"/>" +
            "<Override PartName=\"/xl/worksheets/sheet1.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/>" +
            "<Override PartName=\"/xl/styles.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml\"/>" +
            "</Types>");
        Add(zip, "_rels/.rels",
            "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
            "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
            "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\"/>" +
            "</Relationships>");
        Add(zip, "xl/workbook.xml",
            "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
            "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" " +
            "xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">" +
            "<sheets><sheet name=\"Activity Logs\" sheetId=\"1\" r:id=\"rId1\"/></sheets>" +
            "</workbook>");
        Add(zip, "xl/_rels/workbook.xml.rels",
            "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
            "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
            "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet1.xml\"/>" +
            "<Relationship Id=\"rId2\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles\" Target=\"styles.xml\"/>" +
            "</Relationships>");
        Add(zip, "xl/styles.xml", Styles);
        Add(zip, "xl/worksheets/sheet1.xml", sheet.ToString());
    }

    // ------------------------------------------------------------------ cells

    private static int StatusStyle(AuditBadge b) => b switch
    {
        AuditBadge.Good => StatusGood,
        AuditBadge.Warn => StatusWarn,
        AuditBadge.Bad => StatusBad,
        AuditBadge.Refund => StatusRefund,
        _ => StatusInfo,
    };

    /// <summary>Column letter for a zero-based column index (0 = A). The table never goes past Z.</summary>
    private static string Col(int index) => ((char)('A' + index)).ToString();

    private static void Row(StringBuilder sb, int number, int height, string cells)
        => sb.Append($"<row r=\"{number}\" ht=\"{height}\" customHeight=\"1\">{cells}</row>");

    private static void Row(StringBuilder sb, int number, int height, params string[] cells)
        => Row(sb, number, height, string.Concat(cells));

    private static string Str(string col, int row, string text, int style)
        => $"<c r=\"{col}{row}\" s=\"{style}\" t=\"inlineStr\"><is><t xml:space=\"preserve\">{Escape(text)}</t></is></c>";

    private static string Num(string col, int row, decimal value, int style, CultureInfo inv)
        => $"<c r=\"{col}{row}\" s=\"{style}\"><v>{value.ToString(inv)}</v></c>";

    private static string Blank(string col, int row, int style) => $"<c r=\"{col}{row}\" s=\"{style}\"/>";

    /// <summary>XML-escapes text and drops characters Excel refuses to open.</summary>
    private static string Escape(string text)
    {
        var sb = new StringBuilder(text.Length + 8);
        foreach (char ch in text)
        {
            switch (ch)
            {
                case '&': sb.Append("&amp;"); break;
                case '<': sb.Append("&lt;"); break;
                case '>': sb.Append("&gt;"); break;
                case '"': sb.Append("&quot;"); break;
                default:
                    if (ch >= ' ' || ch == '\t' || ch == '\n' || ch == '\r') sb.Append(ch);
                    break;
            }
        }
        return sb.ToString();
    }

    private static void Add(ZipArchive zip, string name, string content)
    {
        var entry = zip.CreateEntry(name, CompressionLevel.Optimal);
        using var writer = new StreamWriter(entry.Open(), new UTF8Encoding(false));
        writer.Write(content);
    }

    // ------------------------------------------------------------------ stylesheet

    private const string Styles =
        "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
        "<styleSheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">" +

        "<numFmts count=\"3\">" +
        "<numFmt numFmtId=\"164\" formatCode=\"mmm\\ d\\,\\ yyyy\\ h:mm\\ AM/PM\"/>" +
        "<numFmt numFmtId=\"165\" formatCode=\"mmm\\ d\\,\\ yyyy\"/>" +
        "<numFmt numFmtId=\"166\" formatCode=\"&quot;\u20B1&quot;#,##0.00;[Red]\\-&quot;\u20B1&quot;#,##0.00\"/>" +
        "</numFmts>" +

        "<fonts count=\"10\">" +
        "<font><sz val=\"10\"/><color rgb=\"FF1F2A3C\"/><name val=\"Segoe UI\"/></font>" +                         // 0 normal
        "<font><b/><sz val=\"15\"/><color rgb=\"FF3A7BD5\"/><name val=\"Segoe UI\"/></font>" +                      // 1 title
        "<font><sz val=\"9\"/><color rgb=\"FF7A889C\"/><name val=\"Segoe UI\"/></font>" +                           // 2 subtitle
        "<font><b/><sz val=\"9\"/><color rgb=\"FFFFFFFF\"/><name val=\"Segoe UI\"/></font>" +                       // 3 header
        "<font><b/><sz val=\"10\"/><color rgb=\"FF1F2A3C\"/><name val=\"Segoe UI\"/></font>" +                      // 4 bold
        "<font><b/><sz val=\"10\"/><color rgb=\"FF2E9E5B\"/><name val=\"Segoe UI\"/></font>" +                      // 5 green
        "<font><b/><sz val=\"10\"/><color rgb=\"FFC77A0A\"/><name val=\"Segoe UI\"/></font>" +                      // 6 orange
        "<font><b/><sz val=\"10\"/><color rgb=\"FFD93636\"/><name val=\"Segoe UI\"/></font>" +                      // 7 red
        "<font><b/><sz val=\"10\"/><color rgb=\"FF7C4DDB\"/><name val=\"Segoe UI\"/></font>" +                      // 8 purple
        "<font><b/><sz val=\"10\"/><color rgb=\"FF3A7BD5\"/><name val=\"Segoe UI\"/></font>" +                      // 9 blue
        "</fonts>" +

        "<fills count=\"5\">" +
        "<fill><patternFill patternType=\"none\"/></fill>" +
        "<fill><patternFill patternType=\"gray125\"/></fill>" +
        "<fill><patternFill patternType=\"solid\"><fgColor rgb=\"FF3A7BD5\"/><bgColor indexed=\"64\"/></patternFill></fill>" +   // 2 header blue
        "<fill><patternFill patternType=\"solid\"><fgColor rgb=\"FFEAF2FD\"/><bgColor indexed=\"64\"/></patternFill></fill>" +   // 3 soft blue
        "<fill><patternFill patternType=\"solid\"><fgColor rgb=\"FFF6F8FB\"/><bgColor indexed=\"64\"/></patternFill></fill>" +   // 4 spare
        "</fills>" +

        "<borders count=\"2\">" +
        "<border><left/><right/><top/><bottom/><diagonal/></border>" +
        "<border><left/><right/><top/><bottom style=\"thin\"><color rgb=\"FFE2E8F0\"/></bottom><diagonal/></border>" +
        "</borders>" +

        "<cellStyleXfs count=\"1\"><xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\"/></cellStyleXfs>" +

        "<cellXfs count=\"16\">" +
        "<xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\" xfId=\"0\"/>" +                                                                                  // 0 default
        "<xf numFmtId=\"0\" fontId=\"1\" fillId=\"0\" borderId=\"0\" xfId=\"0\" applyFont=\"1\"><alignment vertical=\"center\"/></xf>" +                                // 1 title
        "<xf numFmtId=\"0\" fontId=\"2\" fillId=\"0\" borderId=\"0\" xfId=\"0\" applyFont=\"1\"><alignment vertical=\"center\"/></xf>" +                                // 2 subtitle
        "<xf numFmtId=\"0\" fontId=\"3\" fillId=\"2\" borderId=\"0\" xfId=\"0\" applyFont=\"1\" applyFill=\"1\"><alignment horizontal=\"left\" vertical=\"center\" indent=\"1\"/></xf>" + // 3 header
        "<xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"1\" xfId=\"0\" applyFont=\"1\" applyBorder=\"1\"><alignment horizontal=\"left\" vertical=\"center\" indent=\"1\"/></xf>" + // 4 text
        "<xf numFmtId=\"164\" fontId=\"0\" fillId=\"0\" borderId=\"1\" xfId=\"0\" applyNumberFormat=\"1\" applyFont=\"1\" applyBorder=\"1\"><alignment horizontal=\"left\" vertical=\"center\" indent=\"1\"/></xf>" + // 5 date+time
        "<xf numFmtId=\"165\" fontId=\"0\" fillId=\"0\" borderId=\"1\" xfId=\"0\" applyNumberFormat=\"1\" applyFont=\"1\" applyBorder=\"1\"><alignment horizontal=\"left\" vertical=\"center\" indent=\"1\"/></xf>" + // 6 date
        "<xf numFmtId=\"3\" fontId=\"0\" fillId=\"0\" borderId=\"1\" xfId=\"0\" applyNumberFormat=\"1\" applyFont=\"1\" applyBorder=\"1\"><alignment horizontal=\"right\" vertical=\"center\" indent=\"1\"/></xf>" +  // 7 integer
        "<xf numFmtId=\"166\" fontId=\"0\" fillId=\"0\" borderId=\"1\" xfId=\"0\" applyNumberFormat=\"1\" applyFont=\"1\" applyBorder=\"1\"><alignment horizontal=\"right\" vertical=\"center\" indent=\"1\"/></xf>" + // 8 peso
        "<xf numFmtId=\"0\" fontId=\"4\" fillId=\"3\" borderId=\"0\" xfId=\"0\" applyFont=\"1\" applyFill=\"1\"><alignment vertical=\"center\" indent=\"1\"/></xf>" +             // 9 label
        "<xf numFmtId=\"166\" fontId=\"4\" fillId=\"3\" borderId=\"0\" xfId=\"0\" applyNumberFormat=\"1\" applyFont=\"1\" applyFill=\"1\"><alignment horizontal=\"right\" vertical=\"center\" indent=\"1\"/></xf>" + // 10 peso bold
        "<xf numFmtId=\"0\" fontId=\"5\" fillId=\"0\" borderId=\"1\" xfId=\"0\" applyFont=\"1\" applyBorder=\"1\"><alignment horizontal=\"left\" vertical=\"center\" indent=\"1\"/></xf>" +  // 11 good
        "<xf numFmtId=\"0\" fontId=\"6\" fillId=\"0\" borderId=\"1\" xfId=\"0\" applyFont=\"1\" applyBorder=\"1\"><alignment horizontal=\"left\" vertical=\"center\" indent=\"1\"/></xf>" +  // 12 warn
        "<xf numFmtId=\"0\" fontId=\"7\" fillId=\"0\" borderId=\"1\" xfId=\"0\" applyFont=\"1\" applyBorder=\"1\"><alignment horizontal=\"left\" vertical=\"center\" indent=\"1\"/></xf>" +  // 13 bad
        "<xf numFmtId=\"0\" fontId=\"8\" fillId=\"0\" borderId=\"1\" xfId=\"0\" applyFont=\"1\" applyBorder=\"1\"><alignment horizontal=\"left\" vertical=\"center\" indent=\"1\"/></xf>" +  // 14 refund
        "<xf numFmtId=\"0\" fontId=\"9\" fillId=\"0\" borderId=\"1\" xfId=\"0\" applyFont=\"1\" applyBorder=\"1\"><alignment horizontal=\"left\" vertical=\"center\" indent=\"1\"/></xf>" +  // 15 info
        "</cellXfs>" +

        "<cellStyles count=\"1\"><cellStyle name=\"Normal\" xfId=\"0\" builtinId=\"0\"/></cellStyles>" +
        "</styleSheet>";
}

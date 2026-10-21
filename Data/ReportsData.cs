using System.Data;
using System.Globalization;
using System.Text;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>The three tabs of the Reports &amp; Financial Auditing page.</summary>
internal enum AuditTab { SalesLogs = 0, Movement = 1, Summary = 2 }

/// <summary>Colour family of a status pill in the audit table (and in the PDF).</summary>
internal enum AuditBadge { Good, Warn, Bad, Info, Refund }

/// <summary>One line of the audit table. The text columns are ready to show; AmountValue / QuantityValue are the numbers for the exports.</summary>
internal sealed record AuditRow(
    DateTime When, string Type, string Item, string Quantity, string Amount, string Reference,
    string Status, AuditBadge Badge, int? TransactionId,
    bool AmountIsNegative = false, string Note = "", decimal? AmountValue = null, int? QuantityValue = null);

/// <summary>What to look at: a date window (From inclusive, ToExclusive exclusive), plus an optional category and staff member.</summary>
internal sealed record AuditFilter(DateTime? From, DateTime? ToExclusive, string? Category, string? Staff);

/// <summary>The numbers on the four summary cards.</summary>
internal sealed record AuditTotals(decimal Sales, int Units, int Discrepancies);

/// <summary>What the page asked for: the filter, the two periods used for the "vs." lines, and the tab.</summary>
internal sealed record AuditQuery(AuditFilter Current, AuditFilter CompareNow, AuditFilter? ComparePrev, AuditTab Tab, string CompareLabel);

/// <summary>The rows for the selected tab plus the card totals (now, and for the period it is compared with).</summary>
internal sealed record AuditReport(List<AuditRow> Rows, AuditTotals Current, AuditTotals CompareNow, AuditTotals ComparePrev);

/// <summary>
/// Everything the admin Reports &amp; Financial Auditing page reads. Nothing is stored here: sales come from
/// dbo.Transactions / dbo.TransactionLines, stock movement from dbo.StockBatches (stock in) and the product lines of sales
/// (stock out). Every sale is also re-checked (line items vs. subtotal, subtotal - discount vs. total, cash received vs. change)
/// and anything that doesn't add up is flagged as a discrepancy.
/// </summary>
internal static class ReportsData
{
    // ------------------------------------------------------------------ public helpers

    public static string[] Headers(AuditTab tab) => tab switch
    {
        AuditTab.Movement => new[] { "DATE & TIME", "MOVEMENT", "PRODUCT", "QUANTITY", "VALUE", "REFERENCE NO.", "STATUS" },
        AuditTab.Summary => new[] { "DATE", "TYPE", "SUMMARY", "ITEMS", "NET SALES", "RECEIPTS", "STATUS" },
        _ => new[] { "DATE & TIME", "TRANSACTION TYPE", "PRODUCT / SERVICE", "QUANTITY", "AMOUNT", "REFERENCE NO.", "STATUS" },
    };

    public static string EmptyHint(AuditTab tab) => tab switch
    {
        AuditTab.Movement => "Stock received and stock sold will be listed here once they are recorded.",
        AuditTab.Summary => "A daily summary appears here once there are sales in the selected period.",
        _ => "Sales, returns and stock adjustments will be listed here once they are recorded.",
    };

    public static string Title(AuditTab tab) => tab switch
    {
        AuditTab.Movement => "Inventory Movement Report",
        AuditTab.Summary => "Audit Summary",
        _ => "Sales Audit Log",
    };

    public static Color BadgeFore(AuditBadge b) => b switch
    {
        AuditBadge.Good => Color.FromArgb(46, 158, 91),
        AuditBadge.Warn => Color.FromArgb(199, 122, 10),
        AuditBadge.Bad => Color.FromArgb(217, 54, 54),
        AuditBadge.Refund => Color.FromArgb(124, 77, 219),
        _ => Color.FromArgb(58, 123, 213),
    };

    public static Color BadgeFill(AuditBadge b) => b switch
    {
        AuditBadge.Good => Color.FromArgb(229, 246, 236),
        AuditBadge.Warn => Color.FromArgb(254, 243, 224),
        AuditBadge.Bad => Color.FromArgb(253, 232, 232),
        AuditBadge.Refund => Color.FromArgb(241, 234, 253),
        _ => Color.FromArgb(234, 242, 253),
    };

    public static Color BadgeBorder(AuditBadge b) => b switch
    {
        AuditBadge.Good => Color.FromArgb(191, 229, 206),
        AuditBadge.Warn => Color.FromArgb(248, 217, 160),
        AuditBadge.Bad => Color.FromArgb(246, 192, 192),
        AuditBadge.Refund => Color.FromArgb(217, 200, 248),
        _ => Color.FromArgb(201, 220, 245),
    };

    // ------------------------------------------------------------------ connection + schema

    private sealed record Schema(
        bool Transactions, bool Lines, bool Batches, bool UnitCost, bool StockDeducted, bool ProductQty, bool Products, bool Services, bool Users);

    private static SqlConnection Open()
    {
        var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        return conn;
    }

    /// <summary>Which tables / columns exist, so a database that has not been fully set up yet shows an empty report instead of an error.</summary>
    private static Schema Probe(SqlConnection conn)
    {
        using var cmd = new SqlCommand(@"
SELECT
  CASE WHEN OBJECT_ID(N'dbo.Transactions', N'U') IS NULL THEN 0 ELSE 1 END,
  CASE WHEN OBJECT_ID(N'dbo.TransactionLines', N'U') IS NULL THEN 0 ELSE 1 END,
  CASE WHEN OBJECT_ID(N'dbo.StockBatches', N'U') IS NULL THEN 0 ELSE 1 END,
  CASE WHEN COL_LENGTH(N'dbo.Products', N'UnitCost') IS NULL THEN 0 ELSE 1 END,
  CASE WHEN COL_LENGTH(N'dbo.Transactions', N'StockDeducted') IS NULL THEN 0 ELSE 1 END,
  CASE WHEN COL_LENGTH(N'dbo.Products', N'QtyOnHand') IS NULL THEN 0 ELSE 1 END,
  CASE WHEN OBJECT_ID(N'dbo.Products', N'U') IS NULL THEN 0 ELSE 1 END,
  CASE WHEN OBJECT_ID(N'dbo.Services', N'U') IS NULL THEN 0 ELSE 1 END,
  CASE WHEN OBJECT_ID(N'dbo.Users', N'U') IS NULL THEN 0 ELSE 1 END", conn);
        using var r = cmd.ExecuteReader();
        r.Read();
        bool B(int i) => r.GetInt32(i) == 1;
        return new Schema(B(0), B(1), B(2), B(3), B(4), B(5), B(6), B(7), B(8));
    }

    private static void Fill(SqlCommand cmd, AuditFilter f)
    {
        cmd.Parameters.Add("@from", SqlDbType.DateTime2).Value = f.From is DateTime a ? a : DBNull.Value;
        cmd.Parameters.Add("@to", SqlDbType.DateTime2).Value = f.ToExclusive is DateTime b ? b : DBNull.Value;
        cmd.Parameters.Add("@cat", SqlDbType.NVarChar, 60).Value = f.Category is string c ? c : DBNull.Value;
        cmd.Parameters.Add("@staff", SqlDbType.NVarChar, 120).Value = f.Staff is string s ? s : DBNull.Value;
    }

    // ------------------------------------------------------------------ filter lists

    public static List<string> GetCategories()
    {
        using var conn = Open();
        var s = Probe(conn);
        var parts = new List<string>();
        if (s.Services) parts.Add("SELECT Category FROM dbo.Services");
        if (s.Products) parts.Add("SELECT Category FROM dbo.Products");
        var list = new List<string>();
        if (parts.Count == 0) return list;

        using var cmd = new SqlCommand(
            "SELECT DISTINCT Category FROM (" + string.Join(" UNION ALL ", parts) + ") x WHERE Category IS NOT NULL AND Category <> N'' ORDER BY Category", conn);
        using var r = cmd.ExecuteReader();
        while (r.Read()) list.Add(r.GetString(0));
        return list;
    }

    public static List<string> GetStaff()
    {
        using var conn = Open();
        var s = Probe(conn);
        var list = new List<string>();
        if (!s.Transactions || !s.Users) return list;

        using var cmd = new SqlCommand(@"
SELECT DISTINCT u.FullName FROM dbo.Transactions t JOIN dbo.Users u ON u.UserId = t.CashierUserId ORDER BY u.FullName", conn);
        using var r = cmd.ExecuteReader();
        while (r.Read()) list.Add(r.GetString(0));
        return list;
    }

    // ------------------------------------------------------------------ loading

    private sealed class Tx
    {
        public int Id;
        public string Receipt = "";
        public DateTime When;
        public string Status = "";
        public decimal Subtotal, Discount, Total, Received, Change;
        public string Method = "";
        public string Staff = "";
        public readonly List<(string Name, int Qty, decimal Price)> Lines = new();
        public readonly List<string> Problems = new();
    }

    private sealed record Move(DateTime When, bool In, string Product, int Qty, decimal? Value, string Reference, string Status, AuditBadge Badge, int? TxId);

    private sealed record StockIssue(string Product, int OnHand, int InBatches);

    /// <summary>Loads the rows for the tab and the card totals. Throws SqlException if the database can't be reached.</summary>
    public static AuditReport Load(AuditFilter current, AuditFilter compareNow, AuditFilter? comparePrev, AuditTab tab)
    {
        using var conn = Open();
        var schema = Probe(conn);

        var sales = LoadSales(conn, schema, current);
        var moves = LoadMoves(conn, schema, current);
        var issues = LoadStockIssues(conn, schema, current);

        var now = TotalsOf(sales, moves, issues.Count);

        AuditTotals nowCompare = now;
        if (compareNow != current)
            nowCompare = TotalsOf(LoadSales(conn, schema, compareNow), LoadMoves(conn, schema, compareNow), issues.Count);

        // Stock mismatches describe the stock as it is today, so they are counted the same way in both periods and never swing the comparison.
        AuditTotals prev = new(0, 0, issues.Count);
        if (comparePrev is not null)
            prev = TotalsOf(LoadSales(conn, schema, comparePrev), LoadMoves(conn, schema, comparePrev), issues.Count);

        var rows = tab switch
        {
            AuditTab.Movement => MovementRows(moves, issues, current),
            AuditTab.Summary => SummaryRows(sales),
            _ => sales.Select(SaleRow).ToList(),
        };

        return new AuditReport(rows, now, nowCompare, prev);
    }

    private static AuditTotals TotalsOf(List<Tx> sales, List<Move> moves, int stockIssues) => new(
        sales.Where(t => t.Status == "Completed").Sum(t => t.Total),
        moves.Sum(m => m.Qty),
        sales.Count(t => t.Problems.Count > 0) + stockIssues);

    // ---- sales

    private static List<Tx> LoadSales(SqlConnection conn, Schema schema, AuditFilter f)
    {
        var list = new List<Tx>();
        if (!schema.Transactions) return list;

        string staffJoin = schema.Users ? "LEFT JOIN dbo.Users u ON u.UserId = t.CashierUserId" : "";
        string staffName = schema.Users ? "ISNULL(u.FullName, N'')" : "N''";
        string staffWhere = schema.Users ? "AND (@staff IS NULL OR u.FullName = @staff)" : "";

        string catWhere = "";
        if (schema.Lines && (schema.Services || schema.Products))
        {
            string sJoin = schema.Services ? "LEFT JOIN dbo.Services s ON s.ServiceId = l.ServiceId" : "";
            string pJoin = schema.Products ? "LEFT JOIN dbo.Products p ON p.ProductId = l.ProductId" : "";
            string catExpr = schema.Services && schema.Products ? "ISNULL(s.Category, p.Category)"
                           : schema.Services ? "s.Category" : "p.Category";
            catWhere = $@"AND (@cat IS NULL OR EXISTS (SELECT 1 FROM dbo.TransactionLines l {sJoin} {pJoin}
                                                      WHERE l.TransactionId = t.TransactionId AND {catExpr} = @cat))";
        }

        string sql = $@"
SELECT t.TransactionId, t.ReceiptNumber, t.CreatedAt, t.Status, t.Subtotal, t.Discount, t.Total,
       t.PaymentMethod, t.AmountReceived, t.ChangeGiven, {staffName}
FROM dbo.Transactions t
{staffJoin}
WHERE (@from IS NULL OR t.CreatedAt >= @from) AND (@to IS NULL OR t.CreatedAt < @to)
  {staffWhere}
  {catWhere}
ORDER BY t.CreatedAt DESC, t.TransactionId DESC";

        using (var cmd = new SqlCommand(sql, conn))
        {
            Fill(cmd, f);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new Tx
                {
                    Id = r.GetInt32(0),
                    Receipt = r.GetString(1),
                    When = r.GetDateTime(2),
                    Status = r.GetString(3),
                    Subtotal = r.GetDecimal(4),
                    Discount = r.GetDecimal(5),
                    Total = r.GetDecimal(6),
                    Method = r.GetString(7),
                    Received = r.GetDecimal(8),
                    Change = r.GetDecimal(9),
                    Staff = r.GetString(10),
                });
            }
        }

        if (list.Count == 0) return list;

        var byId = list.ToDictionary(t => t.Id);
        if (schema.Lines)
        {
            // Every line in the same date window; the dictionary keeps only the ones that belong to a listed sale.
            using var cmd = new SqlCommand(@"
SELECT l.TransactionId, l.Name, l.Quantity, l.UnitPrice
FROM dbo.TransactionLines l JOIN dbo.Transactions t ON t.TransactionId = l.TransactionId
WHERE (@from IS NULL OR t.CreatedAt >= @from) AND (@to IS NULL OR t.CreatedAt < @to)
ORDER BY l.TransactionId, l.LineId", conn);
            Fill(cmd, f);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                if (byId.TryGetValue(r.GetInt32(0), out var t))
                    t.Lines.Add((r.GetString(1), r.GetInt32(2), r.GetDecimal(3)));
        }

        foreach (var t in list) Audit(t);
        return list;
    }

    /// <summary>Re-checks the arithmetic of one sale. Anything that doesn't add up becomes a "discrepancy".</summary>
    private static void Audit(Tx t)
    {
        const decimal tolerance = 0.005m;
        decimal lineSum = t.Lines.Sum(l => l.Price * l.Qty);

        if (t.Lines.Count == 0)
            t.Problems.Add("The sale has no line items.");
        else if (Math.Abs(lineSum - t.Subtotal) > tolerance)
            t.Problems.Add($"The line items add up to {Money.Format(lineSum)}, but the subtotal is {Money.Format(t.Subtotal)}.");

        if (Math.Abs(t.Subtotal - t.Discount - t.Total) > tolerance)
            t.Problems.Add($"Subtotal {Money.Format(t.Subtotal)} minus discount {Money.Format(t.Discount)} should be {Money.Format(t.Subtotal - t.Discount)}, but the total is {Money.Format(t.Total)}.");

        if (t.Status == "Completed" && t.Method == "Cash")
        {
            if (t.Received < t.Total - tolerance)
                t.Problems.Add($"Cash received ({Money.Format(t.Received)}) is less than the total ({Money.Format(t.Total)}).");
            else if (Math.Abs(t.Received - t.Total - t.Change) > tolerance)
                t.Problems.Add($"Change given ({Money.Format(t.Change)}) doesn't match cash received minus the total ({Money.Format(t.Received - t.Total)}).");
        }
    }

    private static AuditRow SaleRow(Tx t)
    {
        string type = t.Status == "Refunded" ? "Refund" : t.Status == "Voided" ? "Void" : "Sale";
        string item = t.Lines.Count == 0 ? "—"
                    : t.Lines.Count == 1 ? t.Lines[0].Name
                    : $"{t.Lines[0].Name} +{t.Lines.Count - 1} more";
        int qty = t.Lines.Sum(l => l.Qty);
        bool negative = t.Status is "Refunded" or "Voided";
        bool flagged = t.Problems.Count > 0;

        string status;
        AuditBadge badge;
        if (flagged) { status = "Discrepancy"; badge = AuditBadge.Bad; }
        else if (t.Status == "Completed") { status = "Completed"; badge = AuditBadge.Good; }
        else if (t.Status == "Pending") { status = "Pending"; badge = AuditBadge.Warn; }
        else if (t.Status == "Refunded") { status = "Refunded"; badge = AuditBadge.Refund; }
        else if (t.Status == "Voided") { status = "Voided"; badge = AuditBadge.Bad; }
        else { status = t.Status; badge = AuditBadge.Info; }

        return new AuditRow(
            t.When, type, item, qty.ToString("N0", CultureInfo.CurrentCulture),
            (negative ? "−" : "") + Money.Format(t.Total), t.Receipt, status, badge, t.Id,
            AmountIsNegative: negative,
            Note: string.Join("\n", t.Problems),
            AmountValue: negative ? -t.Total : t.Total,
            QuantityValue: qty);
    }

    private static List<AuditRow> SummaryRows(List<Tx> sales)
    {
        var culture = CultureInfo.CurrentCulture;
        return sales
            .GroupBy(t => t.When.Date)
            .OrderByDescending(g => g.Key)
            .Select(g =>
            {
                var done = g.Where(t => t.Status == "Completed").ToList();
                int flagged = g.Count(t => t.Problems.Count > 0);
                int other = g.Count(t => t.Status is "Refunded" or "Voided");
                var ordered = g.OrderBy(t => t.Id).ToList();

                string summary = $"{done.Count} completed sale{(done.Count == 1 ? "" : "s")}"
                               + (other > 0 ? $", {other} refunded / voided" : "");
                string reference = ordered.Count == 1 ? ordered[0].Receipt : $"{ordered[0].Receipt} – {ordered[^1].Receipt}";
                int items = done.Sum(t => t.Lines.Sum(l => l.Qty));
                decimal net = done.Sum(t => t.Total);

                return new AuditRow(
                    g.Key, "Daily Summary", summary, items.ToString("N0", culture), Money.Format(net), reference,
                    flagged > 0 ? $"{flagged} to review" : "Balanced",
                    flagged > 0 ? AuditBadge.Bad : AuditBadge.Good, null,
                    Note: string.Join("\n", g.SelectMany(t => t.Problems.Select(p => $"{t.Receipt}: {p}"))),
                    AmountValue: net, QuantityValue: items);
            })
            .ToList();
    }

    // ---- stock movement

    private static List<Move> LoadMoves(SqlConnection conn, Schema schema, AuditFilter f)
    {
        var list = new List<Move>();

        // Stock in: every delivery recorded in Inventory & FIFO (the automatic "OPENING" balance is not a delivery).
        // Deliveries have no staff member on record, so they are left out when a staff filter is chosen.
        if (schema.Batches && schema.Products && f.Staff is null)
        {
            string cost = schema.UnitCost ? "p.UnitCost" : "CAST(NULL AS DECIMAL(10,2))";
            using var cmd = new SqlCommand($@"
SELECT b.BatchNo, b.ReceivedDate, b.CreatedAt, b.QtyReceived, p.Name, {cost}
FROM dbo.StockBatches b JOIN dbo.Products p ON p.ProductId = b.ProductId
WHERE b.BatchNo <> N'OPENING'
  AND (@from IS NULL OR b.ReceivedDate >= @from) AND (@to IS NULL OR b.ReceivedDate < @to)
  AND (@cat IS NULL OR p.Category = @cat)
ORDER BY b.ReceivedDate DESC, b.BatchId DESC", conn);
            Fill(cmd, f);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                DateTime day = r.GetDateTime(1).Date;
                DateTime created = r.GetDateTime(2).ToLocalTime();
                DateTime when = created.Date == day ? created : day;   // show the time of day when it was received the same day it was typed in
                int qty = r.GetInt32(3);
                decimal? cost2 = r.IsDBNull(5) ? null : r.GetDecimal(5);
                list.Add(new Move(when, true, r.GetString(4), qty, cost2 is decimal c ? c * qty : null,
                    r.GetString(0), "Received", AuditBadge.Good, null));
            }
        }

        // Stock out: product lines of sales that really took stock off the shelf. Voided sales put it back, so they are skipped.
        if (schema.Transactions && schema.Lines)
        {
            string deducted = schema.StockDeducted ? "AND t.StockDeducted = 1" : "";
            string catJoin = schema.Products ? "LEFT JOIN dbo.Products p ON p.ProductId = l.ProductId" : "";
            string catWhere = schema.Products ? "AND (@cat IS NULL OR p.Category = @cat)" : "";
            string staffJoin = schema.Users ? "LEFT JOIN dbo.Users u ON u.UserId = t.CashierUserId" : "";
            string staffWhere = schema.Users ? "AND (@staff IS NULL OR u.FullName = @staff)" : "";

            using var cmd = new SqlCommand($@"
SELECT t.TransactionId, t.ReceiptNumber, t.CreatedAt, t.Status, l.Name, l.UnitPrice, l.Quantity
FROM dbo.TransactionLines l
JOIN dbo.Transactions t ON t.TransactionId = l.TransactionId
{catJoin}
{staffJoin}
WHERE l.LineKind = N'Product' AND t.Status <> N'Voided' {deducted}
  AND (@from IS NULL OR t.CreatedAt >= @from) AND (@to IS NULL OR t.CreatedAt < @to)
  {catWhere}
  {staffWhere}
ORDER BY t.CreatedAt DESC, l.LineId DESC", conn);
            Fill(cmd, f);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                int qty = r.GetInt32(6);
                bool refunded = r.GetString(3) == "Refunded";
                list.Add(new Move(r.GetDateTime(2), false, r.GetString(4), qty, r.GetDecimal(5) * qty,
                    r.GetString(1), refunded ? "Refunded" : "Sold",
                    refunded ? AuditBadge.Refund : AuditBadge.Info, r.GetInt32(0)));
            }
        }

        return list;
    }

    /// <summary>Products whose stock total doesn't match their batches (or has gone negative). This is today's state, not tied to a date range.</summary>
    private static List<StockIssue> LoadStockIssues(SqlConnection conn, Schema schema, AuditFilter f)
    {
        var list = new List<StockIssue>();
        if (!schema.Products || !schema.ProductQty || f.Staff is not null) return list;

        string batches = schema.Batches
            ? "LEFT JOIN (SELECT ProductId, SUM(QtyRemaining) AS Qty FROM dbo.StockBatches GROUP BY ProductId) b ON b.ProductId = p.ProductId"
            : "LEFT JOIN (SELECT CAST(NULL AS INT) AS ProductId, CAST(NULL AS INT) AS Qty WHERE 1 = 0) b ON b.ProductId = p.ProductId";

        using var cmd = new SqlCommand($@"
SELECT p.Name, p.QtyOnHand, ISNULL(b.Qty, 0)
FROM dbo.Products p
{batches}
WHERE p.IsActive = 1
  AND (p.QtyOnHand < 0 OR (b.Qty IS NOT NULL AND b.Qty <> p.QtyOnHand))
  AND (@cat IS NULL OR p.Category = @cat)
ORDER BY p.Name", conn);
        Fill(cmd, f);
        using var r = cmd.ExecuteReader();
        while (r.Read()) list.Add(new StockIssue(r.GetString(0), r.GetInt32(1), r.GetInt32(2)));
        return list;
    }

    private static List<AuditRow> MovementRows(List<Move> moves, List<StockIssue> issues, AuditFilter f)
    {
        var culture = CultureInfo.CurrentCulture;
        var rows = new List<AuditRow>();

        foreach (var m in moves.OrderByDescending(m => m.When))
        {
            rows.Add(new AuditRow(
                m.When, m.In ? "Stock In" : "Stock Out", m.Product,
                (m.In ? "+" : "−") + m.Qty.ToString("N0", culture),
                m.Value is decimal v ? Money.Format(v) : "—",
                m.Reference, m.Status, m.Badge, m.TxId,
                AmountValue: m.Value, QuantityValue: m.In ? m.Qty : -m.Qty));
        }

        foreach (var i in issues)
        {
            int diff = i.InBatches - i.OnHand;
            rows.Insert(0, new AuditRow(
                DateTime.Now, "Stock Check", i.Product,
                (diff > 0 ? "+" : diff < 0 ? "−" : "") + Math.Abs(diff).ToString("N0", culture),
                "—", "Inventory", "Mismatch", AuditBadge.Bad, null,
                Note: i.OnHand < 0
                    ? $"{i.Product} shows {i.OnHand} in stock, which is below zero."
                    : $"{i.Product} shows {i.OnHand} in stock, but its batches add up to {i.InBatches}.",
                QuantityValue: diff));
        }

        return rows;
    }

    // ------------------------------------------------------------------ CSV (opens in Excel)

    /// <summary>Writes the rows as a UTF-8 CSV with the numbers as numbers, so Excel can add them up.</summary>
    public static void WriteCsv(string path, AuditTab tab, IEnumerable<AuditRow> rows, string period, string preparedBy)
    {
        var inv = CultureInfo.InvariantCulture;
        static string Q(string s) => "\"" + s.Replace("\"", "\"\"") + "\"";

        using var w = new StreamWriter(path, false, new UTF8Encoding(true));
        w.WriteLine(Q("Pawfect - " + Title(tab)));
        w.WriteLine(Q("Period: " + period + "  |  Generated " + DateTime.Now.ToString("yyyy-MM-dd HH:mm", inv) + "  |  " + preparedBy));
        w.WriteLine();

        var h = Headers(tab);
        string amountHeader = tab == AuditTab.Summary ? "NET SALES (PHP)" : (tab == AuditTab.Movement ? "VALUE (PHP)" : "AMOUNT (PHP)");
        w.WriteLine(string.Join(",", new[] { h[0], h[1], h[2], h[3], amountHeader, h[5], h[6], "NOTES" }.Select(Q)));

        foreach (var r in rows)
        {
            w.WriteLine(string.Join(",", new[]
            {
                Q(r.When.ToString(tab == AuditTab.Summary ? "yyyy-MM-dd" : "yyyy-MM-dd HH:mm", inv)),
                Q(r.Type),
                Q(r.Item),
                r.QuantityValue is int q ? q.ToString(inv) : "",
                r.AmountValue is decimal a ? a.ToString("0.00", inv) : "",
                Q(r.Reference),
                Q(r.Status),
                Q(r.Note.Replace("\n", " ")),
            }));
        }
    }
}

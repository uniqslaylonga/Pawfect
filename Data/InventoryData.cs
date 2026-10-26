using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>One line of the Inventory table: a product and one of its stock batches. A product with no stock has a single line with no batch.</summary>
internal sealed record InventoryRow(
    int ProductId, string Code, string Name, string Category, int QtyOnHand, int ReorderLevel,
    int? BatchId, string BatchNo, string Supplier, DateTime? Received, DateTime? Expiry, int BatchQty);

internal enum StockState { InStock, LowStock, OutOfStock, NearExpiry, Expired }

/// <summary>A product as the Add / Edit Product dialog needs it.</summary>
internal sealed record InventoryProduct(int Id, string Code, string Name, string Category, decimal Price, decimal? UnitCost, int ReorderLevel);

/// <summary>
/// Everything the admin Inventory &amp; FIFO page reads and writes. Stock is kept in two places that always move together:
/// dbo.Products.QtyOnHand (the total, used by Counter POS) and dbo.StockBatches (each delivery, oldest first).
/// The batch table and the extra product columns are created automatically the first time they are needed.
/// </summary>
internal static class InventoryData
{
    /// <summary>A batch that expires within this many days is shown as "Near Expiry".</summary>
    public const int NearExpiryDays = 30;

    private static bool _schemaChecked;

    private static SqlConnection Open()
    {
        var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        EnsureSchema(conn);
        return conn;
    }

    private static void EnsureSchema(SqlConnection conn)
    {
        ProductSchema.Ensure(conn);   // Code / Name / Category / Price / IsActive, if this database lacks them
        if (_schemaChecked) return;

        string[] steps =
        {
            @"
IF OBJECT_ID(N'dbo.Products', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.Products', N'QtyOnHand') IS NULL
    ALTER TABLE dbo.Products ADD QtyOnHand INT NOT NULL CONSTRAINT DF_Products_Qty DEFAULT 0;",
            @"
IF OBJECT_ID(N'dbo.Products', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.Products', N'ReorderLevel') IS NULL
    ALTER TABLE dbo.Products ADD ReorderLevel INT NOT NULL CONSTRAINT DF_Products_Reorder DEFAULT 0;",
            @"
IF OBJECT_ID(N'dbo.Products', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.Products', N'UnitCost') IS NULL
    ALTER TABLE dbo.Products ADD UnitCost DECIMAL(10,2) NULL;",
            @"
IF OBJECT_ID(N'dbo.Products', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.Products', N'CreatedAt') IS NULL
    ALTER TABLE dbo.Products ADD CreatedAt DATETIME2 NULL;",
            @"
IF OBJECT_ID(N'dbo.StockBatches', N'U') IS NULL
CREATE TABLE dbo.StockBatches
(
    BatchId      INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_StockBatches PRIMARY KEY,
    ProductId    INT           NOT NULL CONSTRAINT FK_StockBatches_Products REFERENCES dbo.Products(ProductId),
    BatchNo      NVARCHAR(40)  NOT NULL,
    Supplier     NVARCHAR(100) NULL,
    ReceivedDate DATE          NOT NULL,
    ExpiryDate   DATE          NULL,
    QtyReceived  INT           NOT NULL,
    QtyRemaining INT           NOT NULL,
    CreatedAt    DATETIME2     NOT NULL CONSTRAINT DF_StockBatches_CreatedAt DEFAULT SYSUTCDATETIME()
);",
            // An older StockBatches table may already exist without the newer columns: add whatever is missing.
            @"
IF OBJECT_ID(N'dbo.StockBatches', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.StockBatches', N'BatchNo') IS NULL
    ALTER TABLE dbo.StockBatches ADD BatchNo NVARCHAR(40) NOT NULL CONSTRAINT DF_StockBatches_BatchNo DEFAULT N'OPENING';",
            @"
IF OBJECT_ID(N'dbo.StockBatches', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.StockBatches', N'Supplier') IS NULL
    ALTER TABLE dbo.StockBatches ADD Supplier NVARCHAR(100) NULL;",
            @"
IF OBJECT_ID(N'dbo.StockBatches', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.StockBatches', N'ReceivedDate') IS NULL
    ALTER TABLE dbo.StockBatches ADD ReceivedDate DATE NOT NULL CONSTRAINT DF_StockBatches_ReceivedDate DEFAULT CAST(SYSDATETIME() AS DATE);",
            @"
IF OBJECT_ID(N'dbo.StockBatches', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.StockBatches', N'ExpiryDate') IS NULL
    ALTER TABLE dbo.StockBatches ADD ExpiryDate DATE NULL;",
            @"
IF OBJECT_ID(N'dbo.StockBatches', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.StockBatches', N'QtyReceived') IS NULL
    ALTER TABLE dbo.StockBatches ADD QtyReceived INT NOT NULL CONSTRAINT DF_StockBatches_QtyReceived DEFAULT 0;",
            @"
IF OBJECT_ID(N'dbo.StockBatches', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.StockBatches', N'QtyRemaining') IS NULL
    ALTER TABLE dbo.StockBatches ADD QtyRemaining INT NOT NULL CONSTRAINT DF_StockBatches_QtyRemaining DEFAULT 0;",
            @"
IF OBJECT_ID(N'dbo.StockBatches', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.StockBatches', N'CreatedAt') IS NULL
    ALTER TABLE dbo.StockBatches ADD CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_StockBatches_CreatedAt2 DEFAULT SYSUTCDATETIME();",
            @"
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_StockBatches_Product' AND object_id = OBJECT_ID(N'dbo.StockBatches'))
    CREATE INDEX IX_StockBatches_Product ON dbo.StockBatches (ProductId, ReceivedDate, BatchId) INCLUDE (QtyRemaining);",
        };

        try
        {
            foreach (var sql in steps)
            {
                using var cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            _schemaChecked = true;
        }
        catch (SqlException)
        {
            // no permission to alter the database: the real error shows up in the query that needs the new table or column
        }
    }

    // ------------------------------------------------------------------ status

    public static StockState StateOf(InventoryRow r, DateTime today)
    {
        if (r.Expiry is DateTime e && e.Date < today) return StockState.Expired;
        if (r.QtyOnHand <= 0) return StockState.OutOfStock;
        if (r.Expiry is DateTime e2 && e2.Date <= today.AddDays(NearExpiryDays)) return StockState.NearExpiry;
        if (r.ReorderLevel > 0 && r.QtyOnHand <= r.ReorderLevel) return StockState.LowStock;
        return StockState.InStock;
    }

    public static string Label(StockState s) => s switch
    {
        StockState.LowStock => "Low Stock",
        StockState.OutOfStock => "Out of Stock",
        StockState.NearExpiry => "Near Expiry",
        StockState.Expired => "Expired",
        _ => "In Stock",
    };

    // ------------------------------------------------------------------ reading

    /// <summary>Every active product with its batches that still have units, oldest delivery first. Throws SqlException if the database can't be reached.</summary>
    public static List<InventoryRow> GetRows()
    {
        using var conn = Open();

        // Stock that was counted before batches existed (or was changed outside the app) becomes an "OPENING" batch,
        // so the batches always add up to the product's total.
        using (var fix = new SqlCommand(@"
INSERT INTO dbo.StockBatches (ProductId, BatchNo, Supplier, ReceivedDate, ExpiryDate, QtyReceived, QtyRemaining)
SELECT p.ProductId, N'OPENING', NULL, CAST(SYSDATETIME() AS DATE), NULL, p.QtyOnHand - ISNULL(b.Qty, 0), p.QtyOnHand - ISNULL(b.Qty, 0)
FROM dbo.Products p
LEFT JOIN (SELECT ProductId, SUM(QtyRemaining) AS Qty FROM dbo.StockBatches GROUP BY ProductId) b ON b.ProductId = p.ProductId
WHERE p.IsActive = 1 AND p.QtyOnHand > ISNULL(b.Qty, 0);", conn))
        {
            fix.ExecuteNonQuery();
        }

        var list = new List<InventoryRow>();
        using var cmd = new SqlCommand(@"
SELECT p.ProductId, p.Code, p.Name, p.Category, p.QtyOnHand, p.ReorderLevel,
       b.BatchId, b.BatchNo, b.Supplier, b.ReceivedDate, b.ExpiryDate, b.QtyRemaining
FROM dbo.Products p
LEFT JOIN dbo.StockBatches b ON b.ProductId = p.ProductId AND b.QtyRemaining > 0
WHERE p.IsActive = 1
ORDER BY p.Name, b.ReceivedDate, b.BatchId", conn);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new InventoryRow(
                r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetString(3), r.GetInt32(4), r.GetInt32(5),
                r.IsDBNull(6) ? null : r.GetInt32(6),
                r.IsDBNull(7) ? "" : r.GetString(7),
                r.IsDBNull(8) ? "" : r.GetString(8),
                r.IsDBNull(9) ? null : r.GetDateTime(9),
                r.IsDBNull(10) ? null : r.GetDateTime(10),
                r.IsDBNull(11) ? 0 : r.GetInt32(11)));
        }
        return list;
    }

    /// <summary>Products added and units received since the 1st of this month, for the "vs. last month" lines.</summary>
    public static (int ProductsAdded, int UnitsReceived) GetMonthStats()
    {
        var monthStart = EmployeeManagementPage.MonthStart;
        using var conn = Open();

        int added;
        using (var cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.Products WHERE IsActive = 1 AND CreatedAt >= @m", conn))
        {
            cmd.Parameters.Add("@m", SqlDbType.DateTime2).Value = monthStart.ToUniversalTime();
            added = Convert.ToInt32(cmd.ExecuteScalar(), CultureInfo.InvariantCulture);
        }

        int units;
        using (var cmd = new SqlCommand(
            "SELECT ISNULL(SUM(QtyReceived), 0) FROM dbo.StockBatches WHERE ReceivedDate >= @d AND BatchNo <> N'OPENING'", conn))
        {
            cmd.Parameters.Add("@d", SqlDbType.Date).Value = monthStart;
            units = Convert.ToInt32(cmd.ExecuteScalar(), CultureInfo.InvariantCulture);
        }

        return (added, units);
    }

    public static InventoryProduct? GetProduct(int productId)
    {
        using var conn = Open();
        using var cmd = new SqlCommand(
            "SELECT ProductId, Code, Name, Category, Price, UnitCost, ReorderLevel FROM dbo.Products WHERE ProductId = @id", conn);
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = productId;
        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;
        return new InventoryProduct(r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetString(3), r.GetDecimal(4),
            r.IsDBNull(5) ? null : r.GetDecimal(5), r.GetInt32(6));
    }

    // ------------------------------------------------------------------ products

    /// <summary>Throws SqlException 2627/2601 if the product code already exists.</summary>
    public static void AddProduct(string code, string name, string category, decimal price, decimal? unitCost, int reorderLevel)
    {
        using var conn = Open();
        using var cmd = new SqlCommand(@"
INSERT INTO dbo.Products (Code, Name, Category, Price, UnitCost, ReorderLevel, QtyOnHand, CreatedAt)
VALUES (@code, @name, @cat, @price, @cost, @reorder, 0, SYSUTCDATETIME())", conn);
        Fill(cmd, code, name, category, price, unitCost, reorderLevel);
        cmd.ExecuteNonQuery();
    }

    /// <summary>Throws SqlException 2627/2601 if the product code is taken by another product.</summary>
    public static void UpdateProduct(int id, string code, string name, string category, decimal price, decimal? unitCost, int reorderLevel)
    {
        using var conn = Open();
        using var cmd = new SqlCommand(@"
UPDATE dbo.Products SET Code = @code, Name = @name, Category = @cat, Price = @price, UnitCost = @cost, ReorderLevel = @reorder
WHERE ProductId = @id", conn);
        Fill(cmd, code, name, category, price, unitCost, reorderLevel);
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        cmd.ExecuteNonQuery();
    }

    private static void Fill(SqlCommand cmd, string code, string name, string category, decimal price, decimal? unitCost, int reorderLevel)
    {
        cmd.Parameters.Add("@code", SqlDbType.NVarChar, 40).Value = code.Trim();
        cmd.Parameters.Add("@name", SqlDbType.NVarChar, 100).Value = name.Trim();
        cmd.Parameters.Add("@cat", SqlDbType.NVarChar, 40).Value = category.Trim();
        var p = cmd.Parameters.Add("@price", SqlDbType.Decimal);
        p.Precision = 10;
        p.Scale = 2;
        p.Value = price;
        var c = cmd.Parameters.Add("@cost", SqlDbType.Decimal);
        c.Precision = 10;
        c.Scale = 2;
        c.Value = unitCost is decimal v && v > 0 ? v : DBNull.Value;
        cmd.Parameters.Add("@reorder", SqlDbType.Int).Value = reorderLevel;
    }

    // ------------------------------------------------------------------ stock in / out

    /// <summary>
    /// Restock: records a new batch and adds its units to the product's total, in one transaction.
    /// Leave the batch number empty to get an automatic one (B20261021-001). Returns the new total and the batch number used.
    /// </summary>
    public static (int Now, string BatchNo) ReceiveBatch(int productId, int quantity, string? batchNo, string? supplier, DateTime received, DateTime? expiry)
    {
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));

        using var conn = Open();
        using var tx = conn.BeginTransaction();

        int now;
        using (var cmd = new SqlCommand(
            "UPDATE dbo.Products SET QtyOnHand = QtyOnHand + @q OUTPUT INSERTED.QtyOnHand WHERE ProductId = @id", conn, tx))
        {
            cmd.Parameters.Add("@q", SqlDbType.Int).Value = quantity;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = productId;
            now = cmd.ExecuteScalar() is int n ? n : throw new InvalidOperationException("That product no longer exists.");
        }

        string number = batchNo?.Trim() ?? "";
        if (number.Length == 0)
        {
            using var count = new SqlCommand("SELECT COUNT(*) FROM dbo.StockBatches WHERE ReceivedDate = @d", conn, tx);
            count.Parameters.Add("@d", SqlDbType.Date).Value = received.Date;
            int n = Convert.ToInt32(count.ExecuteScalar(), CultureInfo.InvariantCulture) + 1;
            number = $"B{received:yyyyMMdd}-{n.ToString("000", CultureInfo.InvariantCulture)}";
        }

        using (var cmd = new SqlCommand(@"
INSERT INTO dbo.StockBatches (ProductId, BatchNo, Supplier, ReceivedDate, ExpiryDate, QtyReceived, QtyRemaining)
VALUES (@p, @no, @sup, @rec, @exp, @q, @q)", conn, tx))
        {
            cmd.Parameters.Add("@p", SqlDbType.Int).Value = productId;
            cmd.Parameters.Add("@no", SqlDbType.NVarChar, 40).Value = number;
            cmd.Parameters.Add("@sup", SqlDbType.NVarChar, 100).Value =
                string.IsNullOrWhiteSpace(supplier) ? DBNull.Value : supplier.Trim();
            cmd.Parameters.Add("@rec", SqlDbType.Date).Value = received.Date;
            cmd.Parameters.Add("@exp", SqlDbType.Date).Value = expiry is DateTime x ? x.Date : DBNull.Value;
            cmd.Parameters.Add("@q", SqlDbType.Int).Value = quantity;
            cmd.ExecuteNonQuery();
        }

        tx.Commit();
        return (now, number);
    }

    /// <summary>
    /// FIFO: takes units out of the oldest batches first. Called by Counter POS inside the sale's own transaction, right after
    /// the product total was reduced. Does nothing if batches were never set up.
    /// </summary>
    internal static void ConsumeFifo(SqlConnection conn, SqlTransaction tx, int productId, int quantity)
    {
        using (var exists = new SqlCommand("SELECT CASE WHEN OBJECT_ID(N'dbo.StockBatches', N'U') IS NULL THEN 0 ELSE 1 END", conn, tx))
        {
            if (Convert.ToInt32(exists.ExecuteScalar(), CultureInfo.InvariantCulture) == 0) return;
        }

        var batches = new List<(int Id, int Qty)>();
        using (var cmd = new SqlCommand(
            "SELECT BatchId, QtyRemaining FROM dbo.StockBatches WHERE ProductId = @p AND QtyRemaining > 0 ORDER BY ReceivedDate, BatchId", conn, tx))
        {
            cmd.Parameters.Add("@p", SqlDbType.Int).Value = productId;
            using var r = cmd.ExecuteReader();
            while (r.Read()) batches.Add((r.GetInt32(0), r.GetInt32(1)));
        }

        int left = quantity;
        foreach (var (id, have) in batches)
        {
            if (left <= 0) break;
            int take = Math.Min(have, left);
            using var cmd = new SqlCommand("UPDATE dbo.StockBatches SET QtyRemaining = QtyRemaining - @t WHERE BatchId = @id", conn, tx);
            cmd.Parameters.Add("@t", SqlDbType.Int).Value = take;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.ExecuteNonQuery();
            left -= take;
        }
    }
}

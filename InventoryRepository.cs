using System.Data;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>Products, stock batches and the FIFO stock-out logic.</summary>
internal static class InventoryRepository
{
    public static readonly string[] Categories =
        { "Pet Food", "Grooming", "Accessories", "Supplies", "Health", "Toys", "Treats" };
    public static readonly string[] Units = { "pcs", "bottle", "pack", "box", "bag", "kg", "liter" };

    public sealed record ProductRow(int Id, string Name, string Category, string Unit, int ReorderLevel, bool Active);
    public sealed record BatchRow(int BatchId, int ProductId, string BatchNo, string? Supplier,
                                  DateTime Received, DateTime? Expiry, int Remaining);

    public static List<ProductRow> GetProducts()
    {
        var list = new List<ProductRow>();
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT ProductId, Name, Category, Unit, ReorderLevel, IsActive FROM dbo.Products ORDER BY Name", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            list.Add(new ProductRow(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                                    reader.GetString(3), reader.GetInt32(4), reader.GetBoolean(5)));
        return list;
    }

    /// <summary>Batches that still have stock, oldest first (the FIFO order).</summary>
    public static List<BatchRow> GetBatches()
    {
        var list = new List<BatchRow>();
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            @"SELECT BatchId, ProductId, BatchNo, Supplier, ReceivedDate, ExpiryDate, QuantityRemaining
              FROM dbo.StockBatches
              WHERE QuantityRemaining > 0
              ORDER BY ReceivedDate, BatchId", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            list.Add(new BatchRow(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2),
                reader.IsDBNull(3) ? null : reader.GetString(3),
                reader.GetDateTime(4),
                reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                reader.GetInt32(6)));
        return list;
    }

    /// <summary>Throws SqlException 2627/2601 if the product name already exists.</summary>
    public static void AddProduct(string name, string category, string unit, int reorderLevel)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "INSERT INTO dbo.Products (Name, Category, Unit, ReorderLevel) VALUES (@n, @c, @u, @r)", conn);
        cmd.Parameters.Add("@n", SqlDbType.NVarChar, 100).Value = name.Trim();
        cmd.Parameters.Add("@c", SqlDbType.NVarChar, 30).Value = category;
        cmd.Parameters.Add("@u", SqlDbType.NVarChar, 20).Value = unit.Trim();
        cmd.Parameters.Add("@r", SqlDbType.Int).Value = reorderLevel;
        cmd.ExecuteNonQuery();
    }

    /// <summary>Creates a new batch. Throws SqlException 2627/2601 if the batch number is already used for this product.</summary>
    public static void StockIn(int productId, string? batchNo, string? supplier, DateTime? expiry, int qty, string actor)
    {
        if (string.IsNullOrWhiteSpace(batchNo))
            batchNo = "B" + DateTime.Now.ToString("yyMMdd-HHmmss");

        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var tx = conn.BeginTransaction();
        try
        {
            int batchId;
            using (var cmd = new SqlCommand(
                @"INSERT INTO dbo.StockBatches
                    (ProductId, BatchNo, Supplier, ReceivedDate, ExpiryDate, QuantityReceived, QuantityRemaining)
                  OUTPUT INSERTED.BatchId
                  VALUES (@p, @b, @s, CAST(GETDATE() AS DATE), @e, @q, @q)", conn, tx))
            {
                cmd.Parameters.Add("@p", SqlDbType.Int).Value = productId;
                cmd.Parameters.Add("@b", SqlDbType.NVarChar, 30).Value = batchNo.Trim();
                cmd.Parameters.Add("@s", SqlDbType.NVarChar, 100).Value =
                    string.IsNullOrWhiteSpace(supplier) ? DBNull.Value : supplier.Trim();
                cmd.Parameters.Add("@e", SqlDbType.Date).Value = (object?)expiry ?? DBNull.Value;
                cmd.Parameters.Add("@q", SqlDbType.Int).Value = qty;
                batchId = (int)cmd.ExecuteScalar()!;
            }

            LogMovement(conn, tx, productId, batchId, "In", qty, "Stock in", actor);
            tx.Commit();
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    /// <summary>
    /// Takes stock from the oldest batches first. Returns Ok = false (and changes nothing)
    /// when there isn't enough stock; Available says how much there is.
    /// </summary>
    public static (bool Ok, int Available) StockOut(int productId, int qty, string? reason, string actor)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var tx = conn.BeginTransaction();
        try
        {
            var batches = new List<(int Id, int Remaining)>();
            using (var cmd = new SqlCommand(
                @"SELECT BatchId, QuantityRemaining
                  FROM dbo.StockBatches WITH (UPDLOCK, ROWLOCK)
                  WHERE ProductId = @p AND QuantityRemaining > 0
                  ORDER BY ReceivedDate, BatchId", conn, tx))
            {
                cmd.Parameters.Add("@p", SqlDbType.Int).Value = productId;
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    batches.Add((reader.GetInt32(0), reader.GetInt32(1)));
            }

            int available = batches.Sum(b => b.Remaining);
            if (available < qty)
            {
                tx.Rollback();
                return (false, available);
            }

            int left = qty;
            foreach (var (id, remaining) in batches)
            {
                if (left == 0) break;
                int take = Math.Min(left, remaining);

                using var upd = new SqlCommand(
                    "UPDATE dbo.StockBatches SET QuantityRemaining = QuantityRemaining - @t WHERE BatchId = @id", conn, tx);
                upd.Parameters.Add("@t", SqlDbType.Int).Value = take;
                upd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                upd.ExecuteNonQuery();

                LogMovement(conn, tx, productId, id, "Out", take,
                    string.IsNullOrWhiteSpace(reason) ? "Stock out" : reason.Trim(), actor);
                left -= take;
            }

            tx.Commit();
            return (true, available);
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    private static void LogMovement(SqlConnection conn, SqlTransaction tx, int productId, int batchId,
                                    string type, int qty, string reason, string actor)
    {
        using var cmd = new SqlCommand(
            @"INSERT INTO dbo.StockMovements (ProductId, BatchId, MovementType, Quantity, Reason, ActorName)
              VALUES (@p, @b, @t, @q, @r, @a)", conn, tx);
        cmd.Parameters.Add("@p", SqlDbType.Int).Value = productId;
        cmd.Parameters.Add("@b", SqlDbType.Int).Value = batchId;
        cmd.Parameters.Add("@t", SqlDbType.NVarChar, 3).Value = type;
        cmd.Parameters.Add("@q", SqlDbType.Int).Value = qty;
        cmd.Parameters.Add("@r", SqlDbType.NVarChar, 200).Value = reason;
        cmd.Parameters.Add("@a", SqlDbType.NVarChar, 100).Value = actor;
        cmd.ExecuteNonQuery();
    }
}
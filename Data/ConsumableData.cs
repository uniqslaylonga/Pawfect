using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>
/// Grooming supplies (dbo.Consumables) kept in batches with expiry dates (dbo.ConsumableBatches), and the amount of each supply that a
/// service uses up (dbo.ServiceConsumables). Set the tables up once with Sql/consumable_batches.sql; until then every method here does nothing.
/// Supplies work like retail stock: earliest-expiry batch first, expired batches are never used.
/// </summary>
internal static class ConsumableData
{
    private static bool TablesExist(SqlConnection conn, SqlTransaction? tx)
    {
        using var cmd = new SqlCommand(
            "SELECT CASE WHEN OBJECT_ID(N'dbo.Consumables', N'U') IS NOT NULL AND OBJECT_ID(N'dbo.ConsumableBatches', N'U') IS NOT NULL " +
            "AND OBJECT_ID(N'dbo.ServiceConsumables', N'U') IS NOT NULL THEN 1 ELSE 0 END", conn, tx);
        return Convert.ToInt32(cmd.ExecuteScalar(), CultureInfo.InvariantCulture) == 1;
    }

    /// <summary>
    /// Uses up the supplies for the services in a sale, inside the sale's own transaction. A supply with no batches yet is not tracked and is skipped.
    /// A supply is short only when it has no batches left in date AND expired batches exist: then <see cref="ExpiredStockException"/> is thrown and
    /// the sale is rolled back. An ordinary shortage (nothing left at all) does not block billing; it only drives the low-stock alert.
    /// </summary>
    internal static void ConsumeForServices(SqlConnection conn, SqlTransaction tx, IEnumerable<(int ServiceId, int Quantity)> services)
    {
        if (!TablesExist(conn, tx)) return;

        var needs = new Dictionary<int, (string Name, decimal Qty)>();
        foreach (var (serviceId, quantity) in services)
        {
            using var cmd = new SqlCommand(@"
SELECT c.ConsumableId, c.Name, sc.QtyPerService
FROM dbo.Services s
JOIN dbo.ServiceConsumables sc ON sc.ServiceName = s.Name
JOIN dbo.Consumables c ON c.ItemCode = sc.ConsumableCode AND c.IsActive = 1
WHERE s.ServiceId = @s", conn, tx);
            cmd.Parameters.Add("@s", SqlDbType.Int).Value = serviceId;
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                int id = r.GetInt32(0);
                decimal amount = r.GetDecimal(2) * quantity;
                needs[id] = needs.TryGetValue(id, out var have) ? (have.Name, have.Qty + amount) : (r.GetString(1), amount);
            }
        }

        foreach (var pair in needs)
            Consume(conn, tx, pair.Key, pair.Value.Name, pair.Value.Qty);
    }

    private static void Consume(SqlConnection conn, SqlTransaction tx, int consumableId, string name, decimal needed)
    {
        var today = DateTime.Today;
        var usable = new List<(int Id, decimal Qty)>();   // batches still in date, in the order they will be used
        decimal expired = 0;
        bool anyBatch = false;

        using (var cmd = new SqlCommand(
            "SELECT BatchId, QtyRemaining, ExpiryDate FROM dbo.ConsumableBatches WHERE ConsumableId = @c " +
            "ORDER BY CASE WHEN ExpiryDate IS NULL THEN 1 ELSE 0 END, ExpiryDate, ReceivedDate, BatchId", conn, tx))
        {
            cmd.Parameters.Add("@c", SqlDbType.Int).Value = consumableId;
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                anyBatch = true;
                decimal qty = r.GetDecimal(1);
                if (qty <= 0) continue;
                if (!r.IsDBNull(2) && r.GetDateTime(2).Date < today) expired += qty;
                else usable.Add((r.GetInt32(0), qty));
            }
        }

        if (!anyBatch) return;   // never received in batches: not tracked yet

        decimal available = usable.Sum(b => b.Qty);
        if (available < needed && expired > 0)
            throw new ExpiredStockException(name, expired, available, needed, isSupply: true);

        decimal left = needed;
        foreach (var (id, have) in usable)
        {
            if (left <= 0) break;
            decimal take = Math.Min(have, left);
            using var cmd = new SqlCommand("UPDATE dbo.ConsumableBatches SET QtyRemaining = QtyRemaining - @t WHERE BatchId = @id", conn, tx);
            cmd.Parameters.Add(new SqlParameter("@t", SqlDbType.Decimal) { Precision = 10, Scale = 3, Value = take });
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.ExecuteNonQuery();
            left -= take;
        }

        SyncQtyOnHand(conn, tx, consumableId);
    }

    /// <summary>Consumables.QtyOnHand (what the low-stock alerts read) = all batch units, rounded up to a whole unit.</summary>
    private static void SyncQtyOnHand(SqlConnection conn, SqlTransaction? tx, int consumableId)
    {
        using var cmd = new SqlCommand(@"
UPDATE c SET QtyOnHand = CAST(CEILING(ISNULL((SELECT SUM(b.QtyRemaining) FROM dbo.ConsumableBatches b WHERE b.ConsumableId = c.ConsumableId), 0)) AS INT)
FROM dbo.Consumables c WHERE c.ConsumableId = @c", conn, tx);
        cmd.Parameters.Add("@c", SqlDbType.Int).Value = consumableId;
        cmd.ExecuteNonQuery();
    }

    /// <summary>Receives a new batch of a supply (for a restock screen). Quantity is in the supply's own unit, e.g. bottles.</summary>
    internal static void AddBatch(int consumableId, string batchNo, string? supplier, DateTime received, DateTime? expiry, decimal quantity)
    {
        if (quantity <= 0) throw new InvalidOperationException("Enter a quantity above zero.");
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var tx = conn.BeginTransaction();
        if (!TablesExist(conn, tx)) throw new InvalidOperationException("Run Sql/consumable_batches.sql first.");

        using (var cmd = new SqlCommand(@"
INSERT INTO dbo.ConsumableBatches (ConsumableId, BatchNo, Supplier, ReceivedDate, ExpiryDate, QtyReceived, QtyRemaining)
VALUES (@c, @no, @sup, @rec, @exp, @q, @q)", conn, tx))
        {
            cmd.Parameters.Add("@c", SqlDbType.Int).Value = consumableId;
            cmd.Parameters.Add("@no", SqlDbType.NVarChar, 40).Value = batchNo.Trim();
            cmd.Parameters.Add("@sup", SqlDbType.NVarChar, 100).Value = string.IsNullOrWhiteSpace(supplier) ? DBNull.Value : supplier.Trim();
            cmd.Parameters.Add("@rec", SqlDbType.Date).Value = received.Date;
            cmd.Parameters.Add("@exp", SqlDbType.Date).Value = expiry is { } e ? e.Date : DBNull.Value;
            cmd.Parameters.Add(new SqlParameter("@q", SqlDbType.Decimal) { Precision = 10, Scale = 3, Value = quantity });
            cmd.ExecuteNonQuery();
        }
        SyncQtyOnHand(conn, tx, consumableId);
        tx.Commit();
    }
}

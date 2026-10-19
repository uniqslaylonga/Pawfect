using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>Read-only queries for the Reports page.</summary>
internal static class ReportRepository
{
    public sealed record MovementRow(DateTime When, string Type, string Product, string Unit,
                                     int Quantity, string? BatchNo, string Actor, string? Reason);

    public static List<MovementRow> GetMovements(int top = 500)
    {
        var list = new List<MovementRow>();

        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            @"SELECT TOP (@top) m.CreatedAt, m.MovementType, p.Name, p.Unit, m.Quantity, b.BatchNo, m.ActorName, m.Reason
              FROM dbo.StockMovements m
              JOIN dbo.Products p ON p.ProductId = m.ProductId
              LEFT JOIN dbo.StockBatches b ON b.BatchId = m.BatchId
              ORDER BY m.MovementId DESC", conn);
        cmd.Parameters.Add("@top", System.Data.SqlDbType.Int).Value = top;
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            list.Add(new MovementRow(reader.GetDateTime(0).ToLocalTime(), reader.GetString(1), reader.GetString(2),
                reader.GetString(3), reader.GetInt32(4),
                reader.IsDBNull(5) ? null : reader.GetString(5),
                reader.GetString(6),
                reader.IsDBNull(7) ? null : reader.GetString(7)));
        return list;
    }
}
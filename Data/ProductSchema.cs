using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>
/// Makes sure dbo.Products has the base columns the app reads (Code, Name, Category, Price, IsActive).
/// A database built from an older or different script may lack some of them, which showed up as
/// "Invalid column name 'Code'". Missing columns are added and filled in, using a similarly named
/// column (ProductCode, SKU, ProductName...) when there is one. Existing data is never changed or removed.
/// </summary>
internal static class ProductSchema
{
    private static bool _done;

    private static readonly (string Column, string[] Alternates, string Definition)[] Wanted =
    {
        ("Code",     new[] { "ProductCode", "ItemCode", "SKU", "Sku", "Barcode" }, "NVARCHAR(40) NULL"),
        ("Name",     new[] { "ProductName", "ItemName", "Description" },           "NVARCHAR(100) NULL"),
        ("Category", new[] { "CategoryName", "ProductCategory", "Type" },          "NVARCHAR(40) NULL"),
        ("Price",    new[] { "UnitPrice", "SellingPrice", "SalePrice", "RetailPrice" }, "DECIMAL(10,2) NULL"),
        ("IsActive", new[] { "Active", "Enabled" },                                "BIT NULL"),
    };

    public static void Ensure(SqlConnection conn)
    {
        if (_done) return;
        try
        {
            if (!TableExists(conn)) return;   // the table is created by Sql/employee_setup.sql

            foreach (var (column, alternates, definition) in Wanted)
            {
                if (ColumnExists(conn, column)) continue;

                Run(conn, $"ALTER TABLE dbo.Products ADD [{column}] {definition};");

                string? source = alternates.FirstOrDefault(a => ColumnExists(conn, a));
                string fill = column switch
                {
                    "Code"     => source != null
                                    ? $"CONVERT(NVARCHAR(40), [{source}])"
                                    : "N'P' + RIGHT(N'0000' + CONVERT(NVARCHAR(20), ProductId), 4)",
                    "Name"     => source != null ? $"CONVERT(NVARCHAR(100), [{source}])" : "N'Product ' + CONVERT(NVARCHAR(20), ProductId)",
                    "Category" => source != null ? $"CONVERT(NVARCHAR(40), [{source}])"  : "N''",
                    "Price"    => source != null ? $"TRY_CONVERT(DECIMAL(10,2), [{source}])" : "0",
                    _          => source != null ? $"CONVERT(BIT, [{source}])" : "1",
                };
                // dynamic SQL: the column did not exist when this batch was compiled
                string value = "ISNULL(" + fill + ", " + Fallback(column) + ")";
                string update = "UPDATE dbo.Products SET [" + column + "] = " + value + ";";
                Run(conn, "EXEC(N'" + update.Replace("'", "''") + "');");
                string notNull = "ALTER TABLE dbo.Products ALTER COLUMN [" + column + "] " + definition.Replace(" NULL", " NOT NULL") + ";";
                Run(conn, "EXEC(N'" + notNull + "');");
            }

            // Two rows must not share a Code (e.g. a SKU column with blanks): make the duplicates unique.
            if (ColumnExists(conn, "Code"))
                Run(conn, @"
WITH d AS (SELECT ProductId, Code, ROW_NUMBER() OVER (PARTITION BY Code ORDER BY ProductId) AS rn FROM dbo.Products)
UPDATE p SET p.Code = LEFT(p.Code, 30) + N'-' + CONVERT(NVARCHAR(10), p.ProductId)
FROM dbo.Products p JOIN d ON d.ProductId = p.ProductId WHERE d.rn > 1;");

            _done = true;
        }
        catch (SqlException)
        {
            // no permission to alter the table: the real error shows up in the query that needs the column
        }
    }

    private static string Fallback(string column) => column switch
    {
        "Code" => "N''", "Name" => "N''", "Category" => "N''", "Price" => "0", _ => "1",
    };

    private static bool TableExists(SqlConnection conn)
    {
        using var cmd = new SqlCommand("SELECT CASE WHEN OBJECT_ID(N'dbo.Products', N'U') IS NULL THEN 0 ELSE 1 END", conn);
        return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
    }

    private static bool ColumnExists(SqlConnection conn, string column)
    {
        using var cmd = new SqlCommand("SELECT CASE WHEN COL_LENGTH(N'dbo.Products', @c) IS NULL THEN 0 ELSE 1 END", conn);
        cmd.Parameters.AddWithValue("@c", column);
        return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
    }

    private static void Run(SqlConnection conn, string sql)
    {
        using var cmd = new SqlCommand(sql, conn);
        cmd.ExecuteNonQuery();
    }
}

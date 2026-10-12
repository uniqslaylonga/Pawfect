using Microsoft.Data.SqlClient;

namespace MyApp;

<<<<<<< Updated upstream:AdminStats.cs
internal static class AdminStats
=======
internal static class StaffStats
>>>>>>> Stashed changes:DashboardStats.cs
{
    public static int CountActiveStaff()
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT COUNT(1) FROM dbo.Users WHERE IsActive = 1 AND Role <> 'Admin'", conn);
        return (int)cmd.ExecuteScalar()!;
    }
}
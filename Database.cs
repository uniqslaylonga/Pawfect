namespace MyApp;

internal static class Database
{
    // The password of the "pawfect" SQL login that Setup-Server.ps1 creates. Change both together if you change it.
    private const string ServerLogin = "pawfect";
    private const string ServerPassword = "Pawfect@123";

    private const string LocalDb =
        "Server=(localdb)\\MSSQLLocalDB;Database=Pawfect;Integrated Security=True;TrustServerCertificate=True;";

    /// <summary>
    /// Where the data lives. If a file called server.txt sits next to the program, its content decides:
    ///   - an address such as 192.168.1.10  -> connect to the shared SQL Server on that PC (same data on every PC)
    ///   - a full connection string (contains "=") -> used as is
    /// With no server.txt, the program uses this PC's own LocalDB, exactly as before.
    /// </summary>
    public static string ConnectionString { get; } = Build();

    private static string Build()
    {
        try
        {
            string file = Path.Combine(AppContext.BaseDirectory, "server.txt");
            if (File.Exists(file))
            {
                string text = File.ReadAllText(file).Trim();
                if (text.Length > 0)
                {
                    if (text.Contains('=')) return text;
                    string host = text.Contains(',') ? text : text + ",1433";
                    return $"Server={host};Database=Pawfect;User Id={ServerLogin};Password={ServerPassword};" +
                           "TrustServerCertificate=True;Connect Timeout=5;";
                }
            }
        }
        catch (IOException) { }
        catch (UnauthorizedAccessException) { }
        return LocalDb;
    }
}

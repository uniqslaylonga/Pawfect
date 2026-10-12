namespace MyApp;

internal static class Database
{
    // Edit this ONE line to match your SQL Server (see the setup steps).
    //   SQL Server Express : Server=localhost\SQLEXPRESS;Database=Pawfect;Integrated Security=True;TrustServerCertificate=True;
    //   LocalDB            : Server=(localdb)\MSSQLLocalDB;Database=Pawfect;Integrated Security=True;TrustServerCertificate=True;
    //   SQL login          : Server=localhost\SQLEXPRESS;Database=Pawfect;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;
    public const string ConnectionString =
        "Server=(localdb)\\MSSQLLocalDB;Database=Pawfect;Integrated Security=True;TrustServerCertificate=True;";
}
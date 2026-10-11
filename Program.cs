using Microsoft.Data.SqlClient;

namespace MyApp;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        // First run: the Users table has no admin yet, so create one before showing the login.
        try
        {
            if (!Auth.AnyAdminExists())
            {
                using var setup = new CreateAdminForm();
                if (setup.ShowDialog() != DialogResult.OK)
                    return;
            }
        }
        catch (SqlException ex)
        {
            MessageBox.Show(
                "Can't connect to the database.\n\nCheck the connection string in Database.cs and make sure you ran Sql/setup.sql.\n\n" + ex.Message,
                "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        while (true)
        {
            using var login = new LoginForm();
            if (login.ShowDialog() != DialogResult.OK || login.SignedInUser is null)
                break;

            using var main = new MainForm();
            main.ApplyUser(login.SignedInUser);
            Application.Run(main);

            if (!main.LoggedOut)
                break;
        }
    }
}

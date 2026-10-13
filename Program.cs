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
            // Landing screen first; closing it (Esc / X) quits the app.
            using (var welcome = new WelcomeForm())
            {
                if (welcome.ShowDialog() != DialogResult.OK)
                    break;
            }

            using var login = new LoginForm();
            if (login.ShowDialog() != DialogResult.OK || login.SignedInUser is not { } user)
                continue; // closed the login: back to the landing screen

            // Admins land on the admin dashboard; everyone else gets the staff dashboard.
            bool loggedOut;
            if (user.Role == UserRole.Admin)
            {
                using var admin = new AdminForm();
                admin.ApplyUser(user);
                Application.Run(admin);
                loggedOut = admin.LoggedOut;
            }
            else if (user.Role == UserRole.Customer)
            {
                using var portal = new CustomerPortalForm(user);
                Application.Run(portal);
                loggedOut = portal.LoggedOut;
            }
            else
            {
                using var main = new MainForm();
                main.ApplyUser(user);
                Application.Run(main);
                loggedOut = main.LoggedOut;
            }

            if (!loggedOut)
                break;
        }
    }
}

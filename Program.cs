using Microsoft.Data.SqlClient;

namespace MyApp;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        // Any crash at start-up or later is shown (and saved to crash.log) instead of the app silently closing.
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (_, e) => ReportCrash(e.Exception);
        AppDomain.CurrentDomain.UnhandledException += (_, e) => ReportCrash(e.ExceptionObject as Exception);

        try { Run(); }
        catch (Exception ex) { ReportCrash(ex); }
    }

    private static void ReportCrash(Exception? ex)
    {
        string text = ex?.ToString() ?? "Unknown error";
        try { File.WriteAllText(Path.Combine(AppContext.BaseDirectory, "crash.log"), DateTime.Now + "\n\n" + text); } catch { /* best effort */ }
        MessageBox.Show(text, "Pawfect crashed", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private static void Run()
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
            bool isNewCustomer;
            using (var welcome = new WelcomeForm())
            {
                if (welcome.ShowDialog() != DialogResult.OK)
                    break;
                isNewCustomer = welcome.IsNewCustomer;
            }

            // "I'm New Here": no login, straight to step 1 (Contact) of the Add Pet flow.
            // (Closing it, or finishing the queue ticket, comes back to the landing screen.)
            if (isNewCustomer)
            {
                CustomerFlow.RunNewCustomer();
                continue;
            }

            using var login = new LoginForm();
            if (login.ShowDialog() != DialogResult.OK || login.SignedInUser is not { } user)
                continue; // closed the login: back to the landing screen

            // Admins land on the admin dashboard; staff get the staff dashboard (customers were handled above).
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
                // Existing customers have no dashboard: after logging in they go straight to step 2 (Select a Pet).
                CustomerFlow.RunExistingCustomer(user);
                continue;
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
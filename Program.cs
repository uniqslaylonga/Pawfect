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
            bool wantsLogin;
            using (var welcome = new WelcomeForm())
            {
                if (welcome.ShowDialog() != DialogResult.OK)
                    break;
                wantsLogin = welcome.WantsLogin;
            }

            // Start: a walk-in customer at the kiosk goes straight to step 1 (contact information), no login.
            if (!wantsLogin)
            {
                CustomerFlow.RunKiosk();
                continue;   // back to the landing screen for the next customer
            }

            using var login = new LoginForm();
            bool loginOk = login.ShowDialog() == DialogResult.OK;
            if (loginOk && login.WantsKiosk)
            {
                CustomerFlow.RunKiosk();   // "Start" on the login screen: same as Start on the landing screen
                continue;
            }
            if (!loginOk || login.SignedInUser is not { } user)
                continue; // closed the login: back to the landing screen

            // Admins land on the admin dashboard, customers go straight to step 2 (Select a Pet) of the Add Pet flow, everyone else gets the staff dashboard.
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
                // No customer dashboard: straight to step 2 of the Add Pet flow, then back to the landing screen.
                CustomerFlow.Run(user);
                loggedOut = true;
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
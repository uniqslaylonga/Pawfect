namespace MyApp;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        while (true)
        {
            using var login = new LoginForm();
            if (login.ShowDialog() != DialogResult.OK)
                break;

            using var main = new MainForm();
            Application.Run(main);

            if (!main.LoggedOut)
                break;
        }
    }
}

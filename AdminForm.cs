using MyApp.Controls;

namespace MyApp;

/// <summary>
/// The admin dashboard. Layout lives in AdminForm.Designer.cs, so open this form in the Visual Studio
/// designer to move or restyle anything. Every number, chart and list starts empty: nothing here is sample data.
/// </summary>
internal partial class AdminForm : Form
{
    /// <summary>Set when the admin picks "Log Out", so Program.cs can show the login again.</summary>
    public bool LoggedOut { get; private set; }

    /// <summary>The signed-in account. Set by <see cref="ApplyUser"/> right after the login succeeds.</summary>
    public UserAccount? CurrentUser { get; private set; }

    public AdminForm()
    {
        InitializeComponent();
        DoubleBuffered = true;

        // Empty grey ring until real appointment / retail numbers are fed in.
        splitDonut.SetData(Array.Empty<(double, Color)>(), "0%", "Grooming");

        // Daily / Weekly / Monthly switch for the revenue summary (selection only, until data is wired up).
        SelectOne(grossDailyTab, grossDailyTab, grossWeeklyTab, grossMonthlyTab);
        foreach (var tab in new[] { grossDailyTab, grossWeeklyTab, grossMonthlyTab })
        {
            var current = tab;
            current.Click += (_, _) => SelectOne(current, grossDailyTab, grossWeeklyTab, grossMonthlyTab);
        }
    }

    /// <summary>Shows who is signed in (name + role) and greets them by first name.</summary>
    public void ApplyUser(UserAccount user)
    {
        CurrentUser = user;
        userNameLabel.Text = user.Name;
        userRoleLabel.Text = user.Role.ToString();

        string first = user.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? user.Name;
        pageSubtitle.Text = $"Welcome back, {first}! Here's what's happening today.";
    }

    private static void SelectOne(TabPill chosen, params TabPill[] group)
    {
        foreach (var tab in group)
            tab.Active = ReferenceEquals(tab, chosen);
    }

    private void logoutNav_Click(object? sender, EventArgs e)
    {
        LoggedOut = true;
        Close();
    }
}

using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// The admin's notification bell. It lists the same things as the dashboard Alerts card (out of stock, low stock,
/// requests waiting, voided/refunded sales) and shows the red dot while there is anything to look at.
/// </summary>
internal partial class AdminForm
{
    private List<DashAlert> _bellAlerts = new();
    private System.Windows.Forms.Timer? _bellTimer;

    /// <summary>Called once from the constructor: wires the click and starts the quiet background check.</summary>
    private void InitNotifications()
    {
        notificationButton.Click += (_, _) => ShowNotifications();

        _bellTimer = new System.Windows.Forms.Timer { Interval = 10_000 };
        _bellTimer.Tick += (_, _) => RefreshBell();
        Disposed += (_, _) => _bellTimer.Dispose();
        Shown += (_, _) => { RefreshBell(); _bellTimer.Start(); };
    }

    /// <summary>Quiet check: updates the red dot, never shows an error, and waits while a dialog is open.</summary>
    private void RefreshBell()
    {
        if (CurrentUser is null) return;
        if (Application.OpenForms.Cast<Form>().Any(f => f != this && f.Visible && f.Modal)) return;

        try { SetBell(AdminStats.GetAlerts()); }
        catch (SqlException) { }
    }

    private void SetBell(List<DashAlert> alerts)
    {
        _bellAlerts = alerts;
        notificationButton.ShowDot = alerts.Count > 0;
    }

    private void ShowNotifications()
    {
        // Fresh numbers every time the bell is opened.
        try { SetBell(AdminStats.GetAlerts()); }
        catch (SqlException) { }

        const int width = 340;
        var menu = PawMenu.Create(width);

        if (_bellAlerts.Count == 0)
        {
            menu.Items.Add(new PawMenuItem(MenuKind.Header, "Notifications") { MenuWidth = width - 12, SubText = "Nothing needs your attention" });
            menu.AddDivider();
            menu.Items.Add(new PawMenuItem(MenuKind.Empty, "You're all caught up") { MenuWidth = width - 12, SubText = "No stock, request or sales alerts." });
        }
        else
        {
            menu.Items.Add(new PawMenuItem(MenuKind.Header, "Notifications")
            {
                MenuWidth = width - 12,
                SubText = _bellAlerts.Count == 1 ? "1 thing needs your attention" : $"{_bellAlerts.Count} things need your attention",
                Badge = _bellAlerts.Count.ToString(System.Globalization.CultureInfo.CurrentCulture),
            });
            menu.AddDivider();

            foreach (var a in _bellAlerts.Take(7))
            {
                var (glyph, tone, page) = a.Kind switch
                {
                    AlertKind.OutOfStock => ("", MenuTone.Danger, AdminPage.Inventory),
                    AlertKind.LowStock => ("", MenuTone.Warning, AdminPage.Inventory),
                    AlertKind.Waiting => ("", MenuTone.Primary, AdminPage.Dashboard),
                    _ => ("", MenuTone.Neutral, AdminPage.Reports),
                };
                var row = new PawMenuItem(MenuKind.Action, a.Title)
                {
                    MenuWidth = width - 12,
                    Glyph = glyph,
                    Tone = tone,
                    SubText = a.Detail,
                };
                var target = page;
                row.Click += (_, _) => BeginInvoke(() => ShowPage(target));
                menu.Items.Add(row);
            }

            if (_bellAlerts.Count > 7)
                menu.Items.Add(new PawMenuItem(MenuKind.Caption, $"+ {_bellAlerts.Count - 7} more") { MenuWidth = width - 12 });

            menu.AddDivider();
            var open = new PawMenuItem(MenuKind.Button, "Open dashboard") { MenuWidth = width - 12 };
            open.Click += (_, _) => BeginInvoke(() => ShowPage(AdminPage.Dashboard));
            menu.Items.Add(open);
        }

        // Right-align the dropdown with the bell so it doesn't run off the edge of the window.
        menu.Show(notificationButton, new Point(notificationButton.Width - width, notificationButton.Height + 6));
    }
}

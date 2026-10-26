using System.Globalization;
using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

internal enum StaffAlertKind { OutOfStock, Expired, LowStock, NearExpiry, Approval }

/// <summary>One line of the staff dashboard Alerts card. Approval alerts carry the appointment that is waiting to be confirmed.</summary>
internal sealed record StaffAlert(StaffAlertKind Kind, string Title, string Detail, string Tag, AppointmentItem? Appointment = null);

/// <summary>
/// Everything the staff dashboard Alerts card (and the bell) lists: items with no stock, low stock, batches that are
/// expired or near expiry, and appointment requests waiting for approval. Read-only; staff can't change stock.
/// </summary>
internal static class StaffAlerts
{
    /// <summary>Out of stock, expired, low stock and near-expiry alerts. Parts of the database that were never set up are left out.</summary>
    public static List<StaffAlert> GetStockAlerts()
    {
        var list = new List<StaffAlert>();
        var today = DateTime.Today;
        var culture = CultureInfo.CurrentCulture;

        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        TryRead(conn, @"SELECT Name, QtyOnHand, ReorderLevel FROM dbo.Products
                        WHERE IsActive = 1 AND (QtyOnHand <= 0 OR (ReorderLevel > 0 AND QtyOnHand <= ReorderLevel))
                        ORDER BY QtyOnHand, Name", null, r =>
        {
            string name = r.GetString(0);
            int qty = r.GetInt32(1), level = r.GetInt32(2);
            list.Add(qty <= 0
                ? new StaffAlert(StaffAlertKind.OutOfStock, name, "No stock left" + (level > 0 ? $" (reorder level {level})" : ""), "Out of stock")
                : new StaffAlert(StaffAlertKind.LowStock, name, $"Low stock: {qty} left (reorder at {level})", "Low stock"));
        });

        TryRead(conn, @"SELECT Name, QtyOnHand, MinStock, Unit FROM dbo.Consumables
                        WHERE IsActive = 1 AND MinStock > 0 AND QtyOnHand <= MinStock ORDER BY QtyOnHand, Name", null, r =>
        {
            string name = r.GetString(0), unit = r.GetString(3);
            int qty = r.GetInt32(1), min = r.GetInt32(2);
            list.Add(qty <= 0
                ? new StaffAlert(StaffAlertKind.OutOfStock, name, $"No stock left (minimum {min} {unit})", "Out of stock")
                : new StaffAlert(StaffAlertKind.LowStock, name, $"Low stock: {qty} {unit} left (minimum {min})", "Low stock"));
        });

        TryRead(conn, @"SELECT p.Name, b.BatchNo, b.ExpiryDate, b.QtyRemaining
                        FROM dbo.StockBatches b JOIN dbo.Products p ON p.ProductId = b.ProductId
                        WHERE p.IsActive = 1 AND b.QtyRemaining > 0 AND b.ExpiryDate IS NOT NULL AND b.ExpiryDate <= @limit
                        ORDER BY b.ExpiryDate, p.Name", today.AddDays(InventoryData.NearExpiryDays), r =>
        {
            string name = r.GetString(0), batch = r.IsDBNull(1) ? "" : r.GetString(1);
            var expiry = r.GetDateTime(2).Date;
            int qty = r.GetInt32(3);
            string units = qty == 1 ? "1 unit" : $"{qty} units";
            string batchText = batch.Length > 0 ? $"Batch {batch}" : "Batch";
            if (expiry < today)
                list.Add(new StaffAlert(StaffAlertKind.Expired, name,
                    $"{batchText} expired {expiry.ToString("MMM d", culture)}  \u2022  {units}", "Expired"));
            else
            {
                int days = (expiry - today).Days;
                string left = days == 0 ? "today" : days == 1 ? "tomorrow" : $"in {days} days";
                list.Add(new StaffAlert(StaffAlertKind.NearExpiry, name,
                    $"{batchText} expires {left} ({expiry.ToString("MMM d", culture)})  \u2022  {units}", "Near expiry"));
            }
        });

        return list;
    }

    /// <summary>Stock alerts first (no stock, expired, low stock, near expiry), then the appointment requests waiting for approval.</summary>
    public static List<StaffAlert> Combine(IEnumerable<StaffAlert> stock, IEnumerable<AppointmentItem> waiting)
        => stock.Concat(waiting.Select(a => new StaffAlert(StaffAlertKind.Approval,
                $"{a.CustomerName} - {a.PetName}", $"{a.Service}  \u2022  {When(a.Start)}", "Approval", a)))
            .OrderBy(a => a.Kind).ToList();

    /// <summary>Changes only when the list of alerts changes, so the card isn't rebuilt (and scrolled back to the top) every poll.</summary>
    public static string Signature(IEnumerable<StaffAlert> alerts)
        => string.Join("|", alerts.Select(a => $"{a.Kind}:{a.Title}:{a.Detail}:{a.Appointment?.Id}:{a.Appointment?.Status}:{a.Appointment?.Start.Ticks}:{a.Appointment?.Groomer}:{a.Appointment?.Room}:{a.Appointment?.Notes}"));

    private static string When(DateTime start)
        => start.Date == DateTime.Today
            ? start.ToString("h:mm tt", CultureInfo.CurrentCulture)
            : start.ToString("MMM d, h:mm tt", CultureInfo.CurrentCulture);

    /// <summary>Runs a query, but treats "table or column doesn't exist" as "nothing to report".</summary>
    private static void TryRead(SqlConnection conn, string sql, DateTime? limit, Action<SqlDataReader> each)
    {
        try
        {
            using var cmd = new SqlCommand(sql, conn);
            if (limit is { } d) cmd.Parameters.Add("@limit", System.Data.SqlDbType.Date).Value = d;
            using var r = cmd.ExecuteReader();
            while (r.Read()) each(r);
        }
        catch (SqlException ex) when (ex.Number is 207 or 208)
        {
            // that part of the database isn't set up here
        }
    }
}

/// <summary>The window behind the Alerts card's "View All": every alert in a scrolling list; clicking one picks it.</summary>
internal sealed class AlertsForm : Form
{
    /// <summary>The alert that was clicked, or null if the window was just closed.</summary>
    public StaffAlert? Picked { get; private set; }

    public AlertsForm(IReadOnlyList<StaffAlert> alerts)
    {
        Icon = AppIcon.Value;
        Text = "Alerts";
        Font = Theme.Face(9f);
        BackColor = Color.White;
        ForeColor = Theme.Text;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(480, 560);
        MinimumSize = new Size(400, 360);
        ShowInTaskbar = false;

        var title = new Label
        {
            Dock = DockStyle.Top,
            Height = 52,
            Padding = new Padding(16, 0, 16, 0),
            TextAlign = ContentAlignment.MiddleLeft,
            Font = Theme.Face(11f, FontStyle.Bold),
            Text = alerts.Count == 1 ? "1 alert" : $"{alerts.Count} alerts",
            UseMnemonic = false,
        };

        var list = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White, Padding = new Padding(12, 0, 12, 0) };
        // Rows are docked to the top, so add them last-to-first to keep the first alert on top.
        for (int i = alerts.Count - 1; i >= 0; i--)
        {
            var alert = alerts[i];
            var row = new AlertRowView(alert) { Dock = DockStyle.Top };
            row.Click += (_, _) =>
            {
                Picked = alert;
                DialogResult = DialogResult.OK;
                Close();
            };
            list.Controls.Add(row);
        }

        var close = new Button
        {
            Text = "Close",
            Size = new Size(90, 34),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            ForeColor = Theme.Text,
            Cursor = Cursors.Hand,
            DialogResult = DialogResult.Cancel,
        };
        close.FlatAppearance.BorderColor = Theme.Border;
        var bar = new Panel { Dock = DockStyle.Bottom, Height = 54, Padding = new Padding(12) };
        close.Dock = DockStyle.Right;
        bar.Controls.Add(close);

        Controls.Add(list);
        Controls.Add(bar);
        Controls.Add(title);
        CancelButton = close;
    }
}

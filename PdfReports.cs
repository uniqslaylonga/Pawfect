using System.Globalization;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MyApp;

/// <summary>
/// Builds the PDF exports (customer list, pet records, daily sales) with QuestPDF.
/// Every report shares one layout: brand header, summary cards, a striped table and a page-numbered footer.
/// Colours come from <see cref="Theme"/> so the PDFs always match the app.
/// </summary>
internal static class PdfReports
{
    static PdfReports()
    {
        // QuestPDF Community licence (free for students, non-profits and companies under US$1M revenue).
        QuestPDF.Settings.License = LicenseType.Community;
    }

    private static string Hex(System.Drawing.Color c) => $"#{c.R:X2}{c.G:X2}{c.B:X2}";

    private static readonly string Primary = Hex(Theme.Primary);
    private static readonly string PrimarySoft = Hex(Theme.PrimarySoft);
    private static readonly string TextDark = Hex(Theme.Text);
    private static readonly string TextSoft = Hex(Theme.TextSoft);
    private static readonly string Muted = Hex(Theme.Muted);
    private static readonly string Subtle = Hex(Theme.Subtle);
    private static readonly string Line = Hex(Theme.Border);

    private readonly record struct Col(string Header, float Weight, bool Right = false);
    private sealed record Cx(string Text, string? Color = null, bool Bold = false);

    // ------------------------------------------------------------------ public reports

    public static void Customers(string path, IReadOnlyList<CustomerRecord> customers, string preparedBy)
    {
        var rows = customers.OrderBy(c => c.Name, StringComparer.CurrentCultureIgnoreCase).ToList();
        var recent = rows.Count(c => c.LastVisit is { } v && v.Date >= DateTime.Today.AddDays(-30));

        Build(path, "Customer List", "All registered customers", preparedBy, landscape: true,
            new[]
            {
                ("Total Customers", rows.Count.ToString("N0")),
                ("Registered Pets", rows.Sum(c => c.PetCount).ToString("N0")),
                ("Visited in Last 30 Days", recent.ToString("N0")),
            },
            body => body.Item().Element(c => DataTable(c,
                new[]
                {
                    new Col("Name", 2f), new Col("Phone", 1.4f), new Col("Email", 2.4f), new Col("Address", 3f),
                    new Col("Pets", 2f), new Col("Last Visit", 1.3f), new Col("Added", 1.3f),
                },
                rows.Select(r => new[]
                {
                    new Cx(r.Name, Bold: true), new Cx(r.Phone), new Cx(r.Email), new Cx(r.Address), new Cx(r.PetNames),
                    new Cx(r.LastVisit?.ToString("MMM d, yyyy", CultureInfo.CurrentCulture) ?? ""),
                    new Cx(r.CreatedAt.ToString("MMM d, yyyy", CultureInfo.CurrentCulture)),
                }).ToList())));
    }

    public static void Pets(string path, IReadOnlyList<PetRecord> pets, string preparedBy)
    {
        var rows = pets.OrderBy(p => p.Name, StringComparer.CurrentCultureIgnoreCase).ToList();
        int Count(string species) => rows.Count(p => string.Equals(p.Species, species, StringComparison.OrdinalIgnoreCase));

        Build(path, "Pet Records", "All registered pets and their owners", preparedBy, landscape: false,
            new[]
            {
                ("Total Pets", rows.Count.ToString("N0")),
                ("Dogs", Count("Dog").ToString("N0")),
                ("Cats", Count("Cat").ToString("N0")),
                ("Owners", rows.Select(p => p.CustomerId).Distinct().Count().ToString("N0")),
            },
            body => body.Item().Element(c => DataTable(c,
                new[]
                {
                    new Col("Pet", 2f), new Col("Species", 1.2f), new Col("Breed", 2f),
                    new Col("Owner", 2.2f), new Col("Owner Phone", 1.6f),
                },
                rows.Select(r => new[]
                {
                    new Cx(r.Name, Bold: true), new Cx(r.Species), new Cx(r.Breed), new Cx(r.OwnerName), new Cx(r.OwnerPhone),
                }).ToList())));
    }

    public static void DailySales(string path, IReadOnlyList<TransactionItem> sales, string preparedBy)
    {
        var rows = sales.OrderBy(s => s.When).ToList();
        var done = rows.Where(s => s.Status == TransactionStatus.Completed).ToList();
        decimal total = done.Sum(s => s.Total);
        string Peso(decimal v) => "\u20B1" + v.ToString("N2", CultureInfo.InvariantCulture);

        var byMethod = done.GroupBy(s => s.PaymentMethod)
                           .Select(g => $"{g.Key}: {Peso(g.Sum(s => s.Total))}")
                           .ToList();

        Build(path, "Daily Sales Report", DateTime.Today.ToString("dddd, MMMM d, yyyy", CultureInfo.CurrentCulture), preparedBy, landscape: false,
            new[]
            {
                ("Transactions", rows.Count.ToString("N0")),
                ("Completed Sales", Peso(total)),
                ("Services Sold", done.Sum(s => s.ServiceCount).ToString("N0")),
                ("Items Sold", done.Sum(s => s.ItemCount).ToString("N0")),
            },
            body =>
            {
                body.Item().Element(c => DataTable(c,
                    new[]
                    {
                        new Col("Receipt", 1.6f), new Col("Time", 0.9f), new Col("Customer", 2f),
                        new Col("Services", 1.1f, Right: true), new Col("Items", 0.9f, Right: true),
                        new Col("Total", 1.3f, Right: true), new Col("Paid By", 1.1f), new Col("Status", 1.1f),
                    },
                    rows.Select(r => new[]
                    {
                        new Cx(r.ReceiptNumber, Bold: true),
                        new Cx(r.When.ToString("h:mm tt", CultureInfo.CurrentCulture)),
                        new Cx(r.CustomerName),
                        new Cx(r.ServiceCount.ToString("N0")),
                        new Cx(r.ItemCount.ToString("N0")),
                        new Cx(Peso(r.Total), Bold: true),
                        new Cx(r.PaymentMethod),
                        new Cx(TransactionStatusStyle.Label(r.Status), Hex(TransactionStatusStyle.Pill(r.Status).Fore), Bold: true),
                    }).ToList()));

                body.Item().AlignRight().Width(230).Background(PrimarySoft).Padding(10).Column(c =>
                {
                    c.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Total Completed Sales").Bold();
                        r.ConstantItem(90).AlignRight().Text(Peso(total)).Bold().FontSize(11).FontColor(Primary);
                    });
                    foreach (var line in byMethod)
                        c.Item().PaddingTop(3).AlignRight().Text(line).FontSize(8.5f).FontColor(TextSoft);
                });

                body.Item().Text("Totals include completed sales only. Pending, refunded and voided transactions are listed but not counted.")
                    .FontSize(8).FontColor(Muted);
            });
    }

    /// <summary>Reports &amp; Financial Auditing: the rows of the chosen tab (or the ticked ones) under the period totals.</summary>
    public static void Audit(string path, AuditTab tab, string period, IReadOnlyList<AuditRow> rows, AuditTotals totals,
        int totalRows, bool selectionOnly, string preparedBy)
    {
        var culture = CultureInfo.CurrentCulture;
        string Peso(decimal v) => "\u20B1" + v.ToString("N2", culture);

        string subtitle = period + (selectionOnly ? $"  \u00B7  {rows.Count} of {totalRows} rows selected" : "");
        int flagged = rows.Count(r => r.Note.Length > 0);

        var headers = tab switch
        {
            AuditTab.Movement => new[] { "Date & Time", "Movement", "Product", "Qty", "Value", "Reference", "Status" },
            AuditTab.Summary => new[] { "Date", "Type", "Summary", "Items", "Net Sales", "Receipts", "Status" },
            _ => new[] { "Date & Time", "Type", "Product / Service", "Qty", "Amount", "Reference", "Status" },
        };
        float[] weights = { 1.7f, 1.1f, 2.8f, 0.7f, 1.3f, 1.8f, 1.3f };
        var cols = headers.Select((h, i) => new Col(h, weights[i], Right: i is 3 or 4)).ToArray();

        Build(path, ReportsData.Title(tab), subtitle, preparedBy, landscape: true,
            new[]
            {
                ("Total Sales (Audited)", Peso(totals.Sales)),
                ("Inventory Movement", totals.Units.ToString("N0", culture) + (totals.Units == 1 ? " item" : " items")),
                ("Discrepancies Found", totals.Discrepancies.ToString("N0", culture)),
                ("Rows in This Report", rows.Count.ToString("N0", culture)),
            },
            body =>
            {
                body.Item().Element(c => DataTable(c, cols,
                    rows.Select(r => new[]
                    {
                        new Cx(r.When.TimeOfDay == TimeSpan.Zero
                            ? r.When.ToString("MMM d, yyyy", culture)
                            : r.When.ToString("MMM d, yyyy h:mm tt", culture)),
                        new Cx(r.Type),
                        new Cx(r.Item, Bold: true),
                        new Cx(r.Quantity),
                        new Cx(r.Amount, r.AmountIsNegative ? Hex(Theme.Danger) : null),
                        new Cx(r.Reference),
                        new Cx(r.Status, Hex(ReportsData.BadgeFore(r.Badge)), Bold: true),
                    }).ToList()));

                if (flagged > 0)
                {
                    body.Item().Column(c =>
                    {
                        c.Item().Text("Needs review").Bold().FontSize(10).FontColor(Hex(Theme.Danger));
                        foreach (var r in rows.Where(r => r.Note.Length > 0).Take(40))
                        {
                            string label = r.Type switch
                            {
                                "Stock Check" => r.Item,
                                "Daily Summary" => r.When.ToString("MMM d, yyyy", culture),
                                _ => r.Reference,
                            };
                            c.Item().PaddingTop(3).Text($"{label}: {r.Note.Replace("\n", "; ")}").FontSize(8.5f).FontColor(TextSoft);
                        }
                    });
                }

                body.Item().Text("The cards above cover the whole period. Sales totals count completed sales only; refunded and voided sales are listed but not counted. "
                                 + "A discrepancy is a sale whose line items, discount, total, cash received or change do not add up, or a product whose stock does not match its batches.")
                    .FontSize(8).FontColor(Muted);
            });
    }

    /// <summary>Audit Trail / Activity Logs: the filtered (or ticked) log entries, newest first.</summary>
    public static void ActivityLog(string path, string period, IReadOnlyList<AuditLog.Entry> rows, string preparedBy)
    {
        var culture = CultureInfo.CurrentCulture;
        var cols = new[]
        {
            new Col("Date & Time", 1.6f), new Col("User", 1.4f), new Col("Action", 1.5f), new Col("Module", 1.1f),
            new Col("Description", 3.2f), new Col("Reference", 1.3f), new Col("Status", 0.9f),
        };

        Build(path, "Activity Logs", period, preparedBy, landscape: true,
            new[]
            {
                ("Entries in This Report", rows.Count.ToString("N0", culture)),
                ("Users Involved", rows.Select(r => r.Actor).Distinct().Count().ToString("N0", culture)),
                ("Critical Actions", rows.Count(r => AuditInfo.IsCritical(r.Action)).ToString("N0", culture)),
                ("Logins", rows.Count(r => AuditInfo.Type(r.Action) == "Login").ToString("N0", culture)),
            },
            body =>
            {
                body.Item().Element(c => DataTable(c, cols,
                    rows.Select(r => new[]
                    {
                        new Cx(r.When.ToString("MMM d, yyyy h:mm tt", culture)),
                        new Cx(r.Actor, Bold: true),
                        new Cx(r.Action),
                        new Cx(AuditInfo.Module(r.Action)),
                        new Cx(r.Details),
                        new Cx(AuditInfo.Reference(r.Details)),
                        new Cx(AuditInfo.Status(r.Action), AuditInfo.IsFailed(r.Action) ? Hex(Theme.Danger) : Hex(Theme.Green), Bold: true),
                    }).ToList()));
            });
    }

    // ------------------------------------------------------------------ shared layout

    public static void Inventory(string path, IReadOnlyList<InventoryRow> rows, string preparedBy)
    {
        var today = DateTime.Today;
        var culture = CultureInfo.CurrentCulture;
        var products = rows.GroupBy(r => r.ProductId).Select(g => g.First()).ToList();
        int low = products.Count(p => p.ReorderLevel > 0 && p.QtyOnHand <= p.ReorderLevel);
        int expiring = rows.Count(r => r.Expiry is DateTime e && e.Date <= today.AddDays(InventoryData.NearExpiryDays));

        string Day(DateTime? d) => d is DateTime v ? v.ToString("MMM d, yyyy", culture) : "";
        string StateColor(StockState s) => s switch
        {
            StockState.OutOfStock or StockState.Expired => Hex(Theme.Danger),
            StockState.LowStock or StockState.NearExpiry => Hex(Theme.Orange),
            _ => Hex(Theme.Green),
        };

        Build(path, "Inventory & FIFO Report", today.ToString("dddd, MMMM d, yyyy", culture), preparedBy, landscape: true,
            new[]
            {
                ("Products", products.Count.ToString("N0", culture)),
                ("Total Stock", products.Sum(p => p.QtyOnHand).ToString("N0", culture)),
                ("Low Stock Items", low.ToString("N0", culture)),
                ("Expired / Near Expiry", expiring.ToString("N0", culture)),
            },
            body =>
            {
                body.Item().Element(c => DataTable(c,
                    new[]
                    {
                        new Col("Product", 2.6f), new Col("Category", 1.4f), new Col("Batch #", 1.3f), new Col("Supplier", 1.5f),
                        new Col("Received", 1.1f), new Col("Expiry", 1.1f), new Col("Qty", 0.7f, Right: true), new Col("Status", 1.1f),
                    },
                    rows.Select(r =>
                    {
                        var state = InventoryData.StateOf(r, today);
                        return new[]
                        {
                            new Cx(r.Name, Bold: true),
                            new Cx(r.Category),
                            new Cx(r.BatchNo),
                            new Cx(r.Supplier),
                            new Cx(Day(r.Received)),
                            new Cx(Day(r.Expiry)),
                            new Cx(r.BatchQty.ToString("N0", culture)),
                            new Cx(InventoryData.Label(state), StateColor(state), Bold: true),
                        };
                    }).ToList()));

                body.Item().Text("Batches are listed oldest first, the order they are used (FIFO). \"Near Expiry\" means a batch expires within "
                                 + InventoryData.NearExpiryDays + " days; \"Low Stock\" means a product is at or below its reorder level.")
                    .FontSize(8).FontColor(Muted);
            });
    }

    private static void Build(string path, string title, string subtitle, string preparedBy, bool landscape,
        (string Label, string Value)[] stats, Action<ColumnDescriptor> body)
    {
        Document.Create(doc => doc.Page(page =>
        {
            page.Size(landscape ? PageSizes.A4.Landscape() : PageSizes.A4);
            page.Margin(32);
            page.DefaultTextStyle(x => x.FontFamily("Segoe UI").FontSize(9).FontColor(TextDark));

            page.Header().Element(h => Header(h, title, subtitle, preparedBy));

            page.Content().PaddingTop(14).Column(col =>
            {
                col.Spacing(12);
                col.Item().Element(c => Stats(c, stats));
                body(col);
            });

            page.Footer().Element(Footer);
        }))
        .WithMetadata(new DocumentMetadata { Title = title, Author = "Pawfect" })
        .GeneratePdf(path);
    }

    private static void Header(IContainer container, string title, string subtitle, string preparedBy)
    {
        var stamp = DateTime.Now.ToString("MMM d, yyyy h:mm tt", CultureInfo.CurrentCulture);

        container.Column(col =>
        {
            col.Item().Row(row =>
            {
                row.RelativeItem().Column(c =>
                {
                    c.Item().Text("Pawfect").FontSize(22).Bold().FontColor(Primary);
                    c.Item().Text("Pet Care. Made Simple.").FontSize(8.5f).FontColor(Muted);
                });

                row.RelativeItem().Column(c =>
                {
                    c.Item().AlignRight().Text(title).FontSize(15).Bold();
                    c.Item().AlignRight().Text(subtitle).FontSize(9).FontColor(TextSoft);
                    c.Item().AlignRight().Text($"Generated {stamp}  \u00B7  {preparedBy}").FontSize(8).FontColor(Muted);
                });
            });

            col.Item().PaddingTop(8).LineHorizontal(1.5f).LineColor(Primary);
        });
    }

    private static void Footer(IContainer container)
    {
        container.BorderTop(0.5f).BorderColor(Line).PaddingTop(6).Row(row =>
        {
            row.RelativeItem().Text("Pawfect Pet Care  \u00B7  Confidential, for internal use only").FontSize(8).FontColor(Muted);
            row.RelativeItem().AlignRight().Text(t =>
            {
                t.DefaultTextStyle(x => x.FontSize(8).FontColor(Muted));
                t.Span("Page ");
                t.CurrentPageNumber();
                t.Span(" of ");
                t.TotalPages();
            });
        });
    }

    private static void Stats(IContainer container, (string Label, string Value)[] stats)
    {
        container.Row(row =>
        {
            for (int i = 0; i < stats.Length; i++)
            {
                var stat = stats[i];
                row.RelativeItem().PaddingRight(i < stats.Length - 1 ? 8 : 0)
                   .Background(PrimarySoft).Padding(10).Column(c =>
                   {
                       c.Item().Text(stat.Label).FontSize(8).FontColor(Muted);
                       c.Item().Text(stat.Value).FontSize(15).Bold().FontColor(TextDark);
                   });
            }
        });
    }

    private static void DataTable(IContainer container, Col[] cols, IReadOnlyList<Cx[]> rows)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(c =>
            {
                foreach (var col in cols) c.RelativeColumn(col.Weight);
            });

            table.Header(h =>
            {
                foreach (var col in cols)
                {
                    var right = col.Right;
                    var header = col.Header;
                    h.Cell().Background(Primary).PaddingVertical(7).PaddingHorizontal(6)
                     .Text(t =>
                     {
                         if (right) t.AlignRight();
                         t.Span(header).Bold().FontSize(8.5f).FontColor("#FFFFFF");
                     });
                }
            });

            for (int i = 0; i < rows.Count; i++)
            {
                var fill = i % 2 == 1 ? Subtle : "#FFFFFF";
                for (int j = 0; j < cols.Length; j++)
                {
                    var right = cols[j].Right;
                    var cell = rows[i][j];
                    var text = cell.Text.Length == 0 ? "\u2014" : cell.Text;

                    table.Cell().Background(fill).BorderBottom(0.5f).BorderColor(Line)
                         .PaddingVertical(6).PaddingHorizontal(6)
                         .Text(t =>
                         {
                             if (right) t.AlignRight();
                             var span = t.Span(text);
                             if (cell.Bold) span.Bold();
                             span.FontColor(cell.Color ?? (cell.Text.Length == 0 ? Muted : TextDark));
                         });
                }
            }
        });
    }
}

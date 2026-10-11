using System.Globalization;

namespace MyApp;

/// <summary>A retail product that can be sold at the counter. Fill these from your data source and pass them to <c>BillingPage.SetProducts</c>.</summary>
internal sealed record ProductItem(int Id, string Code, string Name, string Category, decimal Price);

/// <summary>A grooming / care service that can be billed. Pass these to <c>BillingPage.SetServices</c>.</summary>
internal sealed record ServiceOffering(int Id, string Name, decimal Price);

internal enum CartLineKind
{
    Service,
    Product,
}

/// <summary>One line in the transaction cart.</summary>
internal sealed class CartLine
{
    public CartLine(CartLineKind kind, int sourceId, string name, string detail, decimal unitPrice,
        int quantity = 1, bool fromAppointment = false)
    {
        Kind = kind;
        SourceId = sourceId;
        Name = name;
        Detail = detail;
        UnitPrice = unitPrice;
        Quantity = quantity;
        FromAppointment = fromAppointment;
    }

    public CartLineKind Kind { get; }
    public int SourceId { get; }
    public string Name { get; }

    /// <summary>Small grey text under the name, e.g. the pet the service is for.</summary>
    public string Detail { get; }

    public decimal UnitPrice { get; }
    public int Quantity { get; set; }

    /// <summary>True when the line was added automatically because an appointment was picked.</summary>
    public bool FromAppointment { get; }

    public decimal Subtotal => UnitPrice * Quantity;
}

internal enum TransactionStatus
{
    Completed,
    Pending,
    Refunded,
    Voided,
}

/// <summary>A finished sale shown in "Recent Transactions". Pass these to <c>BillingPage.SetTransactions</c>.</summary>
internal sealed record TransactionItem(
    int Id,
    string ReceiptNumber,
    DateTime When,
    string CustomerName,
    int ServiceCount,
    int ItemCount,
    decimal Total,
    string PaymentMethod,
    TransactionStatus Status);

internal enum PaymentKind
{
    Cash,
    EWallet,
}

/// <summary>Everything needed to save the sale and print the receipt. Raised by <c>BillingPage.PrintReceiptRequested</c>.</summary>
internal sealed record BillingCheckout(
    IReadOnlyList<CartLine> Lines,
    decimal Subtotal,
    decimal Discount,
    decimal Total,
    PaymentKind Method,
    decimal AmountReceived,
    decimal Change,
    string Notes,
    AppointmentItem? Appointment);

/// <summary>Peso formatting used across the billing screen.</summary>
internal static class Money
{
    public const string Symbol = "\u20B1";

    public static string Format(decimal value)
    {
        var culture = CultureInfo.CurrentCulture;
        return Symbol + (value == decimal.Truncate(value)
            ? value.ToString("N0", culture)
            : value.ToString("N2", culture));
    }
}

/// <summary>Label and colours for each transaction status.</summary>
internal static class TransactionStatusStyle
{
    private static Color C(string hex) => ColorTranslator.FromHtml(hex);

    public static string Label(TransactionStatus status) => status.ToString();

    public static (Color Fore, Color Fill, Color Border) Pill(TransactionStatus status) => status switch
    {
        TransactionStatus.Completed => (C("#2E9E5B"), C("#E5F6EC"), C("#BFE5CE")),
        TransactionStatus.Pending => (C("#C77A0A"), C("#FEF3E0"), C("#F8D9A0")),
        TransactionStatus.Refunded => (C("#7C4DDB"), C("#F1EAFD"), C("#D9C8F8")),
        TransactionStatus.Voided => (C("#D93636"), C("#FDE8E8"), C("#F6C0C0")),
        _ => (Theme.TextSoft, Theme.Subtle, Theme.Border),
    };
}

namespace MyApp;

/// <summary>One printed / shown line of a receipt.</summary>
internal sealed record ReceiptLine(string Name, string Detail, int Quantity, decimal UnitPrice)
{
    public decimal Subtotal => UnitPrice * Quantity;
}

/// <summary>
/// Everything on a receipt. It is built either from a sale that was just saved or from a saved sale in the database,
/// and is used by the on-screen receipt, the printout and the transaction details. Empty values are simply left off.
/// </summary>
internal sealed record ReceiptData(
    string StoreName,
    string Tagline,
    string Address,
    string Phone,
    string ReceiptNumber,
    DateTime When,
    string Cashier,
    string Customer,
    IReadOnlyList<ReceiptLine> Lines,
    decimal Subtotal,
    decimal Discount,
    decimal Total,
    string PaymentMethod,
    decimal? AmountReceived,
    decimal? Change,
    string Notes)
{
    public const string BrandName = "Pawfect";
    public const string BrandTagline = "Pet Care. Made Simple.";

    public static ReceiptData FromSale(BillingCheckout sale, string receiptNumber, DateTime when,
        string cashier, string customer, string address, string phone)
    {
        bool cash = sale.Method == PaymentKind.Cash;
        return new ReceiptData(
            BrandName, BrandTagline, address, phone,
            receiptNumber, when, cashier, customer,
            sale.Lines.Select(l => new ReceiptLine(l.Name, l.Detail, l.Quantity, l.UnitPrice)).ToList(),
            sale.Subtotal, sale.Discount, sale.Total,
            cash ? "Cash" : "E-Wallet",
            cash ? sale.AmountReceived : null,
            cash ? sale.Change : null,
            sale.Notes);
    }
}

using MyApp.Controls;

namespace MyApp;

internal sealed class PriceAdjustDialog : ThemedDialog
{
    private readonly ComboBox categoryBox;
    private readonly NumericUpDown percentBox;

    public string Summary { get; private set; } = "";

    public PriceAdjustDialog()
        : base("Manage Pricing", "Raise or lower the price of every active service in a category.")
    {
        categoryBox = AddCombo("Apply to",
            new[] { "All categories", "Grooming", "Health & Wellness", "Add-ons", "Others" });
        percentBox = AddNumber("Change (%). Use a negative number to lower prices", 0, -90, 200, decimals: 1, step: 1);
        AddButtons("Apply", Save_Click);
    }

    private void Save_Click(object? sender, EventArgs e)
    {
        decimal pct = percentBox.Value;
        if (pct == 0)
        {
            MessageBox.Show(this, "Enter a percentage other than 0.");
            return;
        }

        string? category = categoryBox.SelectedIndex == 0 ? null : (string)categoryBox.SelectedItem!;
        string what = category ?? "all categories";
        if (MessageBox.Show(this, $"Change prices in {what} by {pct:+0.##;-0.##}%?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        try
        {
            int n = ServiceRepository.AdjustPrices(category, pct);
            Summary = $"Changed {n} service price(s) in {what} by {pct:+0.##;-0.##}%";
            DialogResult = DialogResult.OK;
        }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            MessageBox.Show(this, "Could not update prices: " + ex.Message);
        }
    }
}
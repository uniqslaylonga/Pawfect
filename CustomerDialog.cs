using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

internal sealed class CustomerDialog : ThemedDialog
{
    private readonly CustomerRepository.CustomerRow? _existing;
    private readonly InputField nameBox, phoneBox, emailBox;

    public string? SavedName { get; private set; }

    public CustomerDialog(CustomerRepository.CustomerRow? existing = null)
        : base(existing == null ? "Add Customer" : "Edit Customer",
               existing == null ? "Register a new customer." : "Update this customer's details.")
    {
        _existing = existing;

        nameBox = AddInput("Full name", "\uE77B", "e.g. Juan Dela Cruz", existing?.Name);
        phoneBox = AddInput("Phone", "\uE717", "e.g. 0917 123 4567", existing?.Phone);
        emailBox = AddInput("Email (optional)", "\uE715", "name@example.com", existing?.Email);

        AddButtons(existing == null ? "Add Customer" : "Save Changes", Save_Click);
    }

    private void Save_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(nameBox.Value) || string.IsNullOrWhiteSpace(phoneBox.Value))
        {
            MessageBox.Show(this, "Name and phone are required.");
            return;
        }

        try
        {
            if (_existing == null)
                CustomerRepository.Add(nameBox.Value, phoneBox.Value, emailBox.Value);
            else
                CustomerRepository.Update(_existing.Id, nameBox.Value, phoneBox.Value, emailBox.Value);

            SavedName = nameBox.Value.Trim();
            DialogResult = DialogResult.OK;
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            MessageBox.Show(this, "That phone number is already registered to another customer.");
        }
        catch (SqlException ex)
        {
            MessageBox.Show(this, "Could not save: " + ex.Message);
        }
    }
}
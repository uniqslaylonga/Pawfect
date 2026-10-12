using Microsoft.Data.SqlClient;

namespace MyApp;

internal sealed class ServiceDialog : Form
{
    private readonly ServiceRepository.ServiceRow? _existing;
    private readonly TextBox nameBox = new();
    private readonly ComboBox categoryBox = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox sizeBox = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TextBox descBox = new();
    private readonly NumericUpDown priceBox = new() { DecimalPlaces = 2, Maximum = 100000, Increment = 50 };
    private readonly NumericUpDown minutesBox = new() { Minimum = 5, Maximum = 600, Increment = 5, Value = 60 };

    /// <summary>Name of the saved service, so AdminForm can write it to the audit log.</summary>
    public string? SavedName { get; private set; }

    public ServiceDialog(ServiceRepository.ServiceRow? existing = null)
    {
        _existing = existing;
        Text = existing == null ? "Add Service" : "Edit Service";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = MinimizeBox = false;
        ClientSize = new Size(340, 470);

        categoryBox.DataSource = new[] { "Grooming", "Health & Wellness", "Add-ons", "Others" };
        sizeBox.DataSource = new[] { "Small", "Medium", "Large", "Giant", "All" };

        if (existing != null)
        {
            nameBox.Text = existing.Name;
            categoryBox.SelectedItem = existing.Category;
            sizeBox.SelectedItem = existing.Size;
            descBox.Text = existing.Description ?? "";
            priceBox.Value = existing.Price;
            minutesBox.Value = existing.DurationMinutes;
        }

        var save = new Button { Text = "Save" };
        var cancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel };
        save.Click += Save_Click;
        AcceptButton = save;
        CancelButton = cancel;

        int y = 12;
        void Field(string label, Control c)
        {
            Controls.Add(new Label { Text = label, Left = 16, Top = y, Width = 300 });
            c.SetBounds(16, y + 20, 308, 26);
            Controls.Add(c);
            y += 56;
        }
        Field("Service name", nameBox);
        Field("Category", categoryBox);
        Field("Size", sizeBox);
        Field("Description (optional)", descBox);
        Field("Price (₱)", priceBox);
        Field("Duration (minutes)", minutesBox);

        save.SetBounds(134, y + 4, 85, 32);
        cancel.SetBounds(229, y + 4, 85, 32);
        Controls.Add(save);
        Controls.Add(cancel);
    }

    private void Save_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(nameBox.Text))
        {
            MessageBox.Show("Service name is required.");
            return;
        }

        string category = (string)categoryBox.SelectedItem!;
        string size = (string)sizeBox.SelectedItem!;

        try
        {
            if (_existing == null)
                ServiceRepository.Add(nameBox.Text, category, size, descBox.Text, priceBox.Value, (int)minutesBox.Value);
            else
                ServiceRepository.Update(_existing.Id, nameBox.Text, category, size, descBox.Text, priceBox.Value, (int)minutesBox.Value);

            SavedName = $"{nameBox.Text.Trim()} ({size})";
            DialogResult = DialogResult.OK;
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            MessageBox.Show("That service already exists for this size.");
        }
        catch (SqlException ex)
        {
            MessageBox.Show("Could not save: " + ex.Message);
        }
    }
}
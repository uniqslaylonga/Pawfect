using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

internal sealed class RoomDialog : ThemedDialog
{
    private readonly RoomRepository.RoomRow? _existing;
    private readonly InputField nameBox;
    private readonly ComboBox typeBox;
    private readonly CheckBox? activeBox;

    public string? SavedName { get; private set; }

    public RoomDialog(RoomRepository.RoomRow? existing = null)
        : base(existing == null ? "Add Room" : "Edit Room",
               existing == null ? "Add a grooming room." : "Update this room.")
    {
        _existing = existing;

        nameBox = AddInput("Room name", "\uE80F", "e.g. Grooming Room 1", existing?.Name);
        typeBox = AddCombo("Type", RoomRepository.Types, existing?.Type);
        if (existing != null)
            activeBox = AddCheck("Room is in use (untick to hide it)", existing.Active);

        AddButtons(existing == null ? "Add Room" : "Save Changes", Save_Click);
    }

    private void Save_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(nameBox.Value))
        {
            MessageBox.Show(this, "Room name is required.");
            return;
        }

        string type = (string)typeBox.SelectedItem!;
        try
        {
            if (_existing == null)
                RoomRepository.Add(nameBox.Value, type);
            else
                RoomRepository.Update(_existing.Id, nameBox.Value, type, activeBox!.Checked);

            SavedName = nameBox.Value.Trim();
            DialogResult = DialogResult.OK;
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            MessageBox.Show(this, "A room with that name already exists.");
        }
        catch (SqlException ex)
        {
            MessageBox.Show(this, "Could not save: " + ex.Message);
        }
    }
}
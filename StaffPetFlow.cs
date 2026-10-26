using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>
/// Staff "Add pet": the same two steps a customer goes through. First the Pet Details pop-up (same fields and checks:
/// name rules, Dog/Cat, weight in kg), then the vaccination-card scanner (camera or photo, OCR, and the same checks:
/// card must match the pet, correct dates, nothing expired, anti-rabies present). Both dialogs fit 1536x826,
/// 1536x864 and 1920x1080 screens.
/// </summary>
internal static class StaffPetFlow
{
    /// <summary>Registers a new pet for the customer. Returns the new pet id, or null when staff cancelled the details.</summary>
    public static int? AddPet(IWin32Window owner, int customerId, string ownerName)
    {
        int petId;
        string petName;
        using (var details = new NewPetDetailsDialog(customerId))
        {
            if (details.ShowDialog(owner) != DialogResult.OK) return null;
            petId = details.PetId;
            petName = details.PetName;
        }

        ScanCard(owner, customerId, ownerName, petId, petName);
        return petId;
    }

    /// <summary>
    /// Opens the scanner for this pet until a card with in-date vaccinations is saved, or staff give up
    /// (the pet then stays saved without a vaccine record). Returns true when a valid card was saved.
    /// </summary>
    public static bool ScanCard(IWin32Window owner, int customerId, string ownerName, int petId, string petName)
    {
        while (true)
        {
            try
            {
                using var scan = new VaccinationScanForm(customerId, ownerName, petId);
                FitToScreen(scan);
                if (scan.ShowDialog(owner) == DialogResult.OK)
                {
                    if (!IsExpired(petId)) return true;
                    Msg.Show(owner, "This card has expired too. Please scan a card with current dates.", "Pawfect",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (SqlException ex)
            {
                Msg.Show(owner, "Couldn't open the scanner.\n\n" + ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            var again = Msg.Show(owner,
                $"{PetNameRules.Short(petName)} has no valid vaccine card yet. A valid card is required before grooming.\n\nScan a card now?",
                "Pawfect", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (again != DialogResult.Yes) return false;
        }
    }

    // Same rule as the customer side: one lapsed vaccine is enough to stop.
    private static bool IsExpired(int petId)
    {
        try
        {
            var rows = PetVaccinations.GetForPet(petId);
            return rows.Count > 0 && PetVaccinations.AnyLapsed(rows);
        }
        catch (SqlException) { return false; }   // it saved fine; don't block on a failed re-check
    }

    /// <summary>Keeps a dialog inside the screen's working area (small laptops such as 1536x826 once the taskbar is counted).</summary>
    private static void FitToScreen(Form form)
    {
        var area = Screen.FromPoint(Cursor.Position).WorkingArea;
        int w = Math.Min(form.ClientSize.Width, area.Width - 40);
        int h = Math.Min(form.ClientSize.Height, area.Height - 40);
        form.ClientSize = new Size(w, h);
    }
}

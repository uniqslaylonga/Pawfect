namespace Pawfect.Models;

public class VaccinationRecord
{
    public int VaccinationRecordId { get; set; }

    public int PetId { get; set; }

    public string VaccineName { get; set; } = string.Empty;

    public DateTime DateGiven { get; set; }

    public DateTime? NextDueDate { get; set; }

    public string? VeterinarianName { get; set; }

    public string? ClinicName { get; set; }

    public string? CardImagePath { get; set; }

    public string? OcrExtractedText { get; set; }

    public string VerificationStatus { get; set; } = "Pending";

    public string? StaffNotes { get; set; }

    public DateTime DateSubmitted { get; set; } = DateTime.Now;

    public DateTime? DateVerified { get; set; }

    // Navigation property
    public Pet? Pet { get; set; }
}
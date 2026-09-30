namespace Pawfect.Models;

public class Pet
{
    public int PetId { get; set; }

    public int OwnerId { get; set; }

    public string PetName { get; set; } = string.Empty;

    public string Species { get; set; } = string.Empty;

    public string Breed { get; set; } = string.Empty;

    public decimal Weight { get; set; }

    public string? MedicalConditions { get; set; }

    public string? BehavioralTraits { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.Now;

    public bool IsActive { get; set; } = true;

    // Navigation property
    public User? Owner { get; set; }
}
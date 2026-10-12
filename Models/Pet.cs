using Microsoft.AspNetCore.Identity;

namespace Pawfect.Models;

public class Pet
{
    public int PetId { get; set; }

    // Id of the logged-in Identity user (AspNetUsers.Id)
    public string OwnerId { get; set; } = string.Empty;

    public string PetName { get; set; } = string.Empty;

    public string Species { get; set; } = string.Empty;

    public string Breed { get; set; } = string.Empty;

    public decimal Weight { get; set; }

    public string? MedicalConditions { get; set; }

    public string? BehavioralTraits { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    // Navigation property
    public IdentityUser? Owner { get; set; }
}

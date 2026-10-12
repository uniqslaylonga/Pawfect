namespace Pawfect.Models;

public class Appointment
{
    public int AppointmentId { get; set; }
    public string? CustomerId { get; set; }          // AspNetUsers.Id
    public int? PetId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public DateTime ScheduledAt { get; set; }        // store as UTC
    public decimal Price { get; set; }
    public string Status { get; set; } = "Pending";  // Pending, Approved, Completed, Cancelled
}

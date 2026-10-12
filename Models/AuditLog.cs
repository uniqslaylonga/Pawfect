namespace Pawfect.Models;

public class AuditLog
{
    public int AuditLogId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Action { get; set; } = string.Empty;
    public string? Details { get; set; }
    public string? UserName { get; set; }
}

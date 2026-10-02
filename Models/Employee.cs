using System.ComponentModel.DataAnnotations.Schema;

namespace Pawfect.Models;

public class Employee
{
    public static readonly string[] Roles = { "Admin", "Cashier", "Front Desk", "Groomer" };

    public int EmployeeId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = "Cashier";
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    [NotMapped] public string Code => $"EMP-{EmployeeId:000}";

    [NotMapped]
    public string Department => Role switch
    {
        "Admin" => "Administration",
        "Groomer" => "Grooming",
        _ => "Front Desk"
    };
}

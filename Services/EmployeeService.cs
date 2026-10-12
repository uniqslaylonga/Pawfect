using Microsoft.EntityFrameworkCore;
using Pawfect.Data;
using Pawfect.Models;

namespace Pawfect.Services;

public class EmployeeService(PawfectDbContext db)
{
    public async Task<List<Employee>> LoadAsync() =>
        await db.Employees.AsNoTracking().OrderBy(e => e.EmployeeId).ToListAsync();

    public async Task<string?> AddAsync(Employee e, string actor)
    {
        if (await db.Employees.AnyAsync(x => x.Email == e.Email))
            return "An employee with that email already exists.";
        e.DateCreated = DateTime.UtcNow;
        db.Employees.Add(e);
        await db.SaveChangesAsync();
        await Log("Employee added", $"{e.FullName} ({e.Role}) added", actor);
        return null;
    }

    public async Task<string?> UpdateAsync(Employee form, string actor)
    {
        var e = await db.Employees.FindAsync(form.EmployeeId);
        if (e is null) return "Employee not found.";
        if (await db.Employees.AnyAsync(x => x.Email == form.Email && x.EmployeeId != form.EmployeeId))
            return "An employee with that email already exists.";
        e.FullName = form.FullName;
        e.Email = form.Email;
        e.Role = form.Role;
        e.IsActive = form.IsActive;
        await db.SaveChangesAsync();
        await Log("Employee updated", $"{e.FullName} ({e.Role}) updated", actor);
        return null;
    }

    public async Task SetActiveAsync(int id, bool active, string actor)
    {
        var e = await db.Employees.FindAsync(id);
        if (e is null) return;
        e.IsActive = active;
        await db.SaveChangesAsync();
        await Log(active ? "Employee activated" : "Employee deactivated", e.FullName, actor);
    }

    public async Task DeleteAsync(int id, string actor)
    {
        var e = await db.Employees.FindAsync(id);
        if (e is null) return;
        db.Employees.Remove(e);
        await db.SaveChangesAsync();
        await Log("Employee removed", e.FullName, actor);
    }

    private async Task Log(string action, string details, string actor)
    {
        db.AuditLogs.Add(new AuditLog { Action = action, Details = details, UserName = actor });
        await db.SaveChangesAsync();
    }
}

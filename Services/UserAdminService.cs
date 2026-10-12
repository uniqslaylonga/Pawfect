using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pawfect.Data;
using Pawfect.Models;

namespace Pawfect.Services;

public record UserRow(string Id, string Username, string FullName, string Email, string Role, bool Active);

// Manages Identity accounts. A user with no role is treated as a Customer.
public class UserAdminService(
    PawfectDbContext db,
    UserManager<IdentityUser> users,
    RoleManager<IdentityRole> roles)
{
    public static readonly string[] Roles = { "Admin", "Staff", "Customer" };
    private const string NameClaim = "FullName";

    public async Task<List<UserRow>> LoadAsync()
    {
        var now = DateTimeOffset.UtcNow;
        var all = await db.Users.AsNoTracking().ToListAsync();
        var roleRows = await (from ur in db.UserRoles
                              join r in db.Roles on ur.RoleId equals r.Id
                              select new { ur.UserId, r.Name }).ToListAsync();
        var names = await db.UserClaims.AsNoTracking()
            .Where(c => c.ClaimType == NameClaim).ToListAsync();

        return all.Select(u =>
        {
            var userRoles = roleRows.Where(r => r.UserId == u.Id).Select(r => r.Name).ToList();
            var role = userRoles.Contains("Admin") ? "Admin" : userRoles.Contains("Staff") ? "Staff" : "Customer";
            var full = names.FirstOrDefault(c => c.UserId == u.Id)?.ClaimValue;
            return new UserRow(u.Id, u.UserName ?? "", string.IsNullOrWhiteSpace(full) ? (u.UserName ?? "") : full,
                u.Email ?? "", role, u.LockoutEnd is null || u.LockoutEnd <= now);
        })
        .OrderBy(u => Array.IndexOf(Roles, u.Role))
        .ThenBy(u => u.Username)
        .ToList();
    }

    public async Task<string?> CreateAsync(string fullName, string username, string email,
        string password, string role, string actor)
    {
        var u = new IdentityUser { UserName = username.Trim(), Email = email.Trim(), EmailConfirmed = true };
        var result = await users.CreateAsync(u, password);
        if (!result.Succeeded) return string.Join(" ", result.Errors.Select(e => e.Description));

        if (!string.IsNullOrWhiteSpace(fullName))
            await users.AddClaimAsync(u, new Claim(NameClaim, fullName.Trim()));
        await SetRoleAsync(u, role);
        await Log("User created", $"{u.UserName} ({role}) created", actor);
        return null;
    }

    public async Task<string?> UpdateAsync(string id, string fullName, string username, string email,
        string role, string actorId, string actor)
    {
        var u = await users.FindByIdAsync(id);
        if (u is null) return "User not found.";
        if (id == actorId && role != "Admin") return "You can't remove your own Admin role.";

        if (!string.Equals(u.UserName, username.Trim(), StringComparison.Ordinal))
        {
            var r = await users.SetUserNameAsync(u, username.Trim());
            if (!r.Succeeded) return string.Join(" ", r.Errors.Select(e => e.Description));
        }
        if (!string.Equals(u.Email, email.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            var r = await users.SetEmailAsync(u, email.Trim());
            if (!r.Succeeded) return string.Join(" ", r.Errors.Select(e => e.Description));
        }

        var claims = await users.GetClaimsAsync(u);
        foreach (var c in claims.Where(c => c.Type == NameClaim))
            await users.RemoveClaimAsync(u, c);
        if (!string.IsNullOrWhiteSpace(fullName))
            await users.AddClaimAsync(u, new Claim(NameClaim, fullName.Trim()));

        await SetRoleAsync(u, role);
        await Log("User updated", $"{u.UserName} ({role}) updated", actor);
        return null;
    }

    public async Task<string?> SetActiveAsync(string id, bool active, string actorId, string actor)
    {
        if (id == actorId && !active) return "You can't deactivate your own account.";
        var u = await users.FindByIdAsync(id);
        if (u is null) return "User not found.";

        await users.SetLockoutEnabledAsync(u, true);
        await users.SetLockoutEndDateAsync(u, active ? null : DateTimeOffset.MaxValue);
        if (!active) await users.UpdateSecurityStampAsync(u); // signs out existing sessions
        await Log(active ? "User activated" : "User deactivated", u.UserName ?? "", actor);
        return null;
    }

    public async Task<string?> DeleteAsync(string id, string actorId, string actor)
    {
        if (id == actorId) return "You can't delete your own account.";
        var u = await users.FindByIdAsync(id);
        if (u is null) return "User not found.";
        try
        {
            var r = await users.DeleteAsync(u);
            if (!r.Succeeded) return string.Join(" ", r.Errors.Select(e => e.Description));
        }
        catch (DbUpdateException)
        {
            return "This user still has related records (e.g. pets). Deactivate the account instead.";
        }
        await Log("User removed", u.UserName ?? "", actor);
        return null;
    }

    private async Task SetRoleAsync(IdentityUser u, string role)
    {
        var current = await users.GetRolesAsync(u);
        var wanted = role == "Customer" ? null : role;
        var remove = current.Where(r => r != wanted).ToList();
        if (remove.Count > 0) await users.RemoveFromRolesAsync(u, remove);
        if (wanted is not null && !current.Contains(wanted))
        {
            if (!await roles.RoleExistsAsync(wanted)) await roles.CreateAsync(new IdentityRole(wanted));
            await users.AddToRoleAsync(u, wanted);
        }
    }

    private async Task Log(string action, string details, string actor)
    {
        db.AuditLogs.Add(new AuditLog { Action = action, Details = details, UserName = actor });
        await db.SaveChangesAsync();
    }
}

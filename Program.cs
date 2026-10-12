using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Pawfect.Data;
using Pawfect.Models;
using Pawfect.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddRazorPages(); // needed for the Identity login/register pages
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddDbContext<PawfectDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<PawfectDbContext>();

// Record every sign-in in the audit trail (never let a logging problem block a login).
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnSignedIn = async context =>
    {
        try
        {
            var db = context.HttpContext.RequestServices.GetRequiredService<PawfectDbContext>();
            db.AuditLogs.Add(new AuditLog
            {
                Action = "User login",
                Details = "Signed in",
                UserName = context.Principal?.Identity?.Name
            });
            await db.SaveChangesAsync();
        }
        catch
        {
            // ignore
        }
    };
});

builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<UserAdminService>();
builder.Services.AddScoped<CatalogService>();
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<AuditTrailService>();

var app = builder.Build();

// Development only: apply migrations, create roles, and create a TEST admin account.
// Remove the admin part before deploying anywhere public.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    services.GetRequiredService<PawfectDbContext>().Database.Migrate();

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    foreach (var role in new[] { "Admin", "Staff" })
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    const string adminEmail = "admin@pawfect.test";
    const string adminPassword = "Admin123!";

    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var admin = await userManager.FindByEmailAsync(adminEmail);
    if (admin is null)
    {
        admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
        var created = await userManager.CreateAsync(admin, adminPassword);
        if (!created.Succeeded)
            throw new InvalidOperationException("Could not create test admin: " +
                string.Join("; ", created.Errors.Select(e => e.Description)));
    }
    if (!await userManager.IsInRoleAsync(admin, "Admin"))
        await userManager.AddToRoleAsync(admin, "Admin");
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorPages();

app.MapRazorComponents<Pawfect.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();

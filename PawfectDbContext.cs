using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pawfect.Models;

namespace Pawfect.Data;

public class PawfectDbContext : IdentityDbContext
{
    public PawfectDbContext(DbContextOptions<PawfectDbContext> options)
        : base(options)
    {
    }

    public DbSet<Pet> Pets { get; set; }
    public DbSet<VaccinationRecord> VaccinationRecords { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<RetailSale> RetailSales { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pet>()
            .Property(p => p.Weight)
            .HasPrecision(6, 2);

        modelBuilder.Entity<Appointment>().Property(a => a.Price).HasPrecision(12, 2);
        modelBuilder.Entity<RetailSale>().Property(s => s.Total).HasPrecision(12, 2);
        modelBuilder.Entity<InventoryItem>().Property(i => i.UnitPrice).HasPrecision(12, 2);
    }
}

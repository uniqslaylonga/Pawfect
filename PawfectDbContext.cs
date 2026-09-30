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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pet>()
            .Property(p => p.Weight)
            .HasPrecision(6, 2);
    }
}
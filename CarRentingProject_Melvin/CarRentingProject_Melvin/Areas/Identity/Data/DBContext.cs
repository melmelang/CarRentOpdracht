using CarRentingProject_Melvin.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarRentingProject_Melvin.Data;

public class DBContext : IdentityDbContext<CarRentingProject_AppUser>
{
    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public DbSet<CarRentingProject_Melvin.Models.Gender> Gender { get; set; }

    public DbSet<CarRentingProject_Melvin.Models.Language> Languages { get; set; }

    public DbSet<CarRentingProject_Melvin.Models.Cars> Cars { get; set; }

    public DbSet<CarRentingProject_Melvin.Models.Renter> Renter { get; set; }

    public DbSet<CarRentingProject_Melvin.Models.Tenant> Tenant { get; set; }

    public DbSet<CarRentingProject_Melvin.Models.RentedCars> RentedCars { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}



using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MotoVerse.Entities.Models.Auth;
using MotoVerse.Entities.Models.Motorcycles;
using MyRole = MotoVerse.Entities.Models.Auth.MyRole;

namespace MotoVerse.Infrastructure.Data;

// add-migration init -o "Data/Migrations"

public class AppDbContextFactory
    : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=.;Database=MotoVerseDb;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True");

        return new AppDbContext(optionsBuilder.Options);
    }
}
public class AppDbContext : IdentityDbContext<User, MyRole, string>
{

    #region CTOR
    public AppDbContext() { }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    #endregion

    #region DbSets :

    #region Users
    public DbSet<Admin> Admins { get; set; }
    #endregion

    public DbSet<Product> Products => Set<Product>();
    public DbSet<ReviewProduct> ReviewProducts => Set<ReviewProduct>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Entities.Models.Order> Orders => Set<Entities.Models.Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    public DbSet<Coupon> Coupons => Set<Coupon>();

    #region MotoCycle

    public DbSet<Owner> Owners => Set<Owner>();
    public DbSet<Motorcycle> Motorcycles => Set<Motorcycle>();
    public DbSet<MotorcycleImage> MotorcycleImages => Set<MotorcycleImage>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<ReviewMotorCycle> ReviewMotorCycles => Set<ReviewMotorCycle>();
    public DbSet<Favorite> Favorites => Set<Favorite>();

    #endregion

    #endregion

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.ConfigureWarnings(w =>
        w.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
    // OnModelCreating
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

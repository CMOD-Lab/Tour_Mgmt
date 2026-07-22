using Microsoft.EntityFrameworkCore;
using TourManagement.Domain.Entities;
using TourManagement.Infrastructure.Data.Configurations;

namespace TourManagement.Infrastructure.Data;

/// <summary>
/// Entity Framework Core database context for Tour Management application.
/// Configured for PostgreSQL with snake_case naming conventions.
/// </summary>
public class TourManagementDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of <see cref="TourManagementDbContext"/>.
    /// </summary>
    /// <param name="options">The database context options.</param>
    public TourManagementDbContext(DbContextOptions<TourManagementDbContext> options)
        : base(options)
    {
    }

    /// <summary>Gets or sets the Tours DbSet.</summary>
    public DbSet<Tour> Tours { get; set; } = null!;

    /// <summary>Gets or sets the Users DbSet.</summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <summary>Gets or sets the Bookings DbSet.</summary>
    public DbSet<Booking> Bookings { get; set; } = null!;

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Set default schema to public for PostgreSQL
        modelBuilder.HasDefaultSchema("public");

        // Enable PostgreSQL extensions
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.ApplyConfiguration(new TourConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new BookingConfiguration());
    }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Enable legacy timestamp behavior for DateTime compatibility
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}

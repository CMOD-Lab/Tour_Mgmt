using Microsoft.EntityFrameworkCore;
using Tour_Management.Domain.Entities;
using Tour_Management.Infrastructure.Data.Configurations;

namespace Tour_Management.Infrastructure.Data;

/// <summary>
/// Entity Framework Core database context for the Tour Management application.
/// </summary>
public class TourManagementDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TourManagementDbContext"/> class.
    /// </summary>
    /// <param name="options">The database context options.</param>
    public TourManagementDbContext(DbContextOptions<TourManagementDbContext> options)
        : base(options)
    {
    }

    /// <summary>Gets or sets the Tours DbSet.</summary>
    public DbSet<Tour> Tours { get; set; } = null!;

    /// <summary>Gets or sets the Users DbSet.</summary>
    public DbSet<UserInfo> Users { get; set; } = null!;

    /// <summary>Gets or sets the Bookings DbSet.</summary>
    public DbSet<Booking> Bookings { get; set; } = null!;

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Set default schema to "public" for PostgreSQL
        modelBuilder.HasDefaultSchema("public");

        // Enable PostgreSQL extensions
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.ApplyConfiguration(new TourConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new BookingConfiguration());
    }
}

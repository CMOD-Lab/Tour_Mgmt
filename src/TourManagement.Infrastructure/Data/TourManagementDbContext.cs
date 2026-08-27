using Microsoft.EntityFrameworkCore;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Data;

/// <summary>
/// Entity Framework Core database context for the Tour Management application.
/// </summary>
public class TourManagementDbContext : DbContext
{
    /// <summary>Initializes a new instance of TourManagementDbContext.</summary>
    public TourManagementDbContext(DbContextOptions<TourManagementDbContext> options)
        : base(options)
    {
    }

    /// <summary>Gets or sets the Tours DbSet.</summary>
    public DbSet<Tour> Tours { get; set; } = null!;

    /// <summary>Gets or sets the Bookings DbSet.</summary>
    public DbSet<Booking> Bookings { get; set; } = null!;

    /// <summary>Gets or sets the UserInfos DbSet.</summary>
    public DbSet<UserInfo> UserInfos { get; set; } = null!;

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TourManagementDbContext).Assembly);
    }
}

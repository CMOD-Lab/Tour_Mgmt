using Microsoft.EntityFrameworkCore;
using TourBooking.Domain.Entities;
using TourBooking.Infrastructure.Data.Configurations;

namespace TourBooking.Infrastructure.Data;

/// <summary>
/// Entity Framework Core database context for the Tour Booking application.
/// </summary>
public class TourBookingDbContext : DbContext
{
    /// <summary>Initializes a new instance of the <see cref="TourBookingDbContext"/> class.</summary>
    public TourBookingDbContext(DbContextOptions<TourBookingDbContext> options) : base(options)
    {
    }

    /// <summary>Gets or sets the UserInfo DbSet.</summary>
    public DbSet<UserInfo> UserInfos { get; set; } = null!;

    /// <summary>Gets or sets the Tour DbSet.</summary>
    public DbSet<Tour> Tours { get; set; } = null!;

    /// <summary>Gets or sets the Booking DbSet.</summary>
    public DbSet<Booking> Bookings { get; set; } = null!;

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserInfoConfiguration());
        modelBuilder.ApplyConfiguration(new TourConfiguration());
        modelBuilder.ApplyConfiguration(new BookingConfiguration());
    }
}

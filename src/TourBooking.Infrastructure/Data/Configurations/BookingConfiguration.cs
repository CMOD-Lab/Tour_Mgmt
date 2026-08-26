using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourBooking.Domain.Entities;

namespace TourBooking.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core entity configuration for Booking.
/// </summary>
public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("booking");

        builder.HasKey(b => b.TourId);

        builder.Property(b => b.TourId)
            .HasColumnName("TOUR_ID")
            .ValueGeneratedOnAdd();

        builder.Property(b => b.TourName)
            .HasMaxLength(50)
            .HasColumnName("TOUR_NAME");

        builder.Property(b => b.Place)
            .HasMaxLength(50)
            .HasColumnName("PLACE");

        builder.Property(b => b.Email)
            .HasMaxLength(50)
            .HasColumnName("Email");

        builder.Property(b => b.FirstName)
            .HasMaxLength(50)
            .HasColumnName("FirstName");
    }
}

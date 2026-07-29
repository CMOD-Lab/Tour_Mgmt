using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core entity configuration for the Booking entity.
/// Updated for PostgreSQL compatibility.
/// </summary>
public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("booking");

        builder.HasKey(b => b.BookingId);

        builder.Property(b => b.BookingId)
            .HasColumnName("booking_id")
            .UseIdentityColumn();

        builder.Property(b => b.TourName)
            .HasColumnName("tour_name")
            .HasMaxLength(50);

        builder.Property(b => b.Place)
            .HasColumnName("place")
            .HasMaxLength(50);

        builder.Property(b => b.Email)
            .HasColumnName("email")
            .HasMaxLength(50);

        builder.Property(b => b.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(50);

        builder.Property(b => b.TourId)
            .HasColumnName("tour_id");

        builder.Property(b => b.CreatedDate)
            .HasColumnName("created_date")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("NOW()");

        builder.Property(b => b.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        // Navigation: booking -> tour
        builder.HasOne(b => b.Tour)
            .WithMany(t => t.Bookings)
            .HasForeignKey(b => b.TourId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

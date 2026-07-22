using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core entity configuration for Booking.
/// Configured for PostgreSQL compatibility with snake_case naming.
/// </summary>
public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("booking");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.TourName)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("tour_name");

        builder.Property(b => b.Place)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("place");

        builder.Property(b => b.Email)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnName("email");

        builder.Property(b => b.FirstName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("first_name");

        builder.Property(b => b.TourId)
            .HasColumnName("tour_id");

        builder.Property(b => b.UserId)
            .HasColumnName("user_id");

        builder.Property(b => b.BookingDate)
            .IsRequired()
            .HasColumnName("booking_date")
            .HasDefaultValueSql("NOW()");

        builder.Property(b => b.CreatedDate)
            .IsRequired()
            .HasColumnName("created_date")
            .HasDefaultValueSql("NOW()");

        builder.Property(b => b.ModifiedDate)
            .HasColumnName("modified_date");

        builder.Property(b => b.IsActive)
            .IsRequired()
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.HasOne(b => b.Tour)
            .WithMany(t => t.Bookings)
            .HasForeignKey(b => b.TourId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

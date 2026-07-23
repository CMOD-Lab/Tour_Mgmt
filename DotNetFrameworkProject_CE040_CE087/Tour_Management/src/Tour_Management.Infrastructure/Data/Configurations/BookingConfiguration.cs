using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tour_Management.Domain.Entities;

namespace Tour_Management.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the Booking entity.
/// </summary>
public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("booking");

        builder.HasKey(b => b.BookingId);

        builder.Property(b => b.BookingId)
            .HasColumnName("booking_id")
            .ValueGeneratedOnAdd();

        builder.Property(b => b.TourName)
            .HasColumnName("tour_name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Place)
            .HasColumnName("place")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Email)
            .HasColumnName("email")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.FirstName)
            .HasColumnName("first_name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.CreatedDate)
            .HasDefaultValueSql("NOW()");

        builder.Property(b => b.IsActive)
            .HasDefaultValue(true);

        builder.Property(b => b.CreatedBy)
            .HasMaxLength(100)
            .HasDefaultValue("system");

        builder.Property(b => b.ModifiedBy)
            .HasMaxLength(100);

        builder.HasOne(b => b.Tour)
            .WithMany(t => t.Bookings)
            .HasForeignKey(b => b.TourId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core entity configuration for the Booking entity.
/// </summary>
public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("booking");

        builder.HasKey(b => b.BookingId);

        builder.Property(b => b.BookingId)
            .HasColumnName("TOUR_ID")
            .ValueGeneratedOnAdd();

        builder.Property(b => b.TourName)
            .HasColumnName("TOUR_NAME")
            .HasMaxLength(100);

        builder.Property(b => b.Place)
            .HasColumnName("PLACE")
            .HasMaxLength(100);

        builder.Property(b => b.Email)
            .HasColumnName("Email")
            .HasMaxLength(100);

        builder.Property(b => b.FirstName)
            .HasColumnName("FirstName")
            .HasMaxLength(100);

        builder.Property(b => b.TourId)
            .HasColumnName("TourRefId");

        builder.Property(b => b.BookingDate)
            .HasColumnName("BookingDate")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(b => b.IsActive)
            .HasColumnName("IsActive")
            .HasDefaultValue(true);
    }
}

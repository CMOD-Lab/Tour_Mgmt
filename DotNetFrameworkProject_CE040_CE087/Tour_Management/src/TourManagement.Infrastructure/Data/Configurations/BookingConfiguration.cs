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

        builder.HasKey(b => b.Id);

        builder.Property(b => b.TourName)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("TOUR_NAME");

        builder.Property(b => b.Place)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("PLACE");

        builder.Property(b => b.Email)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("Email");

        builder.Property(b => b.FirstName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("FirstName");

        builder.Property(b => b.BookingDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(b => b.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(b => b.IsActive)
            .HasDefaultValue(true);

        builder.Property(b => b.CreatedBy)
            .HasMaxLength(100)
            .HasDefaultValue("system");

        builder.Property(b => b.ModifiedBy)
            .HasMaxLength(100);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core entity configuration for the Tour entity.
/// </summary>
public class TourConfiguration : IEntityTypeConfiguration<Tour>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Tour> builder)
    {
        builder.ToTable("Tour");

        builder.HasKey(t => t.TourId);

        builder.Property(t => t.TourId)
            .HasColumnName("TOUR_ID")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.TourName)
            .HasColumnName("TOUR_NAME")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Place)
            .HasColumnName("PLACE")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Days)
            .HasColumnName("DAYS")
            .IsRequired();

        builder.Property(t => t.Price)
            .HasColumnName("PRICE")
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.Property(t => t.Locations)
            .HasColumnName("LOCATIONS")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.TourInfo)
            .HasColumnName("TOUR_INFO")
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(t => t.Pic)
            .HasColumnName("pic")
            .HasMaxLength(200);

        builder.Property(t => t.CreatedDate)
            .HasColumnName("CreatedDate")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(t => t.IsActive)
            .HasColumnName("IsActive")
            .HasDefaultValue(true);

        // Relationship: Tour has many Bookings
        builder.HasMany(t => t.Bookings)
            .WithOne(b => b.Tour)
            .HasForeignKey(b => b.TourId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

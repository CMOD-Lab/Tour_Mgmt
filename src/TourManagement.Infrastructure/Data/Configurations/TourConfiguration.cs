using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the Tour entity.
/// </summary>
public class TourConfiguration : IEntityTypeConfiguration<Tour>
{
    public void Configure(EntityTypeBuilder<Tour> builder)
    {
        builder.ToTable("Tour");

        builder.HasKey(t => t.TourId);

        builder.Property(t => t.TourId)
            .HasColumnName("TOUR_ID")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.TourName)
            .IsRequired()
            .HasColumnName("TOUR_NAME")
            .HasMaxLength(20);

        builder.Property(t => t.Place)
            .IsRequired()
            .HasColumnName("PLACE")
            .HasMaxLength(20);

        builder.Property(t => t.Days)
            .IsRequired()
            .HasColumnName("DAYS");

        builder.Property(t => t.Price)
            .IsRequired()
            .HasColumnName("PRICE")
            .HasColumnType("decimal(10,2)");

        builder.Property(t => t.Locations)
            .IsRequired()
            .HasColumnName("LOCATIONS")
            .HasMaxLength(100);

        builder.Property(t => t.TourInfo)
            .IsRequired()
            .HasColumnName("TOUR_INFO")
            .HasMaxLength(200);

        builder.Property(t => t.Pic)
            .HasColumnName("pic")
            .HasMaxLength(200);

        builder.Property(t => t.IsActive)
            .HasDefaultValue(true);

        builder.HasMany(t => t.Bookings)
            .WithOne(b => b.Tour)
            .HasForeignKey(b => b.TourId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

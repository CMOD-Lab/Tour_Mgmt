using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourBooking.Domain.Entities;

namespace TourBooking.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core entity configuration for Tour.
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
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("TOUR_NAME");

        builder.Property(t => t.Place)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("PLACE");

        builder.Property(t => t.Days)
            .IsRequired()
            .HasColumnName("DAYS");

        builder.Property(t => t.Price)
            .IsRequired()
            .HasColumnType("numeric(6,0)")
            .HasColumnName("PRICE");

        builder.Property(t => t.Locations)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("LOCATIONS");

        builder.Property(t => t.TourInfo)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("TOUR_INFO");

        builder.Property(t => t.Pic)
            .HasMaxLength(200)
            .HasColumnName("pic");
    }
}

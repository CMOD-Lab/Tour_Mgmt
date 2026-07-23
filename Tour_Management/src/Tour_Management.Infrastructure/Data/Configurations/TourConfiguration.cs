using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tour_Management.Domain.Entities;

namespace Tour_Management.Infrastructure.Data.Configurations;

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
            .HasColumnName("TOUR_NAME")
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(t => t.Place)
            .HasColumnName("PLACE")
            .IsRequired()
            .HasMaxLength(20);

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
            .HasMaxLength(100);

        builder.Property(t => t.TourInfo)
            .HasColumnName("TOUR_INFO")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.PicturePath)
            .HasColumnName("pic")
            .HasMaxLength(200);

        builder.Property(t => t.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(t => t.IsActive)
            .HasDefaultValue(true);

        builder.HasMany(t => t.Bookings)
            .WithOne()
            .HasForeignKey(b => b.TourName)
            .HasPrincipalKey(t => t.TourName)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

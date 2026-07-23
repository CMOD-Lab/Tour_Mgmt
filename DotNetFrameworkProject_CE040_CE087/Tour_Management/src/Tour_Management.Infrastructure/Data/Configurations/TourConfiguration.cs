using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tour_Management.Domain.Entities;

namespace Tour_Management.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the Tour entity.
/// </summary>
public class TourConfiguration : IEntityTypeConfiguration<Tour>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Tour> builder)
    {
        builder.ToTable("tour");

        builder.HasKey(t => t.TourId);

        builder.Property(t => t.TourId)
            .HasColumnName("tour_id")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.TourName)
            .HasColumnName("tour_name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Place)
            .HasColumnName("place")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Days)
            .HasColumnName("days")
            .IsRequired();

        builder.Property(t => t.Price)
            .HasColumnName("price")
            .IsRequired()
            .HasColumnType("numeric(18,2)");

        builder.Property(t => t.Locations)
            .HasColumnName("locations")
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(t => t.TourInfo)
            .HasColumnName("tour_info")
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(t => t.Pic)
            .HasColumnName("pic")
            .HasMaxLength(500);

        builder.Property(t => t.CreatedDate)
            .HasDefaultValueSql("NOW()");

        builder.Property(t => t.IsActive)
            .HasDefaultValue(true);

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(100)
            .HasDefaultValue("system");

        builder.Property(t => t.ModifiedBy)
            .HasMaxLength(100);

        builder.HasMany(t => t.Bookings)
            .WithOne(b => b.Tour)
            .HasForeignKey(b => b.TourId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

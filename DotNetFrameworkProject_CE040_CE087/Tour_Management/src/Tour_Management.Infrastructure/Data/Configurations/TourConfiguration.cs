using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tour_Management.Domain.Entities;

namespace Tour_Management.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core entity configuration for the Tour entity.
/// Configured for PostgreSQL compatibility with snake_case naming conventions.
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
            .HasMaxLength(1000);

        builder.Property(t => t.Pic)
            .HasColumnName("pic")
            .HasMaxLength(500);

        builder.Property(t => t.CreatedDate)
            .HasColumnName("created_date")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("NOW()");

        builder.Property(t => t.ModifiedDate)
            .HasColumnName("modified_date")
            .HasColumnType("timestamp with time zone");

        builder.Property(t => t.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.HasMany(t => t.Bookings)
            .WithOne(b => b.Tour)
            .HasForeignKey(b => b.TourId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

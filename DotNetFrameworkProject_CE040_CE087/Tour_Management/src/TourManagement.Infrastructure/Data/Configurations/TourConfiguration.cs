using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core entity configuration for Tour.
/// Configured for PostgreSQL compatibility with snake_case naming.
/// </summary>
public class TourConfiguration : IEntityTypeConfiguration<Tour>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Tour> builder)
    {
        builder.ToTable("tour");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.TourName)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("tour_name");

        builder.Property(t => t.Place)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("place");

        builder.Property(t => t.Days)
            .IsRequired()
            .HasColumnName("days");

        builder.Property(t => t.Price)
            .IsRequired()
            .HasColumnType("numeric(18,2)")
            .HasColumnName("price");

        builder.Property(t => t.Locations)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("locations");

        builder.Property(t => t.TourInfo)
            .IsRequired()
            .HasMaxLength(2000)
            .HasColumnName("tour_info");

        builder.Property(t => t.Pic)
            .HasMaxLength(500)
            .HasColumnName("pic");

        builder.Property(t => t.CreatedDate)
            .IsRequired()
            .HasColumnName("created_date")
            .HasDefaultValueSql("NOW()");

        builder.Property(t => t.ModifiedDate)
            .HasColumnName("modified_date");

        builder.Property(t => t.IsActive)
            .IsRequired()
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.HasMany(t => t.Bookings)
            .WithOne(b => b.Tour)
            .HasForeignKey(b => b.TourId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

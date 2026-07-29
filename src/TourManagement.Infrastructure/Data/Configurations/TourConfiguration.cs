using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core entity configuration for the Tour entity.
/// Updated for PostgreSQL compatibility.
/// </summary>
public class TourConfiguration : IEntityTypeConfiguration<Tour>
{
    public void Configure(EntityTypeBuilder<Tour> builder)
    {
        builder.ToTable("tour");

        builder.HasKey(t => t.TourId);

        builder.Property(t => t.TourId)
            .HasColumnName("tour_id")
            .UseIdentityColumn();

        builder.Property(t => t.TourName)
            .HasColumnName("tour_name")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(t => t.Place)
            .HasColumnName("place")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(t => t.Days)
            .HasColumnName("days")
            .IsRequired();

        builder.Property(t => t.Price)
            .HasColumnName("price")
            .HasColumnType("numeric(6,0)")
            .IsRequired();

        builder.Property(t => t.Locations)
            .HasColumnName("locations")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.TourInfo)
            .HasColumnName("tour_info")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.Pic)
            .HasColumnName("pic")
            .HasMaxLength(200);

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
    }
}

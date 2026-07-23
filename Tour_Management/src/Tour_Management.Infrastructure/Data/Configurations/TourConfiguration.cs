using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tour_Management.Domain.Entities;

namespace Tour_Management.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the Tour entity.
/// Configured for PostgreSQL compatibility with snake_case naming.
/// </summary>
public class TourConfiguration : IEntityTypeConfiguration<Tour>
{
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
            .HasMaxLength(20);

        builder.Property(t => t.Place)
            .HasColumnName("place")
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(t => t.Days)
            .HasColumnName("days")
            .IsRequired();

        builder.Property(t => t.Price)
            .HasColumnName("price")
            .IsRequired()
            .HasColumnType("numeric(10,2)");

        builder.Property(t => t.Locations)
            .HasColumnName("locations")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.TourInfo)
            .HasColumnName("tour_info")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.PicturePath)
            .HasColumnName("pic")
            .HasMaxLength(200);

        builder.Property(t => t.CreatedDate)
            .HasDefaultValueSql("now()");

        builder.Property(t => t.IsActive)
            .HasDefaultValue(true);

        builder.HasMany(t => t.Bookings)
            .WithOne()
            .HasForeignKey(b => b.TourName)
            .HasPrincipalKey(t => t.TourName)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

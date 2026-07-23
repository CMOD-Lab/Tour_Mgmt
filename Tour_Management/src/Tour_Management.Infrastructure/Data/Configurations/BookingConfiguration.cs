using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tour_Management.Domain.Entities;

namespace Tour_Management.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the Booking entity.
/// Configured for PostgreSQL compatibility with snake_case naming.
/// </summary>
public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("booking");

        builder.HasKey(b => b.BookingId);

        builder.Property(b => b.BookingId)
            .HasColumnName("tour_id")
            .ValueGeneratedOnAdd();

        builder.Property(b => b.TourName)
            .HasColumnName("tour_name")
            .HasMaxLength(50);

        builder.Property(b => b.Place)
            .HasColumnName("place")
            .HasMaxLength(50);

        builder.Property(b => b.Email)
            .HasColumnName("email")
            .HasMaxLength(50);

        builder.Property(b => b.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(50);

        builder.Property(b => b.CreatedDate)
            .HasDefaultValueSql("now()");

        builder.Property(b => b.IsActive)
            .HasDefaultValue(true);
    }
}

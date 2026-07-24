using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tour_Management.Domain.Entities;

namespace Tour_Management.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core entity configuration for the Booking entity.
/// Configured for PostgreSQL compatibility with snake_case naming conventions.
/// </summary>
public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("booking");

        builder.HasKey(b => b.BookingId);

        builder.Property(b => b.BookingId)
            .HasColumnName("booking_id")
            .ValueGeneratedOnAdd();

        builder.Property(b => b.TourName)
            .HasColumnName("tour_name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Place)
            .HasColumnName("place")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Email)
            .HasColumnName("email")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.FirstName)
            .HasColumnName("first_name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.TourId)
            .HasColumnName("tour_id");

        builder.Property(b => b.UserId)
            .HasColumnName("user_id");

        builder.Property(b => b.CreatedDate)
            .HasColumnName("created_date")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("NOW()");

        builder.Property(b => b.ModifiedDate)
            .HasColumnName("modified_date")
            .HasColumnType("timestamp with time zone");

        builder.Property(b => b.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);
    }
}

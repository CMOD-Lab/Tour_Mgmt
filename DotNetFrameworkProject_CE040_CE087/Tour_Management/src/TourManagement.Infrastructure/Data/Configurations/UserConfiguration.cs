using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core entity configuration for User.
/// Configured for PostgreSQL compatibility with snake_case naming.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user_info");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnName("email");

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("first_name");

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("last_name");

        builder.Property(u => u.Gender)
            .HasMaxLength(20)
            .HasColumnName("gender");

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(512)
            .HasColumnName("password");

        builder.Property(u => u.DateOfBirth)
            .HasColumnName("dob");

        builder.Property(u => u.Street)
            .HasMaxLength(200)
            .HasColumnName("street");

        builder.Property(u => u.City)
            .HasMaxLength(100)
            .HasColumnName("city");

        builder.Property(u => u.State)
            .HasMaxLength(100)
            .HasColumnName("state");

        builder.Property(u => u.IsAdmin)
            .IsRequired()
            .HasColumnName("is_admin")
            .HasDefaultValue(false);

        builder.Property(u => u.CreatedDate)
            .IsRequired()
            .HasColumnName("created_date")
            .HasDefaultValueSql("NOW()");

        builder.Property(u => u.ModifiedDate)
            .HasColumnName("modified_date");

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.HasMany(u => u.Bookings)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

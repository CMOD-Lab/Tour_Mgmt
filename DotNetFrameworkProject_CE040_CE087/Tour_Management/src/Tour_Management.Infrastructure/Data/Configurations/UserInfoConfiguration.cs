using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tour_Management.Domain.Entities;

namespace Tour_Management.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core entity configuration for the UserInfo entity.
/// Configured for PostgreSQL compatibility with snake_case naming conventions.
/// </summary>
public class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<UserInfo> builder)
    {
        builder.ToTable("user_info");

        builder.HasKey(u => u.UserId);

        builder.Property(u => u.UserId)
            .HasColumnName("user_id")
            .ValueGeneratedOnAdd();

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.FirstName)
            .HasColumnName("first_name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .HasColumnName("last_name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Gender)
            .HasColumnName("gender")
            .HasMaxLength(20);

        builder.Property(u => u.Password)
            .HasColumnName("password")
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.DateOfBirth)
            .HasColumnName("date_of_birth")
            .HasColumnType("timestamp with time zone");

        builder.Property(u => u.Street)
            .HasColumnName("street")
            .HasMaxLength(300);

        builder.Property(u => u.City)
            .HasColumnName("city")
            .HasMaxLength(100);

        builder.Property(u => u.State)
            .HasColumnName("state")
            .HasMaxLength(100);

        builder.Property(u => u.CreatedDate)
            .HasColumnName("created_date")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("NOW()");

        builder.Property(u => u.ModifiedDate)
            .HasColumnName("modified_date")
            .HasColumnType("timestamp with time zone");

        builder.Property(u => u.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.HasMany(u => u.Bookings)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

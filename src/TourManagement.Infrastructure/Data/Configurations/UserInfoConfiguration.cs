using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core entity configuration for the UserInfo entity.
/// Updated for PostgreSQL compatibility.
/// </summary>
public class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
{
    public void Configure(EntityTypeBuilder<UserInfo> builder)
    {
        builder.ToTable("user_info");

        builder.HasKey(u => u.Email);

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.Gender)
            .HasColumnName("gender")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(u => u.Password)
            .HasColumnName("password")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(u => u.Dob)
            .HasColumnName("dob")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(u => u.Street)
            .HasColumnName("street")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.City)
            .HasColumnName("city")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.State)
            .HasColumnName("state")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.CreatedDate)
            .HasColumnName("created_date")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("NOW()");

        builder.Property(u => u.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        // PostgreSQL-compatible check constraint using standard SQL syntax
        builder.HasCheckConstraint("ck_gender", "gender = 'Female' OR gender = 'Male'");

        // Navigation
        builder.HasMany(u => u.Bookings)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.Email)
            .HasPrincipalKey(u => u.Email)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

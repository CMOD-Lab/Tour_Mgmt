using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tour_Management.Domain.Entities;

namespace Tour_Management.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the UserInfo entity.
/// Configured for PostgreSQL compatibility with snake_case naming.
/// </summary>
public class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
{
    public void Configure(EntityTypeBuilder<UserInfo> builder)
    {
        builder.ToTable("user_info");

        builder.HasKey(u => u.Email);

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.FirstName)
            .HasColumnName("first_name")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.LastName)
            .HasColumnName("last_name")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Gender)
            .HasColumnName("gender")
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(u => u.Password)
            .HasColumnName("password")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.DateOfBirth)
            .HasColumnName("dob")
            .IsRequired();

        builder.Property(u => u.Street)
            .HasColumnName("street")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.City)
            .HasColumnName("city")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.State)
            .HasColumnName("state")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("now()");

        builder.Property(u => u.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        // PostgreSQL-compatible CHECK constraint syntax
        builder.HasCheckConstraint("ck_gender", "gender = 'Male' OR gender = 'Female'");

        builder.HasMany(u => u.Bookings)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.Email)
            .HasPrincipalKey(u => u.Email)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

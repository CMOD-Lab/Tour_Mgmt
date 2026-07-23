using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tour_Management.Domain.Entities;

namespace Tour_Management.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the UserInfo entity.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<UserInfo>
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

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.Dob)
            .HasColumnName("dob")
            .HasMaxLength(50);

        builder.Property(u => u.Street)
            .HasColumnName("street")
            .HasMaxLength(200);

        builder.Property(u => u.City)
            .HasColumnName("city")
            .HasMaxLength(100);

        builder.Property(u => u.State)
            .HasColumnName("state")
            .HasMaxLength(100);

        builder.Property(u => u.CreatedDate)
            .HasDefaultValueSql("NOW()");

        builder.Property(u => u.IsActive)
            .HasDefaultValue(true);

        builder.Property(u => u.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(100)
            .HasDefaultValue("system");

        builder.Property(u => u.ModifiedBy)
            .HasColumnName("modified_by")
            .HasMaxLength(100);

        builder.HasMany(u => u.Bookings)
            .WithOne()
            .HasForeignKey(b => b.Email)
            .HasPrincipalKey(u => u.Email)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

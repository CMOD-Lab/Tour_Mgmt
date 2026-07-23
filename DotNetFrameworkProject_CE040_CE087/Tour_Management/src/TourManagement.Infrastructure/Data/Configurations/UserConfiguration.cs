using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core entity configuration for the UserInfo entity.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<UserInfo>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<UserInfo> builder)
    {
        builder.ToTable("UserInfo");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Gender)
            .HasMaxLength(20);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("Password");

        builder.Property(u => u.Dob)
            .HasMaxLength(50);

        builder.Property(u => u.Street)
            .HasMaxLength(200);

        builder.Property(u => u.City)
            .HasMaxLength(100);

        builder.Property(u => u.State)
            .HasMaxLength(100);

        builder.Property(u => u.Role)
            .HasMaxLength(50)
            .HasDefaultValue("User");

        builder.Property(u => u.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(u => u.IsActive)
            .HasDefaultValue(true);

        builder.Property(u => u.CreatedBy)
            .HasMaxLength(100)
            .HasDefaultValue("system");

        builder.Property(u => u.ModifiedBy)
            .HasMaxLength(100);

        // Relationship: User has many Bookings
        builder.HasMany(u => u.Bookings)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

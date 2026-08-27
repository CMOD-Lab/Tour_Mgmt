using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core entity configuration for the UserInfo entity.
/// </summary>
public class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<UserInfo> builder)
    {
        builder.ToTable("UserInfo");

        builder.HasKey(u => u.Email);

        builder.Property(u => u.Email)
            .HasColumnName("Email")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.FirstName)
            .HasColumnName("FirstName")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .HasColumnName("LastName")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Gender)
            .HasColumnName("Gender")
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(u => u.Password)
            .HasColumnName("Password")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.DateOfBirth)
            .HasColumnName("dob")
            .IsRequired();

        builder.Property(u => u.Street)
            .HasColumnName("Street")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.City)
            .HasColumnName("City")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.State)
            .HasColumnName("State")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.IsActive)
            .HasColumnName("IsActive")
            .HasDefaultValue(true);

        builder.Property(u => u.CreatedDate)
            .HasColumnName("CreatedDate")
            .HasDefaultValueSql("GETUTCDATE()");

        // Relationship: UserInfo has many Bookings
        builder.HasMany(u => u.Bookings)
            .WithOne()
            .HasForeignKey(b => b.Email)
            .HasPrincipalKey(u => u.Email)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core entity configuration for User.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("UserInfo");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnName("Email");

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("FirstName");

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("LastName");

        builder.Property(u => u.Gender)
            .HasMaxLength(20)
            .HasColumnName("Gender");

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(512)
            .HasColumnName("Password");

        builder.Property(u => u.DateOfBirth)
            .HasColumnName("dob");

        builder.Property(u => u.Street)
            .HasMaxLength(200)
            .HasColumnName("Street");

        builder.Property(u => u.City)
            .HasMaxLength(100)
            .HasColumnName("City");

        builder.Property(u => u.State)
            .HasMaxLength(100)
            .HasColumnName("State");

        builder.Property(u => u.IsAdmin)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(u => u.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasMany(u => u.Bookings)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourBooking.Domain.Entities;

namespace TourBooking.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core entity configuration for UserInfo.
/// </summary>
public class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<UserInfo> builder)
    {
        builder.ToTable("UserInfo", t =>
            t.HasCheckConstraint("CK_Gender", "[Gender] = 'Male' OR [Gender] = 'Female'"));

        builder.HasKey(u => u.Email);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Gender)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Dob)
            .IsRequired()
            .HasColumnName("dob");

        builder.Property(u => u.Street)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.City)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.State)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasMany(u => u.Bookings)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.Email)
            .HasPrincipalKey(u => u.Email)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

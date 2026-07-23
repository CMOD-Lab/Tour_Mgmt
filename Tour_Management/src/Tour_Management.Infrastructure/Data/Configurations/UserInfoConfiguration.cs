using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tour_Management.Domain.Entities;

namespace Tour_Management.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the UserInfo entity.
/// </summary>
public class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
{
    public void Configure(EntityTypeBuilder<UserInfo> builder)
    {
        builder.ToTable("UserInfo");

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
            .HasMaxLength(200);

        builder.Property(u => u.DateOfBirth)
            .HasColumnName("dob")
            .IsRequired();

        builder.Property(u => u.Street)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.City)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.State)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(u => u.IsActive)
            .HasDefaultValue(true);

        builder.HasCheckConstraint("CK_Gender", "[Gender] = 'Male' OR [Gender] = 'Female'");

        builder.HasMany(u => u.Bookings)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.Email)
            .HasPrincipalKey(u => u.Email)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

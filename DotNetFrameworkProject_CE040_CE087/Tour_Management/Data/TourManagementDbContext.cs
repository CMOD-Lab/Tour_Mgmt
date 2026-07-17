using Microsoft.EntityFrameworkCore;
using Tour_Management.Models;

namespace Tour_Management.Data
{
    /// <summary>
    /// Entity Framework Core DbContext for Tour Management application.
    /// Configured with Azure SQL Database connection resiliency and retry logic
    /// to replace direct SqlConnection management.
    /// </summary>
    public class TourManagementDbContext : DbContext
    {
        public TourManagementDbContext(DbContextOptions<TourManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tour> Tours { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<UserInfo> UserInfos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tour entity configuration
            modelBuilder.Entity<Tour>(entity =>
            {
                entity.ToTable("Tour");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TourName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Place).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            });

            // Booking entity configuration
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("booking");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TourName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Place).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(200);
            });

            // UserInfo entity configuration
            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.ToTable("UserInfo");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(200);
            });
        }
    }
}

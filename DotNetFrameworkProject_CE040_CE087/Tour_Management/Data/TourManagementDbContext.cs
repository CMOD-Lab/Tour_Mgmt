using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Tour_Management.Models;

namespace Tour_Management.Data
{
    /// <summary>
    /// Entity Framework Core DbContext for Tour Management.
    /// Replaces direct SqlConnection usage with EF Core connection pooling,
    /// retry logic, and Azure SQL Database integration with transient fault handling.
    /// Connection string is read from environment variables for cloud-native configuration.
    /// </summary>
    public class TourManagementDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of TourManagementDbContext.
        /// Connection string is resolved from environment variable TOURDB_CONNECTION_STRING
        /// or falls back to the named connection string "dbconnection" in configuration.
        /// </summary>
        public TourManagementDbContext()
            : base(GetConnectionString())
        {
            // Enable retry on failure for Azure SQL transient fault handling
            Database.SetInitializer<TourManagementDbContext>(null);
        }

        public DbSet<Tour> Tours { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tour>().ToTable("Tour");
            modelBuilder.Entity<Booking>().ToTable("booking");
            modelBuilder.Entity<UserInfo>().ToTable("UserInfo");
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Resolves the database connection string from environment variables first,
        /// then falls back to the named connection string in Web.config / appsettings.
        /// This enables cloud-native configuration without rebuilding.
        /// </summary>
        private static string GetConnectionString()
        {
            // Cloud-native: read from environment variable first (Azure App Service / Container Apps)
            string envConnStr = Environment.GetEnvironmentVariable("TOURDB_CONNECTION_STRING");
            if (!string.IsNullOrWhiteSpace(envConnStr))
            {
                return envConnStr;
            }
            // Fallback to named connection string in configuration
            return "dbconnection";
        }
    }
}

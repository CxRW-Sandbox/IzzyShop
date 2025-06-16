using Microsoft.EntityFrameworkCore;
using IzzyShop.Models;

namespace IzzyShop.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Vulnerable: No proper constraints on sensitive data
            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired();

            // Vulnerable: No proper constraints on price
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            // Vulnerable: No proper constraints on review content
            modelBuilder.Entity<Review>()
                .Property(r => r.Content)
                .IsRequired();
        }
    }
} 
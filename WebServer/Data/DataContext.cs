using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Web.Models;

namespace Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Rate> Rates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
            });

            modelBuilder.Entity<Rate>().ToTable("Rates");
            modelBuilder.Entity<Rate>(entity =>
            {
                entity.HasKey(p => p.Id);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

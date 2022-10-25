using eShop.ProductApi.DataAccess.Mappings;
using eShop.ProductApi.Entity;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.DataAccess
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<CategoryEntity> Category { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryMapping());
            modelBuilder.ApplyConfiguration(new ProductMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}

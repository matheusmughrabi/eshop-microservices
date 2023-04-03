using eShop.ProductApi.DataAccess.Mappings;
using eShop.ProductApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.DataAccess
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<CategoryGroupEntity> CategoryGroup { get; set; }
        public DbSet<CategoryEntity> Category { get; set; }
        public DbSet<ProductEntity> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryMapping());
            modelBuilder.ApplyConfiguration(new ProductMapping());
            modelBuilder.ApplyConfiguration(new CategoryGroupMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}

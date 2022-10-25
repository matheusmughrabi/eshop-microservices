using eShop.ProductApi.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.ProductApi.DataAccess.Mappings
{
    public class ProductMapping : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.ToTable("Product");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                    .IsRequired()
                    .HasColumnType("nvarchar(100)");

            builder.Property(c => c.Description)
                    .HasColumnType("nvarchar(500)");

            builder.Property(c => c.Price)
                .IsRequired()
                .HasColumnType("decimal(19,5)");
        }
    }
}

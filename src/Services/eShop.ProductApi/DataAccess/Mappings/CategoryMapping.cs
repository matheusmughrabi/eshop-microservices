using eShop.ProductApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.ProductApi.DataAccess.Mappings
{
    public class CategoryMapping : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("Category");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                    .IsRequired()
                    .HasColumnType("nvarchar(100)");

            builder.Property(c => c.Description)
                    .HasColumnType("nvarchar(500)");

            builder
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}

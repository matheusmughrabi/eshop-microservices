using eShop.ProductApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.ProductApi.DataAccess.Mappings;

public class CategoryGroupMapping : IEntityTypeConfiguration<CategoryGroupEntity>
{
    public void Configure(EntityTypeBuilder<CategoryGroupEntity> builder)
    {
        builder.ToTable("CategoryGroup");

        builder.HasKey(c => c.Id);

        builder
            .HasMany(c => c.Categories)
            .WithOne(p => p.CategoryGroup)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}

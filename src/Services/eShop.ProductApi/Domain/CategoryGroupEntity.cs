using eShop.ProductApi.Entity;

namespace eShop.ProductApi.Domain;

public class CategoryGroupEntity : BaseEntity
{
    public string Name { get; set; }
    public List<CategoryEntity> Categories { get; set; }
}

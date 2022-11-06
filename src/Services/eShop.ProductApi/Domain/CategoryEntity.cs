using eShop.ProductApi.Exceptions;

namespace eShop.ProductApi.Entity
{
    public class CategoryEntity : Entity
    {
        protected CategoryEntity()
        {
        }

        public CategoryEntity(string name, string? description = null)
        {
            if (name is null)
                throw new InvalidPropertyValueException("name cannot be null.");

            Name = name;
            Description = description;
        }

        public string Name { get; set; }
        public string? Description { get; set; }
        public List<ProductEntity> Products { get; set; }
    }
}

using eShop.ProductApi.Domain.Validations;

namespace eShop.ProductApi.Entity
{
    public class CategoryEntity : Entity
    {
        protected CategoryEntity()
        {
        }

        public CategoryEntity(string name, string? description = null)
        {
            CategoryValidations.ValidateIfNullOrEmptyName(name);
            CategoryValidations.ValidateIfNameIsTooLong(name);

            Name = name;
            Description = description;
        }

        public string Name { get; private set; }
        public string? Description { get; private set; }
        public List<ProductEntity> Products { get; set; }

        public void Update(string name, string? description = null)
        {
            CategoryValidations.ValidateIfNullOrEmptyName(name);
            CategoryValidations.ValidateIfNameIsTooLong(name);

            Name = name;
            Description = description;
        }
    }
}

using eShop.ProductApi.Domain.Validations;

namespace eShop.ProductApi.Entity
{
    public class ProductEntity : Entity
    {
        protected ProductEntity() { }

        public ProductEntity(string name, decimal price, string? description = null)
        {
            ProductValidations.ValidateIfNullName(name);
            ProductValidations.ValidateIfPriceEqualOrLowerThanZero(price);

            Name = name;
            Description = description;
            Price = price;
        }

        public string Name { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public CategoryEntity Category { get; set; }

        public void Update(string name, decimal price, string? description = null)
        {
            ProductValidations.ValidateIfNullName(name);
            ProductValidations.ValidateIfPriceEqualOrLowerThanZero(price);

            Name = name;
            Description = description;
            Price = price;
        }

        public override bool Equals(object? obj)
        {
            var other = obj as ProductEntity;

            return Name == other?.Name || Id == other?.Id;    
        }
    }
}

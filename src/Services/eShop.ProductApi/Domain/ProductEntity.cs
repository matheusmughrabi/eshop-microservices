using eShop.ProductApi.Exceptions;

namespace eShop.ProductApi.Entity
{
    public class ProductEntity : Entity
    {
        protected ProductEntity() { }

        public ProductEntity(string name, decimal price, string description = null)
        {
            if (name is null)
                throw new InvalidPropertyValueException("name cannot be null.");

            if (price <= 0)
                throw new InvalidPropertyValueException("price must be greater than zero.");

            Name = name;
            Description = description;
            Price = price;
        }

        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public CategoryEntity Category { get; set; }

        public override bool Equals(object? obj)
        {
            var other = obj as ProductEntity;

            return Name == other?.Name || Id == other?.Id;    
        }
    }
}

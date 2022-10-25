using eShop.ProductApi.Exceptions;
using eShop.ProductApi.Guards;

namespace eShop.ProductApi.Entity
{
    public class ProductEntity : Entity
    {
        protected ProductEntity() { }

        public ProductEntity(string name, decimal price, string description = null)
        {
            ProductGuards.NameNull(name);
            ProductGuards.PriceEqualOrLessThanZero(price);

            Name = name;
            Description = description;
            Price = price;
        }

        public string Name { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public CategoryEntity Category { get; private set; }

        public override bool Equals(object? obj)
        {
            var other = obj as ProductEntity;

            return Name == other?.Name || Id == other?.Id;    
        }
    }
}

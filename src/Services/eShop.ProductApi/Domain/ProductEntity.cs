using eShop.ProductApi.Exceptions;

namespace eShop.ProductApi.Entity
{
    public class ProductEntity : Entity
    {
        protected ProductEntity() { }

        public ProductEntity(string name, decimal price, string? description = null)
        {
            GuardAgainstNullName(name);
            GuardAgainstPriceEqualOrLowerThanZero(price);

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
            GuardAgainstNullName(name);
            GuardAgainstPriceEqualOrLowerThanZero(price);

            Name = name;
            Description = description;
            Price = price;
        }

        private void GuardAgainstNullName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new InvalidPropertyValueException("Name cannot be null or empty.");
        }

        private void GuardAgainstPriceEqualOrLowerThanZero(decimal price)
        {
            if (price <= 0)
                throw new InvalidPropertyValueException("Price must be greater than zero.");
        }

        public override bool Equals(object? obj)
        {
            var other = obj as ProductEntity;

            return Name == other?.Name || Id == other?.Id;    
        }
    }
}

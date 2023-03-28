using eShop.ProductApi.Exceptions;

namespace eShop.ProductApi.Entity
{
    public class ProductEntity : BaseEntity
    {
        protected ProductEntity() { }

        public ProductEntity(string name, decimal price, string? description = null, string? imagePath = null)
        {
            GuardAgainstNullName(name);
            GuardAgainstPriceEqualOrLowerThanZero(price);

            Name = name;
            Description = description;
            Price = price;
            ImagePath = imagePath;
            // When the product has just been created, the quantity on hand is zero
            QuantityOnHand = 0; 
        }

        public string Name { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public string? ImagePath { get; set; }
        public int QuantityOnHand { get; private set; }
        public Guid? CategoryId { get; private set; }
        public CategoryEntity? Category { get; }

        public void Update(string name, decimal price, string? description = null)
        {
            GuardAgainstNullName(name);
            GuardAgainstPriceEqualOrLowerThanZero(price);

            Name = name;
            Description = description;
            Price = price;
        }

        public void ChangeCategory(Guid categoryId)
        {
            CategoryId = categoryId;
        }

        public void AddStock(int quantity)
        {
            if(quantity <= 0)
                throw new InvalidPropertyValueException("quantity must be greater than zero.");

            QuantityOnHand += quantity;
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

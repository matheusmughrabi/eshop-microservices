using eShop.ProductApi.Guards;
using eShop.ProductApi.Interfaces;

namespace eShop.ProductApi.Entity
{
    public class CategoryEntity : Entity, IAggregateRoot
    {
        private readonly List<ProductEntity> _products  = new List<ProductEntity>();

        protected CategoryEntity()
        {
        }

        public CategoryEntity(string name, string? description = null)
        {
            CategoryGuards.CategoryNameNull(name);

            Name = name;
            Description = description;
        }

        public string Name { get; private set; }
        public string? Description { get; private set; }
        public IReadOnlyCollection<ProductEntity> Products => _products.AsReadOnly();

        public void AddProduct(ProductEntity product)
        {
            CategoryGuards.DuplicateProductInCategory(_products, product);

            _products.Add(product);
        }

        public void Update(string name, string description)
        {
            CategoryGuards.CategoryNameNull(name);
            Name = name;
            Description = description;
        }
    }
}

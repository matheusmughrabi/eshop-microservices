using eShop.ProductApi.Entity;
using eShop.ProductApi.Exceptions;

namespace eShop.ProductApi.Guards
{
    public static class CategoryGuards
    {
        public static void CategoryNameNull(string name)
        {
            if (name is null)
                throw new InvalidPropertyValueException("name cannot be null.");
        }

        public static void DuplicateProductInCategory(List<ProductEntity> _products, ProductEntity product)
        {
            if (_products.Any(p => product.Equals(p)))
                throw new DuplicateProductException("product already exists in this category");
        }
    }
}

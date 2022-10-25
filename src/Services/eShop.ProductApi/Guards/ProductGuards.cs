using eShop.ProductApi.Exceptions;

namespace eShop.ProductApi.Guards
{
    public static class ProductGuards
    {
        public static void NameNull(string name)
        {
            if (name is null)
                throw new InvalidPropertyValueException("name cannot be null.");
        }

        public static void PriceEqualOrLessThanZero(decimal price)
        {
            if (price <= 0)
                throw new InvalidPropertyValueException("price must be greater than zero.");
        }
    }
}

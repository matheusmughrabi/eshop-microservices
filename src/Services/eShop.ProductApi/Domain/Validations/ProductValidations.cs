using eShop.ProductApi.Exceptions;
using eShop.ProductApi.Notifications;

namespace eShop.ProductApi.Domain.Validations
{
    public static class ProductValidations
    {
        /// <summary>
        /// Throws an InvalidPropertyValueException if name is null or empty
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="InvalidPropertyValueException"></exception>
        public static void ValidateIfNullName(string name)
        {
            if (string.IsNullOrEmpty(name))
                    throw new InvalidPropertyValueException("Name cannot be null or empty.");
        }

        /// <summary>
        /// Adds a warning notification to the notifications list if name is null or empty
        /// </summary>
        /// <param name="notifications"></param>
        /// <param name="name"></param>
        public static void ValidateIfNullName(this List<Notification> notifications, string name)
        {
            if (string.IsNullOrEmpty(name))
                notifications.Add(new Notification() { Message = "Name cannot be null or empty.", Type = ENotificationType.Warning });
        }

        /// <summary>
        /// Adds an InvalidPropertyValueException if price is lower or equal than zero
        /// </summary>
        /// <param name="price"></param>
        /// <exception cref="InvalidPropertyValueException"></exception>
        public static void ValidateIfPriceEqualOrLowerThanZero(decimal price)
        {
            if (price <= 0)
                throw new InvalidPropertyValueException("Price must be greater than zero.");
        }

        /// <summary>
        /// Adds a warning notification to the notifications list if price is lower or equal than zero
        /// </summary>
        /// <param name="notifications"></param>
        /// <param name="price"></param>
        public static void ValidateIfPriceEqualOrLowerThanZero(this List<Notification> notifications, decimal price)
        {
            if (price <= 0)
                notifications.Add(new Notification() { Message = "Price must be greater than zero.", Type = ENotificationType.Warning });
        }
    }
}

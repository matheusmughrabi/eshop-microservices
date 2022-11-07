using eShop.ProductApi.Exceptions;
using eShop.ProductApi.Notifications;

namespace eShop.ProductApi.Domain.Validations
{
    public static class CategoryValidations
    {
        public static void ValidateIfNullOrEmptyName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new InvalidPropertyValueException("Name cannot be null or empty.");
        }

        public static void ValidateIfNullOrEmptyName(this List<Notification> notifications, string name)
        {
            if (string.IsNullOrEmpty(name))
                notifications.Add(new Notification() { Message = "Name cannot be null or empty.", Type = ENotificationType.Warning });
        }

        public static void ValidateIfNameIsTooLong(string name)
        {
            if (name.Length > 100)
                throw new InvalidPropertyValueException("Name length is limited to 100 characters.");
        }

        public static void ValidateIfNameIsTooLong(this List<Notification> notifications, string name)
        {
            if (name.Length > 100)
                notifications.Add(new Notification() { Message = "Name length is limited to 100 characters.", Type = ENotificationType.Warning });
        }
    }
}

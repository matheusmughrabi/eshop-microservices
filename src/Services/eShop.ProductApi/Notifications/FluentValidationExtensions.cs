using FluentValidation.Results;

namespace eShop.ProductApi.Notifications
{
    public static class FluentValidationExtensions
    {
        public static List<Notification> ToNotifications(this ValidationResult result)
        {
            var notifications = new List<Notification>();
            foreach (var error in result.Errors)
            {
                notifications.Add(new Notification() { Message = error.ErrorMessage, Type = error.Severity.ToNotificationType() });
            }
            return notifications;
        }

        public static ENotificationType ToNotificationType(this FluentValidation.Severity severity)
        {
            switch (severity)
            {
                case FluentValidation.Severity.Error:
                    return ENotificationType.Error;
                case FluentValidation.Severity.Warning:
                    return ENotificationType.Warning;
                case FluentValidation.Severity.Info:
                    return ENotificationType.Informative;
                default:
                    throw new ArgumentException("Invalid severity value");
            }
        }
    }
}

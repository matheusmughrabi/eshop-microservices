namespace eShop.ProductApi.Notifications
{
    public class Notification
    {
        public string Message { get; set; }
        public ENotificationType Type { get; set; }
    }

    public enum ENotificationType
    {
        Informative = 1,
        Warning = 2,
        Error = 3,
    }
}

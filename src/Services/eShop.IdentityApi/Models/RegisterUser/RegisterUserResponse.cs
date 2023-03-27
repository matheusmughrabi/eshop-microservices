namespace eShop.IdentityApi.Models.RegisterUser;

public class RegisterUserResponse
{
    public List<Notification> Notifications { get; set; }
}

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
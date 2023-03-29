namespace eShop.EventBus.Configuration;

public class RabbitMQConfiguration
{
    public string Host { get; set; }
    public string Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string CliendProvidedName { get; set; }
}

using eShop.EventBus.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;

namespace eShop.EventBus.Implementation;

public class RabbitMessageBus : IMessageBus
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly RabbitMQConfiguration _rabbitMQConfig;

    public RabbitMessageBus(IOptions<RabbitMQConfiguration> rabbitMQConfig)
    {
        _rabbitMQConfig = rabbitMQConfig.Value;
    }

    public IConnection GetConnection()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMQConfig.Host,
            Port = int.Parse(_rabbitMQConfig.Port),
            UserName = _rabbitMQConfig.Username,
            Password = _rabbitMQConfig.Password,
            ClientProvidedName = _rabbitMQConfig.CliendProvidedName
        };

        return factory.CreateConnection();
    }

    public IModel GetChannel(IConnection connection)
    {
        return connection.CreateModel();
    }

    public void Consume()
    {

    }
}

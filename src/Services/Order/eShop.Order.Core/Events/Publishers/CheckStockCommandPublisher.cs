using eShop.EventBus.Implementation;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using eShop.EventBus.Base;
using eShop.EventBus.Messages;
using eShop.EventBus.Constants;

namespace eShop.Order.Core.Events.Publishers;

public class CheckStockCommandPublisher : IEventPublisher<CheckStockCommandMessage>
{
    private const string EXCHANGENAME = ExchangeConstants.OrderExchange;
    private const string ROUTINGKEY = RoutingKeysConstants.CheckStock;

    private readonly IMessageBus _messageBus;

    public CheckStockCommandPublisher(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public void Publish(CheckStockCommandMessage eventMessage)
    {
        using (var connection = _messageBus.GetConnection())
        using (var channel = _messageBus.GetChannel(connection))
        {
            channel.ExchangeDeclare(exchange: EXCHANGENAME, type: ExchangeType.Direct, durable: true, autoDelete: false);

            var jsonMessage = JsonSerializer.Serialize(eventMessage);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: EXCHANGENAME, routingKey: ROUTINGKEY, basicProperties: properties, body: body);
        }
    }
}

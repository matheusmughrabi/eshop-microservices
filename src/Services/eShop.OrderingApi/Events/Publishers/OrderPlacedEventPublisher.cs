using eShop.EventBus.Events.Base;
using eShop.EventBus.Events.Messages;
using eShop.EventBus.Implementation;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace eShop.OrderingApi.Events.Publishers;

public class OrderPlacedEventPublisher : IEventPublisher<OrderPlacedEventMessage>
{
    private const string EXCHANGENAME = "orderPlacedExchange";
    private const string ROUTINGKEY = "orderPlacedRoutingKey";
    private const string QUEUENAME = "orderPlacedQueue";

    private readonly IMessageBus _messageBus;

    public OrderPlacedEventPublisher(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public void Publish(OrderPlacedEventMessage eventMessage)
    {
        using (var connection = _messageBus.GetConnection())
        using (var channel = _messageBus.GetChannel(connection))
        {
            channel.ExchangeDeclare(exchange: EXCHANGENAME, type: ExchangeType.Direct, durable: true, autoDelete: false);
            channel.QueueDeclare(queue: QUEUENAME, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: QUEUENAME, exchange: EXCHANGENAME, routingKey: ROUTINGKEY);

            var jsonMessage = JsonSerializer.Serialize(eventMessage);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            channel.BasicPublish(exchange: EXCHANGENAME, routingKey: ROUTINGKEY, basicProperties: null, body: body);
        }
    }
}

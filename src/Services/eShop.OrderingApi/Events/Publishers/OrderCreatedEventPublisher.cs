using eShop.EventBus.Events.Base;
using eShop.EventBus.Events.Messages;
using eShop.EventBus.Implementation;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace eShop.OrderingApi.Events.Publishers;

public class OrderCreatedEventPublisher : IEventPublisher<OrderCreatedEventMessage>
{
    private const string EXCHANGENAME = "orderCreatedExchange";
    private const string ROUTINGKEY = "orderCreatedRoutingKey";
    private const string QUEUENAME = "orderCreatedQueue";

    private readonly IMessageBus _messageBus;

    public OrderCreatedEventPublisher(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public void Publish(OrderCreatedEventMessage eventMessage)
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

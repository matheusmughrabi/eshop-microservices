using eShop.EventBus.Events.Base;
using eShop.EventBus.Implementation;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using eShop.EventBus.Events.BasketCheckout;

namespace eShop.BasketApi.Events.Publishers;

public class BasketCheckoutEventPublisher : IEventPublisher<BasketCheckoutEventMessage>
{
    private const string EXCHANGENAME = "basketCheckoutExchange";
    private const string ROUTINGKEY = "basketCheckoutRoutingKey";
    private const string QUEUENAME = "basketCheckoutQueue";

    private readonly IMessageBus _messageBus;

    public BasketCheckoutEventPublisher(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public void Publish(BasketCheckoutEventMessage eventMessage)
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

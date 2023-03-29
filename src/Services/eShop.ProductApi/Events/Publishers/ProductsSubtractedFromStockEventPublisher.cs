﻿using eShop.EventBus.Events.Base;
using eShop.EventBus.Events.Messages;
using eShop.EventBus.Implementation;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace eShop.ProductApi.Events.Publishers;

public class ProductsSubtractedFromStockEventPublisher : IEventPublisher<ProductsSubtractedFromStockEventMessage>
{
    private const string EXCHANGENAME = "productsSubtractedFromStockExchange";
    private const string ROUTINGKEY = "productsSubtractedFromStockRoutingKey";
    private const string QUEUENAME = "productsSubtractedFromStockQueue";

    private readonly IMessageBus _messageBus;

    public ProductsSubtractedFromStockEventPublisher(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public void Publish(ProductsSubtractedFromStockEventMessage eventMessage)
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

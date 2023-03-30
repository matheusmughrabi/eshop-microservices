using eShop.EventBus.Events.Messages;
using eShop.EventBus.Implementation;
using eShop.ProductApi.Features.Product;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace eShop.OrderingApi.Events.Consumers;

public class OrderPlacedEventConsumer : IHostedService
{
    private const string EXCHANGENAME = "orderPlacedExchange";
    private const string ROUTINGKEY = "orderPlacedRoutingKey";
    private const string QUEUENAME = "orderPlacedQueue";

    private readonly IMessageBus _messageBus;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public OrderPlacedEventConsumer(IMessageBus messageBus, IServiceScopeFactory serviceScopeFactory)
    {
        _messageBus = messageBus;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Factory.StartNew(() =>
        {
            using (var connection = _messageBus.GetConnection())
            using (var channel = _messageBus.GetChannel(connection))
            {
                channel.ExchangeDeclare(exchange: EXCHANGENAME, type: ExchangeType.Direct, durable: true, autoDelete: false);
                channel.QueueDeclare(queue: QUEUENAME, durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind(queue: QUEUENAME, exchange: EXCHANGENAME, routingKey: ROUTINGKEY);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        IMediator scopedMediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);

                        OrderCreatedEventMessage eventMessageObject = System.Text.Json.JsonSerializer.Deserialize<OrderCreatedEventMessage>(message);

                        var subtractFromStockComand = new SubtractFromStockCommand()
                        {
                            OrderId = eventMessageObject.OrderId,
                            Products = eventMessageObject.Products.Select(c => new SubtractFromStockCommand.Product()
                            {
                                Id = Guid.Parse(c.Id),
                                Quantity = c.Quantity
                            }).ToList()
                        };
                        
                        var response = await scopedMediator.Send(subtractFromStockComand);
                    }
                };

                channel.BasicConsume(queue: QUEUENAME,
                                     autoAck: true,
                                     consumer: consumer);

                while (!cancellationToken.IsCancellationRequested)
                {
                    Thread.Sleep(1000);
                }
            }
        }, cancellationToken);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

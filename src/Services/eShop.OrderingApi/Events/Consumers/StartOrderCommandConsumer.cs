using eShop.EventBus.Constants;
using eShop.EventBus.Implementation;
using eShop.EventBus.Messages;
using eShop.OrderingApi.Application.CreateOrder;
using eShop.OrderingApi.Events.Constants;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace eShop.OrderingApi.Events.EventConsumers;

public class StartOrderCommandConsumer : IHostedService
{
    private const string EXCHANGENAME = ExchangeConstants.OrderExchange;
    private const string ROUTINGKEY = RoutingKeysConstants.StartOrder;
    private const string QUEUENAME = QueueConstants.StartOrder;

    private readonly IMessageBus _messageBus;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public StartOrderCommandConsumer(IMessageBus messageBus, IServiceScopeFactory serviceScopeFactory)
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

                        StartOrderCommandMessage startOrderCommandMessage = System.Text.Json.JsonSerializer.Deserialize<StartOrderCommandMessage>(message);

                        var createOrderCommand = new CreateOrderCommand()
                        {
                            UserId = startOrderCommandMessage.UserId,
                            Products = startOrderCommandMessage.Products.Select(product => new CreateOrderCommand.Product()
                            {
                                Id = product.Id,
                                ImagePath = product.ImagePath,
                                Name = product.Name,
                                PriceAtPurchase = product.PriceAtPurchase,
                                Quantity = product.Quantity
                            }).ToList()
                        };
                        
                        var response = await scopedMediator.Send(createOrderCommand);

                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                };

                channel.BasicConsume(queue: QUEUENAME,
                                     autoAck: false,
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

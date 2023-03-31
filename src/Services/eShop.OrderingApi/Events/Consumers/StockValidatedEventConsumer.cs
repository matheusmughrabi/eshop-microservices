using eShop.EventBus.Events.Messages;
using eShop.EventBus.Implementation;
using eShop.OrderingApi.Application.UpdateOrder;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace eShop.OrderingApi.Events.Consumers;

public class StockValidatedEventConsumer : IHostedService
{
    private const string EXCHANGENAME = "stockValidatedExchange";
    private const string ROUTINGKEY = "stockValidatedRoutingKey";
    private const string QUEUENAME = "stockValidatedQueue";

    private readonly IMessageBus _messageBus;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public StockValidatedEventConsumer(IMessageBus messageBus, IServiceScopeFactory serviceScopeFactory)
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

                        StockValidatedEventMessage eventMessageObject = System.Text.Json.JsonSerializer.Deserialize<StockValidatedEventMessage>(message);

                        var command = new UpdateOrderCommand();
                        command.Id = eventMessageObject.OrderId;

                        if (eventMessageObject.Success)
                        {
                            command.Status = Domain.Enums.OrderStatusEnum.Placed;
                        }
                        else
                        {
                            command.Status = Domain.Enums.OrderStatusEnum.Invalid;

                            command.Notifications = new List<UpdateOrderCommand.Notification>();
                            foreach (var product in eventMessageObject.UnderstockedProducts)
                            {
                                command.Notifications.Add(new UpdateOrderCommand.Notification() { Description = $"Not enough stock of product {product.Name} for this order" });
                            }

                        }
                        
                        var response = await scopedMediator.Send(command);
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

using eShop.EventBus.Events.Base;
using eShop.EventBus.Implementation;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using MediatR;
using System.Text;
using eShop.OrderingApi.Application.UpdateOrderStatus;
using eShop.EventBus.Events.Messages;

namespace eShop.OrderingApi.Events.Consumers;

public class ProductsSubtractedFromStockEventConsumer : IHostedService
{
    private const string EXCHANGENAME = "productsSubtractedFromStockExchange";
    private const string ROUTINGKEY = "productsSubtractedFromStockRoutingKey";
    private const string QUEUENAME = "productsSubtractedFromStockQueue";

    private readonly IMessageBus _messageBus;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ProductsSubtractedFromStockEventConsumer(IMessageBus messageBus, IServiceScopeFactory serviceScopeFactory)
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

                        ProductsSubtractedFromStockEventMessage eventMessageObject = System.Text.Json.JsonSerializer.Deserialize<ProductsSubtractedFromStockEventMessage>(message);

                        var command = new UpdateOrderStatusCommand();
                        command.Id = eventMessageObject.OrderId;

                        if (eventMessageObject.Success)
                        {
                            command.Status = Domain.Enums.OrderStatusEnum.Placed;
                        }
                        else
                        {
                            command.Status = Domain.Enums.OrderStatusEnum.Invalid;
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

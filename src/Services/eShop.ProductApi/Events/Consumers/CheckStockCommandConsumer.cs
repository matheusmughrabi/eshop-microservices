using eShop.EventBus.Constants;
using eShop.EventBus.Implementation;
using eShop.EventBus.Messages;
using eShop.ProductApi.Events.Constants;
using eShop.ProductApi.Features.Product;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace eShop.ProductApi.Events.Consumers;

public class CheckStockCommandConsumer : IHostedService
{
    private const string EXCHANGENAME = ExchangeConstants.OrderExchange;
    private const string ROUTINGKEY = RoutingKeysConstants.CheckStock;
    private const string QUEUENAME = QueueConstants.CheckStock;

    private readonly IMessageBus _messageBus;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CheckStockCommandConsumer(IMessageBus messageBus, IServiceScopeFactory serviceScopeFactory)
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

                        CheckStockCommandMessage eventMessageObject = System.Text.Json.JsonSerializer.Deserialize<CheckStockCommandMessage>(message);

                        var checkStockCommand = new CheckStockCommand()
                        {
                            OrderId = eventMessageObject.OrderId,
                            Products = eventMessageObject.Products.Select(c => new CheckStockCommand.Product()
                            {
                                Id = Guid.Parse(c.Id),
                                Quantity = c.Quantity
                            }).ToList()
                        };

                        var response = await scopedMediator.Send(checkStockCommand);

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

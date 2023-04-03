using eShop.EventBus.Constants;
using eShop.EventBus.Implementation;
using eShop.EventBus.Messages;
using eShop.Order.Core.Application.CreateOrder;
using eShop.Order.Core.Application.UpdateOrder;
using eShop.Order.Core.Domain.Enums;
using eShop.Order.Core.Events.Constants;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace eShop.Order.Messages.Worker.Consumers
{
    public class StockValidatedEventConsumer : BackgroundService
    {
        private readonly ILogger<StartOrderCommandConsumer> _logger;
        private const string EXCHANGENAME = ExchangeConstants.OrderExchange;
        private const string ROUTINGKEY = RoutingKeysConstants.Order_StockValidated;
        private const string QUEUENAME = QueueConstants.StockValidated;

        private readonly IMessageBus _messageBus;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public StockValidatedEventConsumer(
            IMessageBus messageBus, 
            IServiceScopeFactory serviceScopeFactory, ILogger<StartOrderCommandConsumer> logger)
        {
            _messageBus = messageBus;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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
                            command.Status = OrderStatusEnum.Placed;
                        }
                        else
                        {
                            command.Status = OrderStatusEnum.Invalid;

                            command.Notifications = new List<UpdateOrderCommand.Notification>();
                            foreach (var product in eventMessageObject.UnderstockedProducts)
                            {
                                command.Notifications.Add(new UpdateOrderCommand.Notification() { Description = $"Not enough stock of product {product.Name} for this order" });
                            }

                        }

                        var response = await scopedMediator.Send(command);

                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                };

                channel.BasicConsume(queue: QUEUENAME,
                                     autoAck: false,
                                     consumer: consumer);

                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}
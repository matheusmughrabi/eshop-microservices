using eShop.EventBus.Configuration;
using eShop.EventBus.Implementation;
using eShop.Order.Core.Application.CreateOrder;
using eShop.Order.Core.Events.Publishers;
using eShop.Order.Core.Repository;
using eShop.Order.Messages.Worker.Consumers;
using MediatR;
using MongoDB.Driver;
using System.Reflection;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.AddHostedService<StartOrderCommandConsumer>();
        services.AddHostedService<StockValidatedEventConsumer>();
        services.Configure<RabbitMQConfiguration>(configuration.GetSection("RabbitMQ"));
        services.AddSingleton<IMessageBus, RabbitMessageBus>();
        services.AddScoped<OrderPlacedEventPublisher>();
        services.AddScoped<CheckStockCommandPublisher>();
        services.AddMediatR(Assembly.Load("eshop.Order.Core"));
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<CreateOrderCommandValidator>();

        services.AddScoped<IMongoClient>(c =>
        {
            return new MongoClient(configuration.GetConnectionString("MongoDb"));
        });
    })
    .Build();

await host.RunAsync();

using eShop.EventBus.Configuration;
using eShop.EventBus.Implementation;
using eShop.OrderingApi.Application.CreateOrder;
using eShop.OrderingApi.DIContainer;
using eShop.OrderingApi.Events.Consumers;
using eShop.OrderingApi.Events.EventConsumers;
using eShop.OrderingApi.Events.Publishers;
using eShop.OrderingApi.Repository;
using MediatR;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<CreateOrderCommandValidator>();
builder.Services.RegisterAuthentication(builder.Configuration);

builder.Services.AddScoped<IMongoClient>(c =>
{
    return new MongoClient(builder.Configuration.GetConnectionString("MongoDb"));
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton<IMessageBus, RabbitMessageBus>();
builder.Services.AddHostedService<StartOrderCommandConsumer>();
builder.Services.AddHostedService<StockValidatedEventConsumer>();
builder.Services.AddScoped<CheckStockCommandPublisher>();
builder.Services.AddScoped<OrderPlacedEventPublisher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

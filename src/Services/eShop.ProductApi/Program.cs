using Azure.Storage.Blobs;
using eShop.EventBus.Configuration;
using eShop.EventBus.Implementation;
using eShop.OrderingApi.Events.Consumers;
using eShop.ProductApi.Configurations;
using eShop.ProductApi.DataAccess;
using eShop.ProductApi.DIContainer;
using eShop.ProductApi.Events.Publishers;
using eShop.ProductApi.Services.Blob;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;


// Dependency injection container
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
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
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.RegisterAuthentication(builder.Configuration);
builder.Services.RegisterAuthorization();

builder.Services.AddDbContext<ProductDbContext>(options => options
    .UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 31))));

builder.Services.AddSingleton(new BlobServiceClient(builder.Configuration.GetConnectionString("AzureStorageConnectionString")));

builder.Services.AddScoped<IBlobService, BlobService>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "eShop_Product";
});

builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton<IMessageBus, RabbitMessageBus>();
builder.Services.AddHostedService<CheckStockCommandConsumer>();
builder.Services.AddScoped<StockValidatedEventPublisher>();
builder.Services.AddHostedService<OrderPlacedEventConsumer>();

var app = builder.Build();

app.CreateBlobContainers();

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

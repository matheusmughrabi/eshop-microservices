using eShop.OrderingApi.Application.PlaceOrder;
using eShop.OrderingApi.DIContainer;
using eShop.OrderingApi.Repository;
using MediatR;
using MongoDB.Driver;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<PlaceOrderCommandValidator>();
builder.Services.RegisterAuthentication(builder.Configuration);

builder.Services.AddScoped<IMongoClient>(c =>
{
    return new MongoClient(builder.Configuration.GetConnectionString("MongoDb"));
});

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

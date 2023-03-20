using Azure.Storage.Blobs;
using eShop.ProductApi.Configurations;
using eShop.ProductApi.DataAccess;
using eShop.ProductApi.DIContainer;
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
}); builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.RegisterAuthentication(builder.Configuration);
builder.Services.RegisterAuthorization();

builder.Services.AddDbContext<ProductDbContext>(options => options
    .UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 31))));

builder.Services.AddSingleton(new BlobServiceClient(builder.Configuration.GetConnectionString("AzureStorageConnectionString")));

builder.Services.AddScoped<IBlobService, BlobService>();

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

﻿using eShop.WebUI.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace eShop.WebUI.DIContainer;

public static class AuthenticationExtensions
{
    public static void RegisterAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenConfiguration = new TokenConfiguration();
        configuration.Bind(nameof(TokenConfiguration), tokenConfiguration);
        services.AddSingleton(tokenConfiguration);

        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidIssuer = tokenConfiguration.Issuer,

            ValidateAudience = false,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration.Secret)),

            RequireExpirationTime = true,
            ValidateLifetime = true,

            ClockSkew = TimeSpan.Zero
        };

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = tokenValidationParameters;

            options.Events = new JwtBearerEvents()
            {
                OnMessageReceived = context =>
                {
                    if (context.Request.Cookies.ContainsKey("X-Access-Token"))
                    {
                        context.Token = context.Request.Cookies["X-Access-Token"];
                    }

                    return Task.CompletedTask;
                }
            };
        });
    }
}

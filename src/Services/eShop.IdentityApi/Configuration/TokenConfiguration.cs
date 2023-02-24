﻿namespace eShop.IdentityApi.Configuration;

public class TokenConfiguration
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
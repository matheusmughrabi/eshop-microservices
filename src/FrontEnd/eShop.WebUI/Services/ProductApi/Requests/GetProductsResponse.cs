﻿namespace eShop.WebUI.Services.ProductApi.Requests;

public class GetProductsResponse
{
    public List<Product> Products { get; set; }

    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public Guid? CategoryId { get; set; }
    }
}

﻿using eShop.EventBus.Base;

namespace eShop.EventBus.Messages;

public class OrderPlacedEventMessage : IEventMessage
{
    public string OrderId { get; set; }
    public List<Product> Products { get; set; }

    public class Product
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
    }
}

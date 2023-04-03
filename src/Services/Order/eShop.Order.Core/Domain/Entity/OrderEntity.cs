using eShop.Order.Core.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eShop.Order.Core.Domain.Entity;

public class OrderEntity
{
    public OrderEntity(string userId)
    {
        UserId = userId;
        DateOfPurchase = DateTime.UtcNow;
        Status = OrderStatusEnum.Processing;
        Products = new List<Product>();
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; }
    public string UserId { get; private set; }
    public DateTime DateOfPurchase { get; private set; }
    public OrderStatusEnum Status { get; set; }
    public List<Product> Products { get; private set; }
    public List<Notification> Notifications { get; private set; }

    public void AddProduct(Product product)
    {
        if (product is null)
            throw new ArgumentNullException(nameof(product));

        Products.Add(product);
    }

    public void AddNotification(Notification notification)
    {
        if (notification is null)
            throw new ArgumentNullException(nameof(notification));

        if (string.IsNullOrEmpty(notification.Description))
            throw new Exception("Description cannot be null or empty");

        if (Notifications is null)
            Notifications = new List<Notification>();

        Notifications.Add(notification);
    }

    public class Product
    {
        public Product(string id, string name, decimal priceAtPurchase, int quantity, string? imagePath = null)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("id cannot be null or empty");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name cannot be null or empty");

            if (priceAtPurchase <= 0)
                throw new ArgumentException("PriceAtPurchase has to be greater than zero");

            if (quantity <= 0)
                throw new ArgumentException("Quantity has to be greater than zero");

            Id = id;
            Name = name;
            PriceAtPurchase = priceAtPurchase;
            Quantity = quantity;
            ImagePath = imagePath;
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal PriceAtPurchase { get; private set; }
        public int Quantity { get; private set; }
        public string? ImagePath { get; private set; }
    }

    public class Notification
    {
        public string Description { get; set; }
    }
}

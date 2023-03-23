using eShop.OrderingApi.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eShop.OrderingApi.Entity;

public class OrderEntity
{
    public OrderEntity(string userId)
    {
        UserId = userId;
        DateOfPurchase = DateTime.UtcNow;
        Status = OrderStatusEnum.Placed;
        Products = new List<Product>();
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; }
    public string UserId { get; private set; }
    public DateTime DateOfPurchase { get; private set; }
    public OrderStatusEnum Status { get; private set; }
    public List<Product> Products { get; private set; }

    public void AddProduct(Product product)
    {
        if (product is null)
            throw new ArgumentNullException(nameof(product));

        Products.Add(product);
    }

    public class Product
    {
        public Product(string id, string name, decimal priceAtPurchase, int quantity)
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
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal PriceAtPurchase { get; private set; }
        public int Quantity { get; private set; }
    }
}

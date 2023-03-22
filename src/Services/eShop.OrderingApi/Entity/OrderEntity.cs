using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eShop.OrderingApi.Entity;

public class OrderEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string UserId { get; set; }
    public DateTime DateOfPurchase { get; set; }
    public List<Product> Products { get; set; }

    public class Product
    {
        public string Id { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal PriceAtTimeOfOrder { get; set; }
        public int Quantity { get; set; }
    }
}

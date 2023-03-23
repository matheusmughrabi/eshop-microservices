using eShop.OrderingApi.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eShop.OrderingApi.Repository.Models;

public class GetOrdersByUserIdResponse
{
    public List<Order> Orders { get; set; }

    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }
        public DateTime DateOfPurchase { get; private set; }
        public OrderStatusEnum Status { get; private set; }
    }
}

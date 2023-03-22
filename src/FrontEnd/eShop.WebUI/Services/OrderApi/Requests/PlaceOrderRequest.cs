namespace eShop.WebUI.Services.OrderApi.Requests;

public class PlaceOrderRequest
{
    public List<Product> Products { get; set; }

    public class Product
    {
        public string Id { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public int Quantity { get; set; }
    }
}

namespace eShop.WebUI.Services.BasketApi.Requests;

public class GetBasketResponse
{
    public List<Item> Items { get; set; }

    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
        public int Quantity { get; set; }
    }
}

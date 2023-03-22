namespace eShop.WebUI.Services.BasketApi.Requests;

public class AddToBasketRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string? ImagePath { get; set; }
    public int Quantity { get; set; }
}

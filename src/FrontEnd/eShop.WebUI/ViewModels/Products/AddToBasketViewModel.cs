namespace eShop.WebUI.ViewModels.Products;

public class AddToBasketViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string? ImagePath { get; set; }
}

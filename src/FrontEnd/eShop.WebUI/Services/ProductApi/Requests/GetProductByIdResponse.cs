namespace eShop.WebUI.Services.ProductApi.Requests;

public class GetProductByIdResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int QuantityOnHand { get; set; }
    public string ImagePath { get; set; }
    public Guid CategoryId { get; set; }
}

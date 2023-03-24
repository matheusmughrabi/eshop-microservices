namespace eShop.WebUI.Services.ProductApi.Requests;

public class GetCategoriesResponse
{
    public int TotalItems { get; set; }
    public List<Category> Categories { get; set; }

    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int TotalProducts { get; set; }
    }
}

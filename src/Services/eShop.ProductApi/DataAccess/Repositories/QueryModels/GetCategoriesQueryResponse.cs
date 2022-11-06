namespace eShop.ProductApi.DataAccess.Repositories.QueryModels
{
    public class GetCategoriesQueryResponse
    {
        public List<Category> Categories { get; set; }

        public class Category
        {
            public string Name { get; set; }
            public string? Description { get; set; }
            public int TotalProducts { get; set; }
        }
    }
}

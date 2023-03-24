namespace eShop.WebUI.Services.ProductApi.Requests;

public class GetAllCategoryGroupsResponse
{
    public List<CategoryGroup> CategoryGroups { get; set; }

    public class CategoryGroup
    {
        public string Name { get; set; }
        public List<Category> Categories { get; set; }

        public class Category
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}

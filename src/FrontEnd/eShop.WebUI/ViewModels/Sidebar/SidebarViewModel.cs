namespace eShop.WebUI.ViewModels.Sidebar;

public class SidebarViewModel
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

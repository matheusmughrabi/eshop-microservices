namespace eShop.AdminUI.DIContainer
{
    public static class RoutingExtensions
    {
        public static void AddCustomRouting(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/Catalog", "Catalog_Admin");
            }).AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/Catalog/Category/Index", "");
            });
        }
    }
}

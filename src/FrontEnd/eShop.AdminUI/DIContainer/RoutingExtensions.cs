namespace eShop.AdminUI.Container
{
    public static class RoutingExtensions
    {
        public static void AddCustomRouting(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddRazorPages().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/Catalog/Category/Index", "");
            });
        }
    }
}

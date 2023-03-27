using eShop.WebUI.Services.ProductApi;
using eShop.WebUI.ViewModels.Sidebar;
using Microsoft.AspNetCore.Mvc;

namespace eShop.WebUI.ViewComponents;

public class SidebarViewComponent : ViewComponent
{
    private readonly IProductApiClient _productApiClient;

    public SidebarViewComponent(IProductApiClient productApiClient)
    {
        _productApiClient = productApiClient;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var response = await _productApiClient.GetAllCategoryGroups();

        var viewModel = new SidebarViewModel()
        {
            CategoryGroups = response.CategoryGroups.Select(categoryGroup => new SidebarViewModel.CategoryGroup
            {
                Name = categoryGroup.Name,
                Categories = categoryGroup.Categories.Select(category => new SidebarViewModel.CategoryGroup.Category
                {
                    Id = category.Id,
                    Name = category.Name
                }).ToList()
            }).ToList(),
        };

        return View(viewModel);
    }
}

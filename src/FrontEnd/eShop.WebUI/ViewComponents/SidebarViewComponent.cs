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

    private async Task<SidebarViewModel> GetMenuItemsAsync()
    {
        return new SidebarViewModel()
        {
            CategoryGroups = new List<SidebarViewModel.CategoryGroup>()
            {
                new SidebarViewModel.CategoryGroup()
                {
                    Name = "Drinks",
                    Categories = new List<SidebarViewModel.CategoryGroup.Category>()
                    {
                        new SidebarViewModel.CategoryGroup.Category()
                        {
                            Id = Guid.NewGuid(),
                            Name = "Beer"
                        },
                        new SidebarViewModel.CategoryGroup.Category()
                        {
                            Id = Guid.NewGuid(),
                            Name = "Soda"
                        },
                        new SidebarViewModel.CategoryGroup.Category()
                        {
                            Id = Guid.NewGuid(),
                            Name = "Soft Drinks"
                        },
                    }
                },

                new SidebarViewModel.CategoryGroup()
                {
                    Name = "Eletronics",
                    Categories = new List<SidebarViewModel.CategoryGroup.Category>()
                    {
                        new SidebarViewModel.CategoryGroup.Category()
                        {
                            Id = Guid.NewGuid(),
                            Name = "Notebooks"
                        },
                        new SidebarViewModel.CategoryGroup.Category()
                        {
                            Id = Guid.NewGuid(),
                            Name = "Mouse"
                        },
                        new SidebarViewModel.CategoryGroup.Category()
                        {
                            Id = Guid.NewGuid(),
                            Name = "Screens"
                        },
                    }
                },

                new SidebarViewModel.CategoryGroup()
                {
                    Name = "Toys",
                    Categories = new List<SidebarViewModel.CategoryGroup.Category>()
                    {
                        new SidebarViewModel.CategoryGroup.Category()
                        {
                            Id = Guid.NewGuid(),
                            Name = "Toys"
                        },
                        new SidebarViewModel.CategoryGroup.Category()
                        {
                            Id = Guid.NewGuid(),
                            Name = "Dog Toys"
                        }
                    }
                },
            }

        };
    }
}

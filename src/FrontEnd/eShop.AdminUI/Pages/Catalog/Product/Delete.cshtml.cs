using eShop.AdminUI.Services.ProductApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShop.AdminUI.Pages.Catalog.Product
{
    public class DeleteModel : PageModel
    {
        private readonly IProductApiClient _productApiClient;

        public DeleteModel(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        public async Task<IActionResult> OnPostDeleteProduct([FromBody] ProductApiClient.DeleteProductRequest data)
        {
            var response = await _productApiClient.DeleteProduct(data);
            return new JsonResult(response);
        }
    }
}

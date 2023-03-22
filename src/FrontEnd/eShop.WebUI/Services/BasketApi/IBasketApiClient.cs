using eShop.WebUI.Services.BasketApi.Requests;

namespace eShop.WebUI.Services.BasketApi;

public interface IBasketApiClient
{
    Task<GetBasketResponse> GetBasket();
    Task<bool> AddToBasket(AddToBasketRequest request);
    Task<bool> RemoveItem(RemoveItemFromBasketRequest request);
}

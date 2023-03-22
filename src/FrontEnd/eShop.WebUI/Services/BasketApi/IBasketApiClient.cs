using eShop.WebUI.Services.BasketApi.Requests;

namespace eShop.WebUI.Services.BasketApi;

public interface IBasketApiClient
{
    Task<bool> AddToBasket(AddToBasketRequest request);
}

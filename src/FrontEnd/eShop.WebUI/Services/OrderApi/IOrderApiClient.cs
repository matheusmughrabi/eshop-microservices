using eShop.WebUI.Services.OrderApi.Requests;

namespace eShop.WebUI.Services.OrderApi;

public interface IOrderApiClient
{
    Task<bool> PlaceOrder(PlaceOrderRequest request);
}

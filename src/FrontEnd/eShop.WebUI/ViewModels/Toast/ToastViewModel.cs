namespace eShop.WebUI.ViewModels.Toast;

public class ToastViewModel
{
    public ToastViewModel(string message, ToastTypeEnum toastType)
    {
        Message = message;
        ToastType = toastType;
    }

    public string Message { get; set; }
    public ToastTypeEnum ToastType { get; set; }

    public string GetToastBackgroud()
    {
        switch (ToastType)
        {
            case ToastTypeEnum.Success: return "bg-success";
            case ToastTypeEnum.Error: return "bg-danger";
            default: return "";
        }
    }
}

public enum ToastTypeEnum
{
    Success = 1,
    Error = 2,
}

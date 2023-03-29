using eShop.WebUI.Enums.Order;

namespace eShop.WebUI.ViewModels.Order;

public class OrderViewModel
{
    public List<Order> Orders { get; set; }

    public class Order
    {
        public string Id { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public OrderStatusEnum Status { get; set; }
        public string StatusDescription { get; set; }
        public List<Product> Products { get; set; }

        public decimal CalculateTotal()
        {
            return Products.Sum(c => c.PriceAtPurchase * c.Quantity);
        }
    }

    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public int Quantity { get; set; }
        public string ImagePath { get; set; }
    }
}

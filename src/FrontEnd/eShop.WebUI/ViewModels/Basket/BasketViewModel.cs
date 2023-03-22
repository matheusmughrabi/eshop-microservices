namespace eShop.WebUI.ViewModels.Basket;

public class BasketViewModel
{
    public List<Item> Items { get; set; }

    public decimal CalculateTotalCost()
    {
        return Items.Sum(x => x.TotalCost);
    }

    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
        public int Quantity { get; set; }
        public decimal TotalCost => Price * Quantity;
    }
}

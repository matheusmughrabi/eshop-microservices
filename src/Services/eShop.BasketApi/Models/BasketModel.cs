namespace eShop.BasketApi.Models;

public class BasketModel
{
    public List<Item> Items { get; set; }

    public void AddItem(Item item)
    {
        if (item is null)
            throw new ArgumentException("item cannot be null");

        if (item.Quantity <= 0)
            throw new ArgumentException("quantity of the item must be greater than zero");

        if (Items is null)
            Items = new List<Item>();

        var itemFromBasket = Items.FirstOrDefault(c => c.Id == item.Id);
        if (itemFromBasket is null)
        {
            Items.Add(item);
            return;
        }

        itemFromBasket.Quantity += item.Quantity;
    }

    public void RemoveItem(Guid itemId)
    {
        var itemToBeRemoved = Items.FirstOrDefault(c => c.Id == itemId);
        if (itemToBeRemoved is null)
            throw new ArgumentException("Item not found");

        Items.Remove(itemToBeRemoved);
    }


    public void SubtractItemQuantity(Guid itemId)
    {
        var itemFromBasket = Items.FirstOrDefault(c => c.Id == itemId);
        if (itemFromBasket is null)
            throw new ArgumentException("item is not in the basket");

        if (itemFromBasket.Quantity == 0)
            throw new ArgumentException("item quantity is already zero");

        itemFromBasket.Quantity -= 1;

        // If after updating itemFromBasket.Quantity there is nothing left, then I'll just remove the object from the list
        if (itemFromBasket.Quantity == 0)
            Items.Remove(itemFromBasket);
    }

    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
        public int Quantity { get; set; }
    }
}

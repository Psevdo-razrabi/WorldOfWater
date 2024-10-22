using Inventory;

public class Factory
{
    public static Item CreateItem(ItemDescription description, int quantity) => new Item(description, quantity);
}
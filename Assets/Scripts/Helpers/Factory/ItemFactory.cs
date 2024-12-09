using Inventory;

namespace Factory
{
    public class ItemFactory
    {
        public static Item CreateItem(ItemDescription description, int quantity) => new Item(description, quantity);
    }
}
using Factory;

namespace Inventory
{
    public class ItemCreator
    {
        public Item CreateItem(ItemDescription itemDescription, int quantity) =>
            ItemFactory.CreateItem(itemDescription, quantity);
    }
}
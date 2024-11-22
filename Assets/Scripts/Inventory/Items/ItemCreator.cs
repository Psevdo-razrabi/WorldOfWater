namespace Inventory
{
    public class ItemCreator
    {
        public Item CreateItem(ItemDescription itemDescription, int quantity) =>
            Factory.CreateItem(itemDescription, quantity);
    }
}
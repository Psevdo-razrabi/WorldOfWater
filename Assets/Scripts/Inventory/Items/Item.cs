using Helpers;

namespace Inventory
{
    public class Item
    {
        public GuidItem Id { get; private set; }
        public ItemDescription ItemDescription { get; private set; }
        public int Quantity { get; private set; }

        public void SetQuantity(int quantity)
        {
            Preconditions.CheckValidateData(quantity);
            Quantity = quantity;
        }

        public Item(ItemDescription itemDescription, int quantity = 1)
        {
            Id = GuidItem.NewGuid();
            ItemDescription = itemDescription;
            Quantity = quantity;
        }
    }
}
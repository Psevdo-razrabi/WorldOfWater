using System;

namespace Inventory
{
    public class ItemOperationMediator
    {
        private ItemTypeResolver _itemTypeResolver;
        private ItemCreator _itemCreator;
        
        public event Action<Item> OnItemTake;

        public ItemOperationMediator(ItemTypeResolver itemTypeResolver, ItemCreator itemCreator)
        {
            _itemTypeResolver = itemTypeResolver;
            _itemCreator = itemCreator;
        }

        public void TakeItem(EItemType type)
        {
            var description = _itemTypeResolver.FindItemAfterType(type);
            var item = _itemCreator.CreateItem(description, 1);
            OnItemTake?.Invoke(item);
        }

        public void DropItem()
        {
            
        }
    }
}
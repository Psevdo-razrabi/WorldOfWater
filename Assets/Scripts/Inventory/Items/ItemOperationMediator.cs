using System;
using Helpers.PoolObject;
using UnityEngine;

namespace Inventory
{
    public class ItemOperationMediator
    {
        private ItemTypeResolver _itemTypeResolver;
        private ItemCreator _itemCreator;
        private ObjectPoolInventory _objectPoolInventory;
        private readonly Transform _playerTransform;
        public event Action<Item> OnItemTake;

        public ItemOperationMediator(ItemTypeResolver itemTypeResolver, ItemCreator itemCreator, ObjectPoolInventory objectPoolInventory, Player playerTransform)
        {
            _itemTypeResolver = itemTypeResolver;
            _itemCreator = itemCreator;
            _objectPoolInventory = objectPoolInventory;
            _playerTransform = playerTransform.transform;
        }

        public void TakeItem(ItemType itemType)
        {
            var description = _itemTypeResolver.FindItemAfterType(itemType.Type);
            var item = _itemCreator.CreateItem(description, 1);
            OnItemTake?.Invoke(item);
            itemType.gameObject.SetActive(false);
        }

        public void DropItem(Item item)
        {
            (GameObject, ItemType) initItem = default;
            switch (item.ItemDescription.ItemType)
            {
                case EItemType.Cloth : initItem = _objectPoolInventory.GetObject(PoolKeyName.ClothPrefab);
                    break;
                case EItemType.Metal : initItem = _objectPoolInventory.GetObject(PoolKeyName.MetalPrefab);
                    break;
                case EItemType.Plastic : initItem = _objectPoolInventory.GetObject(PoolKeyName.PlasticPrefab);
                    break;
                case EItemType.Wood : initItem = _objectPoolInventory.GetObject(PoolKeyName.WoodPrefab);
                    break;
            }
            CreateImpulse(initItem);
        }

        private void CreateImpulse((GameObject, ItemType) initItem)
        {
            initItem.Item1.transform.position = _playerTransform.position + _playerTransform.forward;
            var rb = initItem.Item2.GetComponent<Rigidbody>();
            
            rb.AddForce(new Vector3(0, 2f, 2f), ForceMode.Impulse);
        }
    }
}
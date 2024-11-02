using System.Collections.Generic;
using Helpers;
using UnityEngine;

namespace Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private int capacity;
        [SerializeField] private List<ItemDescription> startingItem;
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private InventoryDescription _inventoryDescription;
        [SerializeField] private InventoryItemAnimator _inventoryItemAnimator;
        
        private BuildInventory _build;
        
        public InventoryStorage InventoryStorage { get; private set; }

        private void Awake()
        {
            _build = new BuildInventory(_inventoryView, _inventoryDescription, _inventoryItemAnimator);
            InventoryStorage = _build
                .WithStartingItem(startingItem)
                .WithAnimation(new UiAnimation())
                .WithCapacity(capacity)
                .Build();
            
            InventoryStorage.InventoryPresenter.Initialize();
            InventoryStorage.InventoryModel.Initialize();
        }

        private void OnDestroy()
        {
            InventoryStorage.InventoryPresenter.Dispose();
        }
    }
}
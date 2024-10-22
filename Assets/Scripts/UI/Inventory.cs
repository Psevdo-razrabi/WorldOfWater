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
        private BuildInventory _build;
        
        public InventoryStorage InventoryStorage { get; private set; }

        private void Awake()
        {
            _build = new BuildInventory(_inventoryView);
            InventoryStorage = _build
                .WithStartingItem(startingItem)
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
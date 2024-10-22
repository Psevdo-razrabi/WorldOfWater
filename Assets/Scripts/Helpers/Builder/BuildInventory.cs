using System;
using System.Collections.Generic;
using Inventory;

namespace Helpers
{
    public class BuildInventory
    {
        private InventoryView _inventoryView;
        private IEnumerable<ItemDescription> _items;
        private int _capacity;

        public BuildInventory(InventoryView inventoryView)
        {
            Preconditions.CheckNotNull(inventoryView, "View is null");
            _inventoryView = inventoryView;
        }
        
        public BuildInventory WithStartingItem(IEnumerable<ItemDescription> items)
        {
            _items = items;
            return this;
        }

        public BuildInventory WithCapacity(int capacity)
        {
            _capacity = capacity;
            return this;
        }

        public InventoryStorage Build()
        {
            InventoryModel model = _items != null
                ? new InventoryModel(_items, _capacity)
                : new InventoryModel(Array.Empty<ItemDescription>(), _capacity);
            InventoryPresenter inventoryPresenter = new InventoryPresenter(_inventoryView, model, _capacity);
            return new InventoryStorage(_inventoryView, model, inventoryPresenter);
        }
    }

    public class InventoryStorage
    {
        public readonly InventoryView InventoryView;
        public readonly InventoryModel InventoryModel;
        public readonly InventoryPresenter InventoryPresenter;

        public InventoryStorage(InventoryView inventoryView, InventoryModel inventoryModel, InventoryPresenter inventoryPresenter)
        {
            InventoryView = inventoryView;
            InventoryModel = inventoryModel;
            InventoryPresenter = inventoryPresenter;
        }
    }
}
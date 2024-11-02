﻿using System;
using System.Collections.Generic;
using Inventory;

namespace Helpers
{
    public class BuildInventory
    {
        private readonly InventoryView _inventoryView;
        private IEnumerable<ItemDescription> _items;
        private int _capacity;
        private UiAnimation _uiAnimation;
        private readonly InventoryDescription _inventoryDescription;
        private readonly InventoryItemAnimator _inventoryItemAnimator;

        public BuildInventory(InventoryView inventoryView, InventoryDescription description, InventoryItemAnimator inventoryItemAnimator)
        {
            Preconditions.CheckNotNull(inventoryView, "View is null");
            _inventoryView = inventoryView;
            _inventoryDescription = description;
            _inventoryItemAnimator = inventoryItemAnimator;
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

        public BuildInventory WithAnimation(UiAnimation animation)
        {
            _uiAnimation = animation;
            return this;
        }

        public InventoryStorage Build()
        {
            InventoryModel model = _items != null
                ? new InventoryModel(_items, _capacity, _uiAnimation, _inventoryItemAnimator)
                : new InventoryModel(Array.Empty<ItemDescription>(), _capacity, _uiAnimation, _inventoryItemAnimator);
            InventoryPresenter inventoryPresenter = new InventoryPresenter(_inventoryView, model, _inventoryDescription, _capacity);
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
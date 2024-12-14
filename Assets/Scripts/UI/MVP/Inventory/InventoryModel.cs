using System;
using System.Collections.Generic;
using System.Linq;
using Factory;
using Helpers;
using Helpers.Shader;
using NewInput;
using R3;
using UnityEngine;

namespace Inventory
{
    public class InventoryModel : StorageModel, IDisposable
    {
        private readonly IEnumerable<ItemDescription> _items;
        private readonly ItemOperationMediator _mediator;
        private readonly InventoryItemAnimator _inventoryItemAnimator;
        public readonly UiInput UIInput;
        private int _capasity;
        public Action<Material, Mesh> SetMeshes { get; private set; }
        public Action<string, string> SetTexts { get; private set; }
        public Subject<bool> OnOpenInventory { get; private set; } = new();
        public DataViewInventory DataViewInventory;
        
        public InventoryModel(IEnumerable<ItemDescription> items, int capacity, UiAnimation animation, 
            InventoryItemAnimator inventoryItemAnimator, ItemOperationMediator itemOperationMediator, UiInput input)
        {
            Preconditions.CheckValidateData(capacity);
            ItemsArray = new ObservableArray<Item>(capacity);
            _onModelChange = new Subject<Item[]>();
            _items = items;
            _capasity = capacity;
            _mediator = itemOperationMediator;
            _inventoryItemAnimator = inventoryItemAnimator;
            UIInput = input;

            DataViewInventory = new DataViewInventory(new R3.ReactiveProperty<int>(), _capasity, ItemsArray, animation);
        }

        public void Initialize()
        {
            Subscribe();
            SetMeshes += _inventoryItemAnimator.SetProperties;
            SetTexts += _inventoryItemAnimator.SetTexts;
            _mediator.OnItemTake += TryAddItem;

            foreach (var item in _items)
            {
                ItemsArray.TryAdd(ItemFactory.CreateItem(item, 1));
            }
        }

        public void OpenInventory(bool isActive)
        {
            ShaderHelper.SetGlobalFloat(ShaderParameterId.BlurXId, isActive ? 0.07f : 0f);
            ShaderHelper.SetGlobalFloat(ShaderParameterId.BlurYId, isActive ? 0.07f : 0f);
        }

        public void DropItem(int indexItem)
        {
            _mediator.DropItem(ItemsArray[indexItem]);
            Remove(ItemsArray[indexItem]);
        }

        public void SetQuantity(int quantity, Item item)
        {
            var totalQuantity = item.Quantity + quantity;
            item.SetQuantity(totalQuantity);
        }

        public void TryAddItem(Item item)
        {
            HandleExistingItem(item);
            Update();
        }

        private Item FindExistingItem(Item[] arrayItems, Item item)
        {
            return arrayItems.Where(t => t != null)
                .FirstOrDefault(t => t.ItemDescription.ItemType == item.ItemDescription.ItemType && t.Quantity < t.ItemDescription.MaxStack);
        }

        private void HandleExistingItem(Item newItem)
        {
            var arrayItems = GetArray();
            var itemTarget = FindExistingItem(arrayItems, newItem);

            if (itemTarget == null)
            {
                Add(newItem);
            }
            else
            {
                SetQuantity(1, itemTarget);
            }
        }
        
        private void Subscribe()
        {
            ItemsArray.ValueChangeInArray
                .Subscribe(items => _onModelChange.OnNext(items));
        }

        public void Dispose()
        {
            _onModelChange?.Dispose();
            ItemsArray?.Dispose();
            SetMeshes -= _inventoryItemAnimator.SetProperties;
            SetTexts -= _inventoryItemAnimator.SetTexts;
            _mediator.OnItemTake -= TryAddItem;
        }
    }
}
using System;
using System.Collections.Generic;
using Helpers;
using Helpers.Shader;
using R3;
using UnityEngine;
using Zenject;

namespace Inventory
{
    public class InventoryModel : IDisposable
    {
        private readonly IEnumerable<ItemDescription> _items;
        private readonly ItemOperationMediator _mediator;
        private int _capasity;
        private readonly InventoryItemAnimator _inventoryItemAnimator;
        private readonly Subject<Item[]> _onModelChange = new();
        public Action<Material, Mesh> SetMeshes { get; private set; }
        public Action<string, string> SetTexts { get; private set; }
        public Subject<bool> OnOpenInventory { get; private set; } = new();
        public Observable<Item[]> OnModelChange => _onModelChange;
        public readonly ObservableArray<Item> Items;
        public DataView DataView;
        
        public InventoryModel(IEnumerable<ItemDescription> items, int capacity, UiAnimation animation, 
            InventoryItemAnimator inventoryItemAnimator, ItemOperationMediator itemOperationMediator)
        {
            Preconditions.CheckValidateData(capacity);
            Items = new ObservableArray<Item>(capacity);
            _items = items;
            _capasity = capacity;
            _mediator = itemOperationMediator;
            _inventoryItemAnimator = inventoryItemAnimator;

            DataView = new DataView(new R3.ReactiveProperty<int>(), _capasity, Items, animation);
        }

        public void Initialize()
        {
            Subscribe();
            SetMeshes += _inventoryItemAnimator.SetProperties;
            SetTexts += _inventoryItemAnimator.SetTexts;
            _mediator.OnItemTake += Add;

            foreach (var item in _items)
            {
                Items.TryAdd(Factory.CreateItem(item, 1));
            }
        }

        public void OpenInventory(bool isActive)
        {
            ShaderHelper.SetGlobalFloat(ShaderParameterId.BlurXId, isActive ? 0.07f : 0f);
            ShaderHelper.SetGlobalFloat(ShaderParameterId.BlurYId, isActive ? 0.07f : 0f);
        }

        public Item Get(int index) => Items[index];
        public void Add(Item item) => Items.TryAdd(item);
        public bool Remove(Item item) => Items.TryRemove(item);
        public void Clear() => Items.Clear();
        public void Update() => Items.Update();
        public void Swap(int indexOne, int indexTwo) => Items.Swap(indexOne, indexTwo);

        public int CombineItem(int indexOne, int indexTwo)
        {
            var totalQuantity = Items[indexOne].Quantity + Items[indexTwo].Quantity;
            Items[indexTwo].SetQuantity(totalQuantity);
            Remove(Items[indexOne]);
            return totalQuantity;
        }
        
        private void Subscribe()
        {
            Items.ValueChangeInArray
                .Subscribe(items => _onModelChange.OnNext(items));
        }

        public void Dispose()
        {
            _onModelChange?.Dispose();
            Items?.Dispose();
            SetMeshes -= _inventoryItemAnimator.SetProperties;
            SetTexts -= _inventoryItemAnimator.SetTexts;
            _mediator.OnItemTake -= Add;
        }
    }
}
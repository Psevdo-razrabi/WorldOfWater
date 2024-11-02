using System;
using System.Collections.Generic;
using Helpers;
using R3;
using UnityEngine;

namespace Inventory
{
    public class InventoryModel : IDisposable
    {
        private readonly IEnumerable<ItemDescription> _items;
        private int _capasity;
        private readonly InventoryItemAnimator _inventoryItemAnimator;
        private readonly Subject<Item[]> _onModelChange = new();
        public Action<Material, Mesh> SetMeshes { get; private set; }
        public Action<string, string> SetTexts { get; private set; }
        public Observable<Item[]> OnModelChange => _onModelChange;
        public readonly ObservableArray<Item> Items;
        public DataView DataView;
        
        public InventoryModel(IEnumerable<ItemDescription> items, int capacity, UiAnimation animation, InventoryItemAnimator inventoryItemAnimator)
        {
            Preconditions.CheckValidateData(capacity);
            Items = new ObservableArray<Item>(capacity);
            _items = items;
            _capasity = capacity;
            _inventoryItemAnimator = inventoryItemAnimator;

            DataView = new DataView(new R3.ReactiveProperty<int>(), _capasity, Items, animation);
        }

        public void Initialize()
        {
            Subscribe();
            SetMeshes += _inventoryItemAnimator.SetProperties;
            SetTexts += _inventoryItemAnimator.SetTexts;

            foreach (var item in _items)
            {
                Items.TryAdd(Factory.CreateItem(item, 1));
            }
        }

        private void Subscribe()
        {
            Items.ValueChangeInArray
                .Subscribe(items => _onModelChange.OnNext(items));
        }

        public Item Get(int index) => Items[index];
        public bool Add(Item item) => Items.TryAdd(item);
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

        public void Dispose()
        {
            _onModelChange?.Dispose();
            Items?.Dispose();
            SetMeshes -= _inventoryItemAnimator.SetProperties;
            SetTexts -= _inventoryItemAnimator.SetTexts;
        }
    }
}
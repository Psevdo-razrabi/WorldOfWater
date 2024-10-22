using System;
using System.Collections.Generic;
using Helpers;
using R3;

namespace Inventory
{
    public class InventoryModel : IDisposable
    {
        private readonly IEnumerable<ItemDescription> _items;
        private int _capasity;
        private readonly Subject<Item[]> _onModelChange = new();
        public Observable<Item[]> OnModelChange => _onModelChange;
        public readonly ObservableArray<Item> Items;
        public DataView DataView;
        
        public InventoryModel(IEnumerable<ItemDescription> items, int capacity)
        {
            Preconditions.CheckValidateData(capacity);
            Items = new ObservableArray<Item>(capacity);
            _items = items;
            _capasity = capacity;
            
            DataView = new DataView(new ReactiveProperty<int>(), _capasity, Items);
        }

        public void Initialize()
        {
            Subscribe();

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
        }
    }
}
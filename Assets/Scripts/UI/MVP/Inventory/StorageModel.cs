using Helpers;
using R3;

namespace Inventory
{
    public abstract class StorageModel
    {
        protected Subject<Item[]> _onModelChange;
        protected ObservableArray<Item> ItemsArray; 
        public Observable<Item[]> OnModelChange => _onModelChange;
        
        public virtual bool TryGet(int index, out Item item) => ItemsArray.TryGet(index, out item);
        public virtual Item Get(int index) => ItemsArray[index];
        public virtual void Add(Item item) => ItemsArray.TryAdd(item);
        public virtual bool Remove(Item item) => ItemsArray.TryRemove(item);
        public virtual Item[] GetArray() => ItemsArray.GetArray();
        public virtual void Clear() => ItemsArray.Clear();
        public virtual void Update() => ItemsArray.Update();
        public virtual void Swap(int indexOne, int indexTwo) => ItemsArray.Swap(indexOne, indexTwo);
        
        public int CombineItem(int indexOne, int indexTwo)
        {
            var totalQuantity = ItemsArray[indexOne].Quantity + ItemsArray[indexTwo].Quantity;
            ItemsArray[indexTwo].SetQuantity(totalQuantity);
            Remove(ItemsArray[indexOne]);
            return totalQuantity;
        }
    }
}
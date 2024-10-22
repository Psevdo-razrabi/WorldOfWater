using Helpers;
using R3;

namespace Inventory
{
    public class DataView
    {
        public readonly int Capacity;
        public readonly ReactiveProperty<int> Coins;
        public readonly ObservableArray<Item> Items;

        public DataView(ReactiveProperty<int> coins, int capacity, ObservableArray<Item> items)
        {
            Coins = coins;
            Capacity = capacity;
            Items = items;
        }
    }
}
using Helpers;
using R3;

namespace Inventory
{
    public class DataViewInventory
    {
        public readonly int Capacity;
        public readonly ReactiveProperty<int> Coins;
        public readonly UiAnimation Animation;
        public readonly ObservableArray<Item> Items;

        public DataViewInventory(ReactiveProperty<int> coins, int capacity, ObservableArray<Item> items, UiAnimation animation)
        {
            Coins = coins;
            Capacity = capacity;
            Items = items;
            Animation = animation;
        }
    }
}
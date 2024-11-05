using System.Collections.Generic;
using UnityEngine;

namespace Sources.Scripts.Items
{
    public class TakedItemsData
    {
        private Dictionary<ItemType, int> _itemCounts = new();
        private List<Item> _capturedItems = new();

        public void AddCaptureItem(Item item)
        {
            _capturedItems.Add(item);
        }

        public void AddTakedItems()
        {
            foreach (Item item in _capturedItems)
            {
                ItemType type = item.ItemType; 
                if (_itemCounts.ContainsKey(type))
                    _itemCounts[type]++;
                else
                    _itemCounts[type] = 1;
            }
        }

        public void TakeAddedItems()
        {
            foreach (var VARIABLE in _itemCounts)
            {
                Debug.Log($"{VARIABLE.Key} {VARIABLE.Value}");
            }
            // берешь карчое словарь как нить и чистишь хуйню эту
            ClearData();
        }
        private void ClearData()
        {
            _itemCounts.Clear();
            _capturedItems.Clear();
        }
    }
}
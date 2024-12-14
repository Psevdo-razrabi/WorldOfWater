﻿using UnityEngine;

namespace Inventory
{
    public class Money : MonoBehaviour
    {
        [SerializeField] private Inventory _inventory;

        private void Update()
        {
            _inventory.InventoryStorage.InventoryModel.DataViewInventory.Coins.Value += 1;
        }
    }
}
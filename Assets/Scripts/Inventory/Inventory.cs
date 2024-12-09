using System.Collections.Generic;
using Helpers;
using NewInput;
using UnityEngine;
using Zenject;

namespace Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private int capacity;
        [SerializeField] private List<ItemDescription> startingItem;
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private InventoryDescription _inventoryDescription;
        [SerializeField] private InventoryItemAnimator _inventoryItemAnimator;
        
        private BuildInventory _build;
        private UiInput _uiInput;
        
        public InventoryStorage InventoryStorage { get; private set; }
        public ItemOperationMediator ItemOperationMediator { get; private set; }
        public bool IsInit { get; private set; } = false;

        [Inject]
        private void Construct(ItemOperationMediator itemOperationMediator, UiInput uiInput)
        {
            ItemOperationMediator = itemOperationMediator;
            _uiInput = uiInput;
            InitInventory(itemOperationMediator);
        }
        
        private void InitInventory(ItemOperationMediator itemOperationMediator)
        {
            _build = new BuildInventory(_inventoryView, _inventoryDescription, _inventoryItemAnimator);
            InventoryStorage = _build
                .WithStartingItem(startingItem)
                .WithAnimation(new UiAnimation())
                .WithInput(_uiInput)
                .WithCapacity(capacity)
                .WithItemOperation(itemOperationMediator)
                .Build();
            
            InventoryStorage.InventoryPresenter.Initialize();
            InventoryStorage.InventoryModel.Initialize();

            IsInit = true;
        }

        private void OnDestroy()
        {
            InventoryStorage.InventoryPresenter.Dispose();
        }
    }
}
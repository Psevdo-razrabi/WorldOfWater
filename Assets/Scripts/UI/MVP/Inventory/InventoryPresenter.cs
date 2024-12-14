using System;
using System.Collections.Generic;
using Helpers;
using R3;
using UnityEngine;

namespace Inventory
{
    public class InventoryPresenter : StoragePresenter, IDisposable
    {
        private readonly InventoryView _inventoryView;
        private readonly InventoryModel _inventoryModel;
        private readonly InventoryDescription _inventoryDescription;
        private readonly int _capacity;
        private CompositeDisposable _compositeDisposable = new();
        private List<ViewSlot> _slots = new();
        private readonly StorageView[] _storageViews;

        public InventoryPresenter(InventoryView inventoryView, InventoryModel inventoryModel, InventoryDescription inventoryDescription, int capacity)
        {
            Preconditions.CheckNotNull(inventoryView, "View Is Null");
            Preconditions.CheckNotNull(inventoryModel, "Model Is Null");

            _inventoryView = inventoryView;
            _inventoryModel = inventoryModel;
            _inventoryDescription = inventoryDescription;
            _capacity = capacity;
            _storageViews = new StorageView[] { _inventoryView, _inventoryDescription };
        }

        public async void Initialize()
        {
            _inventoryView.OnDrop += HandleSlot;
            _inventoryView.OnCopy += CopySlot;
            _inventoryView.OnCopyGhostIcon += CopyParametersToGhostIcon;
            _inventoryView.OnOpenInventory += _inventoryView.OpenInventoryAnimation;
            
            _inventoryDescription.OnGetItem += GetItemInventory;
            _inventoryDescription.SetMeshes += SetMeshes;
            _inventoryDescription.SetTexts += SetTexts;

            _inventoryModel.UIInput.DropItem += DropSlot;

            foreach (var view in _storageViews)
            {
                view.OnEventTriggerAdd += AddEventTrigger;
                view.OnGetViewSlots += GetViewSlots;
            }

            _inventoryView.Slots
                .Subscribe(slot =>
                {
                    _slots.Add(slot);
                    _inventoryModel.Update();
                })
                .AddTo(_compositeDisposable);
            
            _inventoryModel.OnModelChange
                .Subscribe(_ => HandleModelChange(_capacity, _inventoryModel, _slots))
                .AddTo(_compositeDisposable);

            _inventoryModel.OnOpenInventory
                .Subscribe((isActive) =>
                {
                    _inventoryModel.OpenInventory(isActive);
                    OpenInventory(isActive);
                })
                .AddTo(_compositeDisposable);

            _inventoryView.IsActiveGrid
                .Subscribe(_inventoryView.OnActiveGrid)
                .AddTo(_compositeDisposable);
            
            await _inventoryView.InitializeViewInventory(_inventoryModel.DataViewInventory);
            await _inventoryDescription.InitializeViewInventory(_inventoryModel.DataViewInventory);
        }
        
        private void HandleSlot(ViewSlot originalSlot, ViewSlot closestSlot, GhostIconView ghostIconView)
        {
            if (originalSlot.Index == closestSlot.Index || closestSlot.GuidItem.Equals(GuidItem.ToEmpty()))
            {
                _inventoryModel.Swap(originalSlot.Index, closestSlot.Index);
                return;
            }

            if (_inventoryModel.TryGet(originalSlot.Index, out var originalItemSlot))
            {
                var closestItemSlot = _inventoryModel.Get(closestSlot.Index);
                
                OperationWithModel(originalItemSlot, closestItemSlot, _inventoryModel, originalSlot.Index, closestSlot.Index);
            }
            
            ghostIconView.Clear();
        }

        private void OpenInventory(bool isActive)
        {
            _inventoryView.InvokeOpenInventory(isActive);
        }

        private void SetMeshes(Material material, Mesh mesh)
        {
            _inventoryModel.SetMeshes.Invoke(material, mesh);
        }

        private void SetTexts(string description, string header)
        {
            _inventoryModel.SetTexts.Invoke(description, header);
        }

        private List<ViewSlot> GetViewSlots() => _slots;
        

        private void DropSlot()
        {
            if (_inventoryDescription.viewSlot.IsEmpty == false)
            {
                _inventoryModel.DropItem(_inventoryDescription.viewSlot.Index);
            }
        }

        public void Dispose()
        {
            _inventoryView.OnDrop -= HandleSlot;
            _inventoryView.OnCopy -= CopySlot;
            _inventoryDescription.OnGetItem -= GetItemInventory;
            _inventoryDescription.SetTexts -= SetTexts;
            _inventoryView.OnOpenInventory -= _inventoryView.OpenInventoryAnimation;
            _inventoryModel.UIInput.DropItem -= DropSlot;
            
            foreach (var view in _storageViews)
            {
                view.OnEventTriggerAdd -= AddEventTrigger;
                view.OnGetViewSlots -= GetViewSlots;
            }
            _compositeDisposable.Clear();
            _compositeDisposable.Dispose();
        }

        private Item GetItemInventory(int index)
        {
            if (_inventoryModel.TryGet(index, out var item))
            {
                return item;
            }

            Debug.Log("slot empty");
            return default;
        }
    }
}
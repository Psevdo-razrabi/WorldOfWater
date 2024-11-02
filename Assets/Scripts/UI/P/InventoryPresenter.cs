using System;
using System.Collections.Generic;
using Helpers;
using R3;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class InventoryPresenter : IDisposable
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
            
            _inventoryDescription.OnGetItem += GetItemInventory;
            _inventoryDescription.SetMeshes += SetMeshes;
            _inventoryDescription.SetTexts += SetTexts;

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
                .Subscribe(_ => HandleModelChange())
                .AddTo(_compositeDisposable);

            _inventoryView.IsActiveGrid
                .Subscribe(_inventoryView.OnActiveGrid)
                .AddTo(_compositeDisposable);
            
            await _inventoryView.InitializeView(_inventoryModel.DataView);
            await _inventoryDescription.InitializeView(_inventoryModel.DataView);
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

        private void HandleModelChange()
        {
            for (int i = 0; i < _capacity; i++)
            {
                var item = _inventoryModel.Get(i);
                if (item == null || item.Id.Equals(GuidItem.IsEmpty()))
                {
                    _slots[i].Clear();
                }
                else
                {
                    _slots[i].SetGuid(item.Id);
                    _slots[i].SetImage(item.ItemDescription.Sprite);
                    _slots[i].SetStackLabel(item.Quantity.ToString());
                }
            }
        }

        private void CopySlot(ViewSlot firstSlot, ViewSlot secondSlot)
        {
            secondSlot.SetImage(firstSlot.Sprite);
            secondSlot.SetGuid(firstSlot.GuidItem);
            secondSlot.SetIndex(firstSlot.Index);
            secondSlot.SetStackLabel(firstSlot._stackLabel.text);
        }
        
        private void AddEventTrigger(EventTriggerType eventTriggerType, Action<BaseEventData> callback, ViewSlot slot)
        {
            var eventTrigger = slot.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = eventTriggerType
            };
            entry.callback.AddListener(callback.Invoke);
            eventTrigger.triggers.Add(entry);
        }

        private void HandleSlot(ViewSlot originalSlot, ViewSlot closestSlot)
        {
            if (originalSlot.Index == closestSlot.Index || closestSlot.GuidItem.Equals(GuidItem.IsEmpty()))
            {
                _inventoryModel.Swap(originalSlot.Index, closestSlot.Index);
                return;
            }
            
            //Any actions with inventory

            var currentItemId = _inventoryModel.Get(originalSlot.Index).ItemDescription.Id;
            var targetItemId = _inventoryModel.Get(closestSlot.Index).ItemDescription.Id;

            if (currentItemId.Equals(targetItemId) &&
                _inventoryModel.Get(closestSlot.Index).ItemDescription.MaxStack > 1)
            {
                _inventoryModel.CombineItem(originalSlot.Index, closestSlot.Index);
            }
            else
            {
                _inventoryModel.Swap(originalSlot.Index, closestSlot.Index);
            }
        }

        public void Dispose()
        {
            _inventoryView.OnDrop -= HandleSlot;
            _inventoryView.OnCopy -= CopySlot;
            _inventoryDescription.OnGetItem -= GetItemInventory;
            _inventoryDescription.SetTexts -= SetTexts;
            
            foreach (var view in _storageViews)
            {
                view.OnEventTriggerAdd -= AddEventTrigger;
                view.OnGetViewSlots -= GetViewSlots;
            }
            _compositeDisposable.Clear();
            _compositeDisposable.Dispose();
        }

        private Item GetItemInventory(int index) => _inventoryModel.Get(index);
    }
}
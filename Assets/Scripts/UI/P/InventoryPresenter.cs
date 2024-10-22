using System;
using Helpers;
using R3;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class InventoryPresenter : IDisposable
    {
        private readonly InventoryView _inventoryView;
        private readonly InventoryModel _inventoryModel;
        private readonly int _capacity;
        private CompositeDisposable _compositeDisposable = new();

        public InventoryPresenter(InventoryView inventoryView, InventoryModel inventoryModel, int capacity)
        {
            Preconditions.CheckNotNull(inventoryView, "View Is Null");
            Preconditions.CheckNotNull(inventoryModel, "Model Is Null");
            
            _inventoryView = inventoryView;
            _inventoryModel = inventoryModel;
            _capacity = capacity;
        }

        public async void Initialize()
        {
            _inventoryView.OnDrop += HandleSlot;
            _inventoryView.OnCopy += CopySlot;
            _inventoryView.OnEventTriggerAdd += AddEventTrigger;
            
            _inventoryModel.OnModelChange
                .Subscribe(_ => HandleModelChange())
                .AddTo(_compositeDisposable);

            _inventoryView.IsActiveGrid
                .Subscribe(_inventoryView.OnActiveGrid)
                .AddTo(_compositeDisposable);
            
            await _inventoryView.InitializeView(_inventoryModel.DataView);
        }

        private void HandleModelChange()
        {
            for (int i = 0; i < _capacity; i++)
            {
                var item = _inventoryModel.Get(i);
                if (item == null || item.Id.Equals(GuidItem.IsEmpty()))
                {
                    _inventoryView.Slots[i].Clear();
                }
                else
                {
                    _inventoryView.Slots[i].SetGuid(item.Id);
                    _inventoryView.Slots[i].SetImage(item.ItemDescription.Sprite);
                    _inventoryView.Slots[i].SetStackLabel(item.Quantity.ToString());
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
            _inventoryView.OnEventTriggerAdd -= AddEventTrigger;
            _compositeDisposable.Clear();
            _compositeDisposable.Dispose();
        }
    }
}
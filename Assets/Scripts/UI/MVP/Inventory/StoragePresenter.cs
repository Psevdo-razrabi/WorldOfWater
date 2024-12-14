using System;
using System.Collections.Generic;
using Helpers;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

namespace Inventory
{
    public abstract class StoragePresenter
    {
        protected void HandleModelChange(int capacity, StorageModel storageModel, List<ViewSlot> slots)
        {
            for (int i = 0; i < capacity; i++)
            {
                var item = storageModel.Get(i);
                if (item == null || item.Id.Equals(GuidItem.ToEmpty()))
                {
                    slots[i].Clear();
                }
                else
                {
                    slots[i].SetGuid(item.Id);
                    slots[i].SetImage(item.ItemDescription.Sprite);
                    slots[i].SetStackLabel(item.Quantity.ToString());
                }
            }
        }
        
        protected void CopySlot(ViewSlot firstSlot, ViewSlot secondSlot)
        {
            secondSlot.SetImage(firstSlot.Sprite);
            secondSlot.SetGuid(firstSlot.GuidItem);
            secondSlot.SetIndex(firstSlot.Index);
            secondSlot.SetStackLabel(firstSlot._stackLabel.text);
        }
        
        protected void AddEventTrigger(EventTriggerType eventTriggerType, Action<BaseEventData> callback, ViewSlot slot)
        {
            var eventTrigger = slot.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = eventTriggerType
            };
            entry.callback.AddListener(callback.Invoke);
            eventTrigger.triggers.Add(entry);
        }
        
        protected void CopyParametersToGhostIcon(ViewSlot currentSlot, GhostIconView ghostIconView)
        {
            ghostIconView.SetImage(currentSlot.Sprite);
        }
        
        protected void OperationWithModel(Item currentItem, Item targetItem, StorageModel storageModel, int originalSlotIndex, int closestSlotIndex)
        {
            var currentItemId = currentItem.ItemDescription.Id;
            var targetItemId = targetItem.ItemDescription.Id;
            
            if (currentItemId.Equals(targetItemId) && targetItem.ItemDescription.MaxStack > 1 && targetItem.Quantity + 1 < targetItem.ItemDescription.MaxStack)
            {
                storageModel.CombineItem(originalSlotIndex, closestSlotIndex);
            }
            
            else
            {
                storageModel.Swap(originalSlotIndex, closestSlotIndex);
            }
        }
    }
}
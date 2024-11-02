using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public abstract class StorageView : MonoBehaviour
    {
        protected readonly Subject<ViewSlot> _isSlots = new();
        protected readonly Subject<bool> _isActiveGrid = new();
        public Observable<bool> IsActiveGrid => _isActiveGrid;
        public Observable<ViewSlot> Slots => _isSlots;
        public event Action<ViewSlot, ViewSlot> OnDrop;
        public event Action<ViewSlot, ViewSlot> OnCopy;
        public event Action<EventTriggerType, Action<BaseEventData>, ViewSlot> OnEventTriggerAdd;
        public event Func<List<ViewSlot>> OnGetViewSlots;
        public event Func<int, Item> OnGetItem;
        
        public abstract UniTask InitializeView(DataView dataView);

        protected void InvokeDrop(ViewSlot originalSlot, ViewSlot closestSlot)
        {
            OnDrop?.Invoke(originalSlot, closestSlot);
        }

        protected void InvokeCopy(ViewSlot firstSlot, ViewSlot secondSlot)
        {
            OnCopy?.Invoke(firstSlot, secondSlot);
        }

        protected void InvokeEventTriggerAdd(EventTriggerType eventTriggerType, Action<BaseEventData> callback, ViewSlot slot)
        {
            OnEventTriggerAdd?.Invoke(eventTriggerType, callback, slot);
        }

        protected List<ViewSlot> InvokeGetViewSlots()
        {
            return OnGetViewSlots?.Invoke();
        }

        protected Item InvokeGetItem(int index)
        {
            return OnGetItem?.Invoke(index);
        }
        
        protected ViewSlot FindClosestSlot(Vector2 position, List<ViewSlot> slots, Canvas canvas)
        {
            foreach (var slot in slots)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(slot.GetComponent<RectTransform>(), position, canvas.worldCamera))
                {
                    return slot;
                }
            }

            return null;
        }
    }
}
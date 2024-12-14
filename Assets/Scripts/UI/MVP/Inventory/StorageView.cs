using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using QuickSlot.Db;
using R3;
using Sync;
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
        public event Action<ViewSlot, ViewSlot, GhostIconView> OnDrop;
        public event Action<ViewSlot, ViewSlot> OnCopy;
        public event Action<EventTriggerType, Action<BaseEventData>, ViewSlot> OnEventTriggerAdd;
        public event Func<List<ViewSlot>> OnGetViewSlots;
        public event Func<int, Item> OnGetItem;
        public event Action<ViewSlot, GhostIconView> OnCopyGhostIcon;
        public event Action<bool> OnOpenInventory;

        public virtual async UniTask InitializeViewInventory(DataViewInventory dataViewInventory)
        {
            await UniTask.Yield();
        }
        
        public virtual async UniTask InitializeViewQuickBar(DataViewQuickBar dataViewQuickBar)
        {
            await UniTask.Yield();
        }
        
        public void InvokeOpenInventory(bool isActive)
        {
            OnOpenInventory?.Invoke(isActive);
        }

        protected void InvokeDrop(ViewSlot originalSlot, ViewSlot closestSlot, GhostIconView ghostIconView)
        {
            OnDrop?.Invoke(originalSlot, closestSlot, ghostIconView);
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
        
        protected ViewSlot FindClosestSlot(Vector2 position, List<ViewSlot> slots)
        {
            foreach (var slot in slots)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(slot.GetComponent<RectTransform>(), position))
                {
                    return slot;
                }
            }

            return null;
        }

        protected void InvokeCopyGhostIcon(ViewSlot slot, GhostIconView ghostIcon)
        {
            OnCopyGhostIcon?.Invoke(slot, ghostIcon);
        }
        
        protected async UniTaskVoid InitSlots(int capacity, ViewSlot prefab, RectTransform listSlots)
        {
            for (int i = 0; i < capacity; i++)
            {
                InitSlot(i, prefab, listSlots);
            }
            
            await UniTask.Yield();
        }
        
        protected void LoadAssets(out ViewSlot prefabSlot, out GhostIconView ghostIcon, string nameSlot, string nameIcon)
        {
            prefabSlot = ResourceManager.Instance
                .GetResources<GameObject>(ResourceManager.Instance.GetOrRegisterKey(nameSlot))
                .GetComponent<ViewSlot>();
            
            ghostIcon = ResourceManager.Instance
                .GetResources<GameObject>(ResourceManager.Instance.GetOrRegisterKey(nameIcon))
                .GetComponent<GhostIconView>();
        }
        
        protected void ClearSlots(RectTransform listSlots)
        {
            var slots = listSlots.GetComponentsInChildren<ViewSlot>();

            foreach (var slot in slots)
            {
                Destroy(slot);
            }
        }
        
        protected RectTransform InitGhostIcon(GhostIconView ghostIcon, RectTransform listSlots)
        {
            ghostIcon = Instantiate(ghostIcon, listSlots);
            ghostIcon.gameObject.SetActive(false);
            return ghostIcon.GetComponent<RectTransform>();
        }
        
        protected void AssignEventTriggers(ViewSlot slot, Action<PointerEventData> onBeginDrag, Action<PointerEventData> onDrag, Action<PointerEventData> onEndDrag)
        {
            InvokeEventTriggerAdd(EventTriggerType.BeginDrag, (eventData) => onBeginDrag((PointerEventData)eventData), slot);
            InvokeEventTriggerAdd(EventTriggerType.Drag, (eventData) => onDrag((PointerEventData)eventData), slot);
            InvokeEventTriggerAdd(EventTriggerType.EndDrag, (eventData) => onEndDrag((PointerEventData)eventData), slot);
        }
        
        
        private void InitSlot(int index, ViewSlot prefab, RectTransform listSlots)
        {
            var slot = Instantiate(prefab, listSlots);
            _isSlots.OnNext(slot);
            slot.SetIndex(index);
        }
    }
}
using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public abstract class StorageView : MonoBehaviour
    {
        [field: SerializeField] protected RectTransform listSlots;
        [field: SerializeField] protected ViewSlot prefabSlots;
        [field: SerializeField] protected GridLayoutGroup grid;
        [field: SerializeField] protected ViewSlot GhostIcon;
        protected Canvas canvas;
        protected Subject<bool> _isActiveGrid = new();
        public Observable<bool> IsActiveGrid => _isActiveGrid;
        public ViewSlot[] Slots { get; set; }
        public event Action<ViewSlot, ViewSlot> OnDrop;
        public event Action<ViewSlot, ViewSlot> OnCopy;
        public event Action<EventTriggerType, Action<BaseEventData>, ViewSlot> OnEventTriggerAdd;
        public abstract UniTask InitializeView(DataView dataView);

        private void Start()
        {
            canvas = GetComponent<Canvas>();
        }

        public void OnActiveGrid(bool isActive)
        {
            grid.enabled = isActive;
        }

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
    }
}
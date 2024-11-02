using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Loader;
using R3;
using Sync;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryView : StorageView
    {
        [field: SerializeField] protected RectTransform listSlots;
        private ViewSlot _prefabSlots;
        [field: SerializeField] protected GridLayoutGroup grid;
        private ViewSlot _ghostIcon;
        [field: SerializeField] protected Canvas canvas;
        [field: SerializeField] protected LoaderResources _loaderResources;
        private RectTransform _rectTransform;
        private List<ViewSlot> _slots;
        private bool _isLoad = false;

        private void Awake()
        {
            ResourceManager.Instance.OnAllLoaded.Subscribe((load) => _isLoad = load).AddTo(this);
        }

        public override async UniTask InitializeView(DataView dataView)
        {
            await UniTask.WaitUntil(() => _isLoad);
            LoadAssets();
            await InitializeSlots(dataView);
            await UniTask.Yield();
        }
        
        public void OnActiveGrid(bool isActive)
        {
            grid.enabled = isActive;
        }
        
        private async UniTask InitializeSlots(DataView dataView)
        {
            ClearSlots();
            InitSlots(dataView.Capacity).Forget();
            _slots = InvokeGetViewSlots();
            AddEvents(dataView.Capacity);
            InitGhostIcon();
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(listSlots);

            await ReloadGrid();
        }

        private void ClearSlots()
        {
            var slots = listSlots.GetComponentsInChildren<ViewSlot>();

            foreach (var slot in slots)
            {
                Destroy(slot);
            }
        }
        
        private void OnBeginDrag(PointerEventData handler)
        {
            if (!handler.pointerClick.TryGetComponent(out ViewSlot slot)) return;
            InvokeCopy(slot, _ghostIcon);
            slot.Clear();
            _ghostIcon.gameObject.SetActive(true);
            _rectTransform.anchoredPosition = slot.GetComponent<RectTransform>().anchoredPosition;
        }

        private void OnDrag(PointerEventData handler)
        {
            _rectTransform.anchoredPosition += handler.delta / canvas.scaleFactor;
        }

        private void OnEndDrag(PointerEventData handler)
        {
            _ghostIcon.gameObject.SetActive(false);
            
            ViewSlot closestSlot = FindClosestSlot(handler.position, _slots, canvas);
            
            if (closestSlot != null)
                InvokeDrop(_ghostIcon, closestSlot);
            else
                InvokeCopy(_ghostIcon, _slots[_ghostIcon.Index]);
        }
        
        private async UniTask ReloadGrid()
        {
            await UniTask.WaitForSeconds(0.5f);
            _isActiveGrid.OnNext(false);
        }

        private void InitSlot(int index)
        {
            var slot = Instantiate(_prefabSlots, listSlots);
            _isSlots.OnNext(slot);
            slot.SetIndex(index);
        }

        private void InitGhostIcon()
        {
            _ghostIcon = Instantiate(_ghostIcon, listSlots);
            _rectTransform = _ghostIcon.GetComponent<RectTransform>();
            _ghostIcon.gameObject.SetActive(false);
        }
        
        private void AssignEventTriggers(ViewSlot slot)
        {
            InvokeEventTriggerAdd(EventTriggerType.BeginDrag, (eventData) => OnBeginDrag((PointerEventData)eventData), slot);
            InvokeEventTriggerAdd(EventTriggerType.Drag, (eventData) => OnDrag((PointerEventData)eventData), slot);
            InvokeEventTriggerAdd(EventTriggerType.EndDrag, (eventData) => OnEndDrag((PointerEventData)eventData), slot);
        }

        private async UniTaskVoid InitSlots(int capacity)
        {
            for (int i = 0; i < capacity; i++)
            {
                InitSlot(i);
            }
            
            await UniTask.Yield();
        }

        private void AddEvents(int capacity)
        {
            for (int i = 0; i < capacity; i++)
            {
                AssignEventTriggers(_slots[i]);
            }
        }

        private void LoadAssets()
        {
            _prefabSlots = ResourceManager.Instance
                .GetResources<GameObject>(ResourceManager.Instance.GetOrRegisterKey(ResourcesName.Slot))
                .GetComponent<ViewSlot>();
            
            _ghostIcon = ResourceManager.Instance
                .GetResources<GameObject>(ResourceManager.Instance.GetOrRegisterKey(ResourcesName.Icon))
                .GetComponent<ViewSlot>();
        }
    }
}
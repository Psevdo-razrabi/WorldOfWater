using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sync;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryView : StorageView
    {
        [field: SerializeField] protected RectTransform listSlots;
        [field: SerializeField] protected GridLayoutGroup grid;
        [field: SerializeField] protected Canvas canvas;
        [field: SerializeField] protected CanvasGroup canvasGroup;
        [field: SerializeField] protected RectTransform inventoryView;
        private GhostIconView _ghostIcon;
        private ViewSlot _currentSlot;
        private ViewSlot _prefabSlots;
        private UiAnimation _uiAnimation;
        private RectTransform _rectTransform;
        private RectTransform _screenDescriptionTransform;
        private List<ViewSlot> _slots;

        public override async UniTask InitializeView(DataView dataView)
        {
            LoadAssets();
            await InitializeSlots(dataView);
            await UniTask.Yield();
        }
        
        public void OnActiveGrid(bool isActive)
        {
            grid.enabled = isActive;
        }

        public void OpenInventoryAnimation(bool isActive)
        {
            canvasGroup.blocksRaycasts = isActive;
            canvasGroup.interactable = isActive;
            _uiAnimation.AnimationWithPositionX(inventoryView, isActive ? 0 : 2000, 0.5f, Ease.OutBack);
        }
        
        private async UniTask InitializeSlots(DataView dataView)
        {
            ClearSlots();
            InitSlots(dataView.Capacity).Forget();
            InitTempSlot();
            _slots = InvokeGetViewSlots();
            _uiAnimation = dataView.Animation;
            AddEvents(dataView.Capacity);
            InitGhostIcon();
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(listSlots);

            await ReloadGrid();
        }

        private void InitTempSlot()
        {
            _currentSlot = Instantiate(_prefabSlots);
            _currentSlot.transform.localScale = Vector3.zero;
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
            
            InvokeCopy(slot, _currentSlot);
            InvokeCopyGhostIcon(slot, _ghostIcon);
            
            slot.Clear();
            _ghostIcon.gameObject.SetActive(true);
            _uiAnimation.AnimationWithAlpha(_ghostIcon.Image, 0.5f, 0.01f, Ease.OutQuad);
            _rectTransform.anchoredPosition = slot.GetComponent<RectTransform>().anchoredPosition;
        }

        private void OnDrag(PointerEventData handler)
        {
            _rectTransform.anchoredPosition += handler.delta / canvas.scaleFactor;
        }

        private void OnEndDrag(PointerEventData handler)
        {
            _ghostIcon.gameObject.SetActive(false);
            
            ViewSlot closestSlot = FindClosestSlot(handler.position, _slots);
            
            if (closestSlot != null)
                InvokeDrop(_currentSlot, closestSlot, _ghostIcon);
            else
                InvokeCopy(_currentSlot, _slots[_currentSlot.Index]);
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
                .GetComponent<GhostIconView>();
        }
    }
}
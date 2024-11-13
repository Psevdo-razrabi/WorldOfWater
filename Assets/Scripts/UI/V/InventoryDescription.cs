using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class InventoryDescription : StorageView
    {
        [SerializeField] private GameObject screenDescription;
        [SerializeField] private Canvas canvas;
        private readonly CancellationTokenSource _token = new();
        private UiAnimation _uiAnimation;
        private RectTransform _descriptionScreenTransform;
        private List<ViewSlot> _slots;
        private Item _currentItem;
        public event Action<Material, Mesh> SetMeshes = delegate { };
        public event Action<string, string> SetTexts = delegate { };

        private void Start()
        {
            _descriptionScreenTransform = screenDescription.GetComponent<RectTransform>();
        }

        public override async UniTask InitializeView(DataView dataView)
        {
            _slots = InvokeGetViewSlots();
            _uiAnimation = dataView.Animation;
            screenDescription.transform.localScale = Vector3.zero;
            
            foreach (var slot in _slots)
            {
                AssignEventTriggers(slot);
            }

            await UniTask.Yield();
        }
        
        private void OnPointerEnter(PointerEventData eventData)
        {
            var slot = FindClosestSlot(eventData.position, _slots, canvas);
            _currentItem = InvokeGetItem(slot.Index);

            if (_currentItem == null)
            {
                //Debug.Log("Slot is empty");
                return;
            }
            
            SetMeshes.Invoke(_currentItem.ItemDescription.Material, _currentItem.ItemDescription.Mesh);
            SetTexts.Invoke(_currentItem.ItemDescription.Description, _currentItem.ItemDescription.NameItem);
            
            ShowAndHideDescription(0f, 1f);
            _descriptionScreenTransform.anchoredPosition = eventData.delta / canvas.scaleFactor;
        }

        private void OnPointerExit(PointerEventData eventData)
        {
            if (_currentItem == null)
            {
                //Debug.Log("Slot is empty");
                return;
            }
            
            ShowAndHideDescription(1f, 0f);
        }

        private void AssignEventTriggers(ViewSlot slot)
        {
            InvokeEventTriggerAdd(EventTriggerType.PointerEnter, (eventData) => OnPointerEnter((PointerEventData)eventData), slot);
            InvokeEventTriggerAdd(EventTriggerType.PointerExit, (eventData) => OnPointerExit((PointerEventData)eventData), slot);
        }

        private void ShowAndHideDescription(float startValue, float endValue)
        {
            _uiAnimation.AnimationWithScale(startValue, endValue, screenDescription.transform, 0.3f, _token.Token).Forget();
        }
    }
}
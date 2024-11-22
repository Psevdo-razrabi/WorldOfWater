using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
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
        private ViewSlot _viewSlot;
        private Vector3 _prevMousePos;
        private readonly Vector2 _descriptionOffset = new (205f, -285f);
        private bool _isSelected;
        
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

            Observable
                .EveryUpdate()
                .Where(_ => _isSelected)
                .Subscribe(_ => SetDescriptionToMouse())
                .AddTo(this);
            
            foreach (var slot in _slots)
            {
                AssignEventTriggers(slot);
            }

            await UniTask.Yield();
        }
        
        private void OnPointerEnter(PointerEventData eventData)
        {
            _viewSlot = FindClosestSlot(eventData.position, _slots);
            _currentItem = InvokeGetItem(_viewSlot.Index);
            
            _uiAnimation.AnimationWithPunch(_viewSlot.transform, new Vector3(0.2f, 0.2f, 0f), 0.2f, Ease.OutBack).Forget();

            if (_currentItem == null)
            {
                //Debug.Log("Slot is empty");
                return;
            }
            
            SetMeshes.Invoke(_currentItem.ItemDescription.Material, _currentItem.ItemDescription.Mesh);
            SetTexts.Invoke(_currentItem.ItemDescription.Description, _currentItem.ItemDescription.NameItem);
            
            ShowAndHideDescription(0f, 1f);
            _descriptionScreenTransform.anchoredPosition = eventData.delta / canvas.scaleFactor;
            _isSelected = true;
        }

        private void OnPointerExit(PointerEventData eventData)
        {
            if (_currentItem == null)
            {
                //Debug.Log("Slot is empty");
                return;
            }
            
            ShowAndHideDescription(1f, 0f);
            _isSelected = false;
        }
        
        private void SetDescriptionToMouse()
        {
            if(Input.mousePosition == _prevMousePos) return;
            _prevMousePos = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out Vector2 movePos);
            _descriptionScreenTransform.transform.position = canvas.transform.TransformPoint(movePos + _descriptionOffset);
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
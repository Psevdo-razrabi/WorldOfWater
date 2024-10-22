using System;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryView : StorageView
    {
        private RectTransform _rectTransform;
        public override async UniTask InitializeView(DataView dataView)
        {
            await InitializeSlots(dataView);
            await UniTask.Yield();
        }
        
        private async UniTask InitializeSlots(DataView dataView)
        {
            Slots = new ViewSlot[dataView.Capacity];
            
            ClearSlots();
            for (int i = 0; i < Slots.Length; i++)
            {
                InitSlot(i);
                InvokeEventTriggerAdd(EventTriggerType.BeginDrag, (eventData) => OnBeginDrag((PointerEventData)eventData), Slots[i]);
                InvokeEventTriggerAdd(EventTriggerType.Drag, (eventData) => OnDrag((PointerEventData)eventData), Slots[i]);
                InvokeEventTriggerAdd(EventTriggerType.EndDrag, (eventData) => OnEndDrag((PointerEventData)eventData), Slots[i]);
            }
            
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
            InvokeCopy(slot, GhostIcon);
            slot.Clear();
            GhostIcon.gameObject.SetActive(true);
            _rectTransform.anchoredPosition = slot.GetComponent<RectTransform>().anchoredPosition;
        }

        private void OnDrag(PointerEventData handler)
        {
            _rectTransform.anchoredPosition += handler.delta / canvas.scaleFactor;
        }

        private void OnEndDrag(PointerEventData handler)
        {
            GhostIcon.gameObject.SetActive(false);
            
            ViewSlot closestSlot = FindClosestSlot(handler.position);
            
            if (closestSlot != null)
                InvokeDrop(GhostIcon, closestSlot);
            else
                InvokeCopy(GhostIcon, Slots[GhostIcon.Index]);
        }

        private ViewSlot FindClosestSlot(Vector2 position)
        {
            foreach (var slot in Slots)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(slot.GetComponent<RectTransform>(), position, canvas.worldCamera))
                {
                    return slot;
                }
            }

            return null;
        }
        
        private async UniTask ReloadGrid()
        {
            await UniTask.WaitForSeconds(0.5f);
            _isActiveGrid.OnNext(false);
        }

        private void InitSlot(int index)
        {
            var slot = Instantiate(prefabSlots, listSlots);
            Slots[index] = slot;    
            Slots[index].SetIndex(index);
        }

        private void InitGhostIcon()
        {
            GhostIcon = Instantiate(GhostIcon, listSlots);
            _rectTransform = GhostIcon.GetComponent<RectTransform>();
            GhostIcon.gameObject.SetActive(false);
        }
    }
}
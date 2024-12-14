using Cysharp.Threading.Tasks;
using Inventory;
using QuickSlot.Db;
using Sync;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QuickSlot
{
    public class QuickBarSlotView : StorageView
    {
        [field: SerializeField] private RectTransform _listSlots;
        private ViewSlot _prefab;
        private GhostIconView _ghostIcon;
        private RectTransform _rectTransform;
        private ViewSlot _currentSlot;

        public override async UniTask InitializeViewQuickBar(DataViewQuickBar dataViewQuickBar)
        {
            LoadAssets(out _prefab, out _ghostIcon, ResourcesName.Slot, ResourcesName.Icon);
            await InitializeSlots(dataViewQuickBar.Capacity);
        }
        
        private async UniTask InitializeSlots(int capacity)
        {
            ClearSlots(_listSlots);
            InitSlots(capacity, _prefab, _listSlots).Forget();
            InitTempSlot();
            AddEvents(capacity);
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(_listSlots);

            _rectTransform = InitGhostIcon(_ghostIcon, _listSlots);
            await UniTask.Yield();
        }
        
        private void InitTempSlot()
        {
            _currentSlot = Instantiate(_prefab);
            _currentSlot.transform.localScale = Vector3.zero;
        }
        
        private void AddEvents(int capacity)
        {
            for (int i = 0; i < capacity; i++)
            {
                //AssignEventTriggers(_slots[i], OnBeginDrag, OnDrag, OnEndDrag);
            }
        }

        private void OnEndDrag(PointerEventData obj)
        {
            
        }

        private void OnDrag(PointerEventData obj)
        {
            
        }

        private void OnBeginDrag(PointerEventData obj)
        {
            
        }
    }
}
using System;
using System.Collections.Generic;
using Helpers;
using Inventory;
using QuickSlot.Db;
using R3;
using Zenject;

namespace QuickSlot
{
    public class QuickBarSlotPresenter : StoragePresenter, IInitializable, IDisposable
    {
        private readonly QuickBarSlotView _quickBarSlotView;
        private readonly QuickBarSlotsModel _quickBarSlotsModel;
        private QuickBarParameters _parameters;
        
        private CompositeDisposable _compositeDisposable = new();
        private List<ViewSlot> _slots = new();
        

        public QuickBarSlotPresenter(QuickBarSlotsModel quickBarSlotsModel, QuickBarSlotView quickBarSlotView)
        {
            _quickBarSlotsModel = quickBarSlotsModel;
            _quickBarSlotView = quickBarSlotView;
        }

        public async void Initialize()
        {
            _quickBarSlotsModel.Initialize();
            var data = _quickBarSlotsModel.DataViewQuickBar;
            await _quickBarSlotView.InitializeViewQuickBar(data);
            Subscribe();
        }
        
        public void Dispose()
        {
            
        }
        
        private void HandleSlot(ViewSlot originalSlot, ViewSlot closestSlot, GhostIconView ghostIconView)
        {
            if (originalSlot.Index == closestSlot.Index || closestSlot.GuidItem.Equals(GuidItem.ToEmpty()))
            {
                _quickBarSlotsModel.Swap(originalSlot.Index, closestSlot.Index);
                return;
            }

            if (_quickBarSlotsModel.TryGet(originalSlot.Index, out var originalItemSlot))
            {
                var closestItemSlot = _quickBarSlotsModel.Get(closestSlot.Index);
                
                OperationWithModel(originalItemSlot, closestItemSlot, _quickBarSlotsModel, originalSlot.Index, closestSlot.Index);
            }
            
            ghostIconView.Clear();
        }
        
        private void Subscribe()
        {
            _quickBarSlotView.OnEventTriggerAdd += AddEventTrigger;
            _quickBarSlotView.OnGetViewSlots += GetViewSlots;
            
            _quickBarSlotView.OnDrop += HandleSlot;
            _quickBarSlotView.OnCopy += CopySlot;
            _quickBarSlotView.OnCopyGhostIcon += CopyParametersToGhostIcon;
            
            _quickBarSlotView.Slots
                .Subscribe(slot =>
                {
                    _slots.Add(slot);
                    _quickBarSlotsModel.Update();
                })
                .AddTo(_compositeDisposable);
            
            _quickBarSlotsModel.OnModelChange
                .Subscribe(_ => HandleModelChange(_parameters.capacity, _quickBarSlotsModel, _slots))
                .AddTo(_compositeDisposable);
        }
        
        private List<ViewSlot> GetViewSlots() => _slots;
    }
}
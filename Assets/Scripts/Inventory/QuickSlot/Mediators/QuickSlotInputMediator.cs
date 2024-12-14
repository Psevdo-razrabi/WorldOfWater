using System;
using Inventory.QuickSlot.Enums;
using NewInput;
using QuickSlot;
using Zenject;

namespace Inventory.QuickSlot.Mediators
{
    public class QuickSlotInputMediator : IInitializable, IDisposable
    {
        private readonly UiInput _uiInput;
        private readonly QuickBarSlotsModel _quickBarSlotsModel;
        
        public QuickSlotInputMediator(UiInput uiInput, QuickBarSlotsModel quickBarSlotsModel)
        {
            _uiInput = uiInput;
            _quickBarSlotsModel = quickBarSlotsModel;
        }

        private void DetermineQuickSlotType(bool isActive, EQuickSlot eQuickSlot)
        {
            switch (eQuickSlot)
            {
                case EQuickSlot.FirstSlot :
                    _quickBarSlotsModel.InvokeSlotSelected(isActive, (int)eQuickSlot);
                    break;
                case EQuickSlot.SecondSlot :
                    _quickBarSlotsModel.InvokeSlotSelected(isActive, (int)eQuickSlot);
                    break;
                case EQuickSlot.ThirdSlot :
                    _quickBarSlotsModel.InvokeSlotSelected(isActive, (int)eQuickSlot);
                    break;
                case EQuickSlot.FourthSlot :
                    _quickBarSlotsModel.InvokeSlotSelected(isActive, (int)eQuickSlot);
                    break;
                case EQuickSlot.FifthSlot :
                    _quickBarSlotsModel.InvokeSlotSelected(isActive, (int)eQuickSlot);
                    break;
                case EQuickSlot.SixthSlot :
                    _quickBarSlotsModel.InvokeSlotSelected(isActive, (int)eQuickSlot);
                    break;
            }
        }

        public void Dispose()
        {
            _uiInput.FirstSlotQuickBar -= DetermineQuickSlotType;
            _uiInput.SecondSlotQuickBar -= DetermineQuickSlotType;
            _uiInput.ThirdSlotQuickBar -= DetermineQuickSlotType;
            _uiInput.FourthSlotQuickBar -= DetermineQuickSlotType;
            _uiInput.FifthSlotQuickBar -= DetermineQuickSlotType;
            _uiInput.SixthSlotQuickBar -= DetermineQuickSlotType;
            _uiInput?.Dispose();
        }

        public void Initialize()
        {
            _uiInput.FirstSlotQuickBar += DetermineQuickSlotType;
            _uiInput.SecondSlotQuickBar += DetermineQuickSlotType;
            _uiInput.ThirdSlotQuickBar += DetermineQuickSlotType;
            _uiInput.FourthSlotQuickBar += DetermineQuickSlotType;
            _uiInput.FifthSlotQuickBar += DetermineQuickSlotType;
            _uiInput.SixthSlotQuickBar += DetermineQuickSlotType;
        }
    }
}
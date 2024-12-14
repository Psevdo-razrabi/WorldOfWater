using System;
using Inventory.QuickSlot.Enums;
using UnityEngine.InputSystem;
using Zenject;

namespace NewInput
{
    public class UiInput : PlayerInput.IUiActions, IDisposable, IInitializable
    {
        public event Action DropItem;
        public event Action<bool, EQuickSlot> FirstSlotQuickBar;
        public event Action<bool, EQuickSlot> SecondSlotQuickBar;
        public event Action<bool, EQuickSlot> ThirdSlotQuickBar;
        public event Action<bool, EQuickSlot> FourthSlotQuickBar;
        public event Action<bool, EQuickSlot> FifthSlotQuickBar;
        public event Action<bool, EQuickSlot> SixthSlotQuickBar;

        private bool _isActive;
        

        private PlayerInput _playerInput;

        public UiInput(PlayerInput playerInput)
        {
            _playerInput = playerInput;
        }

        public void OnFirstSlotQuickBar(InputAction.CallbackContext context)
        {
            OnQuickBarSlot(context, FirstSlotQuickBar, EQuickSlot.FirstSlot);
        }

        public void OnSecondSlotQuickBar(InputAction.CallbackContext context)
        {
            OnQuickBarSlot(context, SecondSlotQuickBar, EQuickSlot.SecondSlot);
        }

        public void OnThirdSlotQuickBar(InputAction.CallbackContext context)
        {
            OnQuickBarSlot(context, ThirdSlotQuickBar, EQuickSlot.ThirdSlot);
        }

        public void OnFourthSlotQuickBar(InputAction.CallbackContext context)
        {
            OnQuickBarSlot(context, FourthSlotQuickBar, EQuickSlot.FourthSlot);
        }

        public void OnFifthSlotQuickBar(InputAction.CallbackContext context)
        {
            OnQuickBarSlot(context, FifthSlotQuickBar, EQuickSlot.FifthSlot);
        }

        public void OnSixthSlotQuickBar(InputAction.CallbackContext context)
        {
            OnQuickBarSlot(context, SixthSlotQuickBar, EQuickSlot.SixthSlot);
        }
        

        public void OnDropItem(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed :
                    DropItem?.Invoke();
                    break;
            }
        }

        public void Dispose()
        {
            _playerInput?.Dispose();
        }

        public void Initialize()
        {
            EnablePlayerAction();
        }
        
        private void EnablePlayerAction()
        {
            _playerInput.Ui.SetCallbacks(this);
            _playerInput.Enable();
        }
        
        private void OnQuickBarSlot(InputAction.CallbackContext context, Action<bool, EQuickSlot> slotAction, EQuickSlot quickSlot)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    _isActive = !_isActive;
                    slotAction?.Invoke(_isActive, quickSlot);
                    break;
            }
        }
    }
}
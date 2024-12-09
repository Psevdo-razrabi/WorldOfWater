using System;
using UnityEngine.InputSystem;
using Zenject;

namespace NewInput
{
    public class UiInput : PlayerInput.IUiActions, IDisposable, IInitializable
    {
        public event Action DropItem;

        private PlayerInput _playerInput;

        public UiInput(PlayerInput playerInput)
        {
            _playerInput = playerInput;
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
    }
}
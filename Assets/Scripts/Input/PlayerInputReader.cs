using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewInput
{
    public class PlayerInputReader : PlayerInput.IPlayerActions, IInputReader, IDisposable
    {
        public event Action<Vector3> Move = delegate { };
        public event Action<bool> Jump = delegate { };
        public event Action TakeItem = delegate { };
        public event Action OpenInventory = delegate { };
        
        public Vector3 Direction
        {
            get
            {
                var direction = _playerInput.Player.Move.ReadValue<Vector2>();
                return new Vector3(direction.x, 0f, direction.y);
            }
        }
        
        private PlayerInput _playerInput;
        private InputObserver _inputObserver;
        
        public void EnablePlayerAction()
        {
            if (_playerInput == null)
            {
                _playerInput = new PlayerInput();
                _inputObserver = new InputObserver(_playerInput);
                _playerInput.Player.SetCallbacks(this);
            }
            _playerInput.Enable();
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            Move?.Invoke(Direction);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Jump?.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Jump?.Invoke(false);
                    break;
            }
        }

        public void OnTakeItem(InputAction.CallbackContext context)
        {
            TakeItem?.Invoke();
        }

        public void OnOpenInventory(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    OpenInventory?.Invoke();
                    break;
            }
        }

        public void Dispose()
        {
            _playerInput?.Dispose();
        }
    }
}
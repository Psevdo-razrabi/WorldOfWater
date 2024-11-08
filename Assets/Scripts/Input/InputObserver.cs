using System;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
public class InputObserver
    {
        private PlayerInput _inputSystem;
        private bool _isMouseLeftButton = true;
        private bool _isMouseRightButton = true;
        private bool _isButtonCrouchDown = true;

        public InputObserver(PlayerInput inputSystem)
        {
            _inputSystem = inputSystem ?? throw new ArgumentNullException();
        }

        public IObservable<Unit> SubscribeInventoryUp()
        {
            return Observable.Create<Unit>(observer =>
            {
                void OnInventoryButtonUp(InputAction.CallbackContext ctx)
                {
                    if (_isMouseLeftButton)
                    {
                        _isMouseLeftButton = false;
                        observer.OnNext(Unit.Default);
                    }
                }

                _inputSystem.Player.OpenInventory.performed += OnInventoryButtonUp;

                return Disposable.Create(() => _inputSystem.Player.OpenInventory.performed -= OnInventoryButtonUp);
            });
        }
        
        public IObservable<Unit> SubscribeInventoryDown()
        {
            return Observable.Create<Unit>(observer =>
            {
                void OnInventoryButtonDown(InputAction.CallbackContext ctx)
                {
                    if (_isMouseLeftButton == false)
                    {
                        _isMouseLeftButton = true;
                        observer.OnNext(Unit.Default);
                    }
                }

                _inputSystem.Player.OpenInventory.canceled += OnInventoryButtonDown;

                return Disposable.Create(() =>
                {
                    _inputSystem.Player.OpenInventory.canceled -= OnInventoryButtonDown;
                });
            });
        }
    }
}
using System;
using UnityEngine;
using Zenject;

namespace Sources.Scripts.Hook.InputControllers
{
    public class InputController : MonoBehaviour
    {
        private InputSystem _inputSystem;
        private InputSwitcher _inputSwitcher;
        private ThrowInputContoller _throwInputContoller;
        private HookOnWaterInputController _hookOnWaterInputController;
        
        [Inject]
        public void Construct(InputSystem inputSystem, InputSwitcher inputSwitcher, ThrowInputContoller throwInputContoller, HookOnWaterInputController hookOnWaterInputController)
        {
            _inputSystem = inputSystem;
            _inputSwitcher = inputSwitcher;
            _throwInputContoller = throwInputContoller;
            _hookOnWaterInputController = hookOnWaterInputController;
        }

        private void Awake()
        {
            EnableInputSystem();
            _inputSwitcher.SwitchController(_throwInputContoller);
        }

        public void SwitchInput()
        {
            _inputSwitcher.SwitchController(_hookOnWaterInputController);
        }
        
        public void EnableInputSystem() => _inputSystem.Enable();
        
        public void DisableInputSystem() => _inputSystem.Disable();
    }
}
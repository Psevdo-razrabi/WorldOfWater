using Sources.Scripts.Hook.InputControllers;
using UnityEngine;

namespace Game.DI
{
    public class InputInstaller : BaseBindings
    {
        [SerializeField] private InputController _inputController;
        
        public override void InstallBindings()
        {
            BindInputSystem();
            BindInputLogicSystem();
        }
        
        private void BindInputSystem()
        { 
            BindNewInstance<InputSystem>();
        }
        
        private void BindInputLogicSystem()
        {
            BindInstance(_inputController);
            BindNewInstance<InputSwitcher>();
            BindNewInstance<ThrowInputContoller>();
            BindNewInstance<HookOnWaterInputController>();
        }
    }
}
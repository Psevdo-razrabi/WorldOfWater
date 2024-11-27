using Sources.Scripts.Hook.InputControllers;
using Sources.Scripts.UI.ThrowToolsUI;
using UnityEngine.InputSystem;
using Zenject;

public class ThrowInputContoller : IInputController
{
    private InputSystem _inputSystem;
    private ThrowHook _hook;
    private ThrowToolsPresenter _toolPresenter;
    
    [Inject]
    public ThrowInputContoller(InputSystem inputSystem, ThrowHook hook, ThrowToolsPresenter toolPresenter)
    {
        _inputSystem = inputSystem;
        _hook = hook;
        _toolPresenter = toolPresenter;
    }

    public void Enable()
    {
        SubscribeThrowHook();
        SubscribeHookBack();
    }

    public void Disable()
    {
        UnSubscribeThrowHook();
        UnSubscribeHookBack();
    }
    
    private void SubscribeHookBack() => _inputSystem.Game.HookBack.performed += OnHookBackPerfomed;
    private void UnSubscribeHookBack() => _inputSystem.Game.HookBack.performed -= OnHookBackPerfomed;
    
    private void SubscribeThrowHook()
    {
        _inputSystem.Game.HookThrow.performed += OnThrowHookPerfomed;
        _inputSystem.Game.HookThrow.canceled += OnThrowHookCanceled;
    }
    
    private void UnSubscribeThrowHook()
    {
        _inputSystem.Game.HookThrow.performed -= OnThrowHookPerfomed;
        _inputSystem.Game.HookThrow.canceled -= OnThrowHookCanceled;
    }   
    
    private void OnThrowHookPerfomed(InputAction.CallbackContext obj)
    {
        _toolPresenter.ThrowPerfermed();
    }
    
    private void OnThrowHookCanceled(InputAction.CallbackContext obj)
    {
        _toolPresenter.ThrowCanceled();
    }
    
    private void OnHookBackPerfomed(InputAction.CallbackContext obj)
    {
        _hook.HookBackPerformed();
    }
}
 
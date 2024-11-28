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
        SubscribeThrowTool();
        SubscribeToolBack();
    }

    public void Disable()
    {
        UnSubscribeThrowTool();
        UnSubscribeToolBack();
    }
    
    private void SubscribeToolBack() => _inputSystem.Game.ToolBack.performed += OnToolBackPerfomed;
    private void UnSubscribeToolBack() => _inputSystem.Game.ToolBack.performed -= OnToolBackPerfomed;
    
    private void SubscribeThrowTool()
    {
        _inputSystem.Game.ToolThrow.performed += OnThrowToolPerfomed;
        _inputSystem.Game.ToolThrow.canceled += OnThrowToolCanceled;
    }
    
    private void UnSubscribeThrowTool()
    {
        _inputSystem.Game.ToolThrow.performed -= OnThrowToolPerfomed;
        _inputSystem.Game.ToolThrow.canceled -= OnThrowToolCanceled;
    }   
    
    private void OnThrowToolPerfomed(InputAction.CallbackContext obj)
    {
        _toolPresenter.ThrowPerfermed();
    }
    
    private void OnThrowToolCanceled(InputAction.CallbackContext obj)
    {
        _toolPresenter.ThrowCanceled();
    }
    
    private void OnToolBackPerfomed(InputAction.CallbackContext obj)
    {
        _hook.HookBackPerformed();
    }
}
 
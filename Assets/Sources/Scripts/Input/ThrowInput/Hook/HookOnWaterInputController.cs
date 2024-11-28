using Sources.Scripts.Hook.InputControllers;
using UnityEngine.InputSystem;
using Zenject;

public class HookOnWaterInputController : IInputController
{
    private InputSystem _inputSystem;
    private ThrowHook _hook;
    
    [Inject]
    public HookOnWaterInputController(InputSystem inputSystem, ThrowHook hook)
    {
        _inputSystem = inputSystem;
        _hook = hook;
    }
    
    public void Enable()
    {
        SubscribePullHook();
        SubscribeBackHook();
    }

    public void Disable()
    {
        UnSubscribePullHook();
        UnSubscribeBackHook();
    }

    private void SubscribePullHook()
    {
        _inputSystem.Game.WaterBack.started += PullHookStarted;
        _inputSystem.Game.WaterBack.canceled += PullHookCanceled;
    }

    private void UnSubscribePullHook()
    {
        _inputSystem.Game.WaterBack.started -= PullHookStarted;
        _inputSystem.Game.WaterBack.canceled -= PullHookCanceled;
    }

    private void SubscribeBackHook() => _inputSystem.Game.ToolBack.performed += HookBackPerformed;
    
    private void UnSubscribeBackHook() => _inputSystem.Game.ToolBack.performed -= HookBackPerformed;

    private void PullHookStarted(InputAction.CallbackContext obj)
    {
        _hook.StartPullingHook();
    }

    private void PullHookCanceled(InputAction.CallbackContext obj)
    {
        _hook.StopPullingHook();
    }

    private void HookBackPerformed(InputAction.CallbackContext obj)
    {
        _hook.HookBackPerformed();
    }
}
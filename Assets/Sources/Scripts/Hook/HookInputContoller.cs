using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class HookInputContoller : MonoBehaviour
{
    private InputSystem _inputSystem;
    private Hook _hook;
    
    [Inject]
    private void Construct(Hook hook)
    {
        _hook = hook;
    }

    private void Awake()
    {
        _inputSystem = new InputSystem();
        _inputSystem.Enable();
    }

    private void OnEnable()
    {
        SubscribeThrowHook();
        SubscribeHookBack();
    }
    
    private void OnDisable()
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
        _hook.HookThrowPerfermed();
    }
    
    private void OnThrowHookCanceled(InputAction.CallbackContext obj)
    {
        _hook.HookThrowCanceled();
    }
    
    private void OnHookBackPerfomed(InputAction.CallbackContext obj)
    {
        _hook.HookBackPerfermed();
    }
}
 
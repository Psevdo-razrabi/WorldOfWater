using System.Collections;
using System.Collections.Generic;
using Game.DI;
using UnityEngine;

public class HookInstaller : BaseBindings
{
    [SerializeField] private Hook _hook;
    
    public override void InstallBindings()
    {
        BindHook();
    }

    private void BindHook() => BindInstance(_hook);
}

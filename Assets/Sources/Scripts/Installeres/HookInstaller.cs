using Game.DI;
using Sources.Scripts.Items;
using Sources.Scripts.UI.ThrowToolsUI;
using UnityEngine;

public class HookInstaller : BaseBindings
{
    [SerializeField] private Hook _hook;
    [SerializeField] private ThrowHook _throwHook;
    [SerializeField] private CircleLoadbarView _circleLoadbarView;
    
    
    public override void InstallBindings()
    {
        BindHookLogic();
        BindUIHook();
    }

    private void BindHookLogic()
    { 
        BindInstance(_hook);
        BindInstance(_throwHook);
    }
    private void BindUIHook()
    {
        BindNewInstance<ThrowToolsPresenter>();
        BindNewInstance<HookModel>();
        BindNewInstance<TakedItemsData>();
        BindInstance(_circleLoadbarView);
    }
}

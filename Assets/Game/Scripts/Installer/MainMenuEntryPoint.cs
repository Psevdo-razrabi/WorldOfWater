using Game.MVVM.Menu;
using Game.Services;
using UnityEngine;
using VContainer;

namespace Game.DI
{
    public class MainMenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private MainMenuLifetimeScope _lifetimeScope;
        
        private void Awake()
        {
            var viewsService = _lifetimeScope.Container.Resolve<ViewsService>();
            viewsService.Initialize();
            viewsService.Open<MainMenuView>();
        }
    }
}

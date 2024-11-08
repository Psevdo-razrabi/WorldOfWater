using Game.MVVM.Menu;
using Game.Services;
using UnityEngine;
using VContainer;

namespace Game.DI
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        [SerializeField] private GameplayLifetimeScope _lifetimeScope;
        
        private void Awake()
        {
            var viewsService = _lifetimeScope.Container.Resolve<ViewsService>();
            viewsService.Initialize();
            viewsService.Open<LobbyView>();
        }
    }
}
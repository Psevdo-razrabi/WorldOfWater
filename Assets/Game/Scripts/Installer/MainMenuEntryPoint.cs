using Game.Services;
using UnityEngine;
using VContainer;

namespace Game.DI
{
    public class MainMenuEntryPoint : MonoBehaviour
    {
        private ViewsService _viewsService;
        
        private void Awake()
        {
            _viewsService.Initialize();
        }

        [Inject]
        private void Construct(ViewsService viewsService)
        {
            _viewsService = viewsService;
        }
    }
}

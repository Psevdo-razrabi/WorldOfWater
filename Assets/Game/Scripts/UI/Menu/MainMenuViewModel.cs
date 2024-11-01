using Game.Services;
using UnityEngine;
using VContainer;

namespace Game.MVVM.Menu
{
    public class MainMenuViewModel : ViewModel
    {
        private ViewsService _viewsService;
        private AuthenticationsService _authenticationsService;

        [Inject]
        private void Construct(ViewsService viewsService, AuthenticationsService authenticationsService)
        {
            _viewsService = viewsService;
            _authenticationsService = authenticationsService;
        }

        public void Init(IBindable singleplayerButton, IBindable multiplayerButton)
        {
            Binder.CreateButtonTriggers<Click>(new()
            {
                new(singleplayerButton, OnSingleplayerClicked),
                new(multiplayerButton, OnMultiplayerClicked)
            });
        }

        private void OnSingleplayerClicked()
        {
            Debug.Log("singleplayer started");
        }

        private async void OnMultiplayerClicked()
        {
            await _authenticationsService.InitServices();
            await _authenticationsService.SignInAnonymously();
            
            _viewsService.Close();
            _viewsService.Open<CreateWorldView>();
            _viewsService.Open<JoinWorldView>();
        }
    }
}

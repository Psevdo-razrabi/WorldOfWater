using Game.Services;
using R3;
using VContainer;

namespace Game.MVVM.Menu
{
    public class LobbyViewModel : ViewModel
    {
        private LobbiesService _lobbiesService;
        private ScenesService _scenesService;

        public void Init(IBindable leaveButton)
        {
            Binder.CreateButtonTrigger<Click>(leaveButton, OnLeaved);
        }

        [Inject]
        private void Construct(LobbiesService lobbiesService, ScenesService scenesService)
        {
            _lobbiesService = lobbiesService;
            _scenesService = scenesService;
        }
        
        private async void OnLeaved()
        {
            await _lobbiesService.LeaveLobby();
            await _scenesService.LoadScene(SceneType.MainMenu);
        }
    }
}
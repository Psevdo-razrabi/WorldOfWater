using Game.DI;
using Game.Services;
using UnityEngine.SceneManagement;
using VContainer;

namespace Game.MVVM.Menu
{
    public class JoinWorldViewModel : ViewModel
    {
        private ViewsService _viewsService;
        private LobbiesService _lobbiesService;
        private ScenesService _scenesService;

        public string Code { get; set; }

        public void Init(IBindable joinButton)
        {
            Binder.CreateButtonTrigger<Click>(joinButton, OnJoined);
        }

        [Inject]
        private void Construct(ViewsService viewsService, LobbiesService lobbiesService,
            ScenesService scenesService)
        {
            _viewsService = viewsService;
            _lobbiesService = lobbiesService;
            _scenesService = scenesService;
        }

        private async void OnJoined()
        {
            await _scenesService.LoadScene(SceneType.Gameplay);
            await _lobbiesService.JoinLobby(Code);
        }
    }
}
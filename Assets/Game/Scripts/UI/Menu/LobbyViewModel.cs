using Game.Services;
using R3;
using VContainer;

namespace Game.MVVM.Menu
{
    public class LobbyViewModel : ViewModel
    {
        private ViewsService _viewsService;
        private LobbiesService _lobbiesService;
        private ScenesService _scenesService;

        public string JoinCode => _lobbiesService.JoinCode;

        public void Init(IBindable leaveButton)
        {
            Binder.CreateButtonTrigger<Click>(leaveButton, OnLeaved);
            _lobbiesService.Connected.Subscribe(UpdateData).AddTo(Binder.Disposable);
        }

        [Inject]
        private void Construct(ViewsService viewsService, LobbiesService lobbiesService, ScenesService scenesService)
        {
            _viewsService = viewsService;
            _lobbiesService = lobbiesService;
            _scenesService = scenesService;
        }

        private void UpdateData()
        {
            Binder.TriggerView();
        }
        
        private async void OnLeaved()
        {
            await _lobbiesService.LeaveLobby();

            await _scenesService.LoadScene(SceneType.MainMenu);

            /*_viewsService.Close();
            _viewsService.Open(ViewId.CreateWorld);
            _viewsService.Open(ViewId.JoinWorld);*/
        }
    }
}
using Game.Services;
using VContainer;

namespace Game.MVVM.Menu
{
    public class LobbyViewModel : ViewModel
    {
        private ViewsService _viewsService;
        private LobbiesService _lobbiesService;

        public string Code => _lobbiesService.LobbyCode;

        public void Init(IBindable leaveButton)
        {
            Binder.CreateButtonTrigger<Click>(leaveButton, OnLeaved);
        }

        [Inject]
        private void Construct(ViewsService viewsService, LobbiesService lobbiesService)
        {
            _viewsService = viewsService;
            _lobbiesService = lobbiesService;
        }

        private async void OnLeaved()
        {
            await _lobbiesService.LeaveLobby();

            /*_viewsService.Close();
            _viewsService.Open(ViewId.CreateWorld);
            _viewsService.Open(ViewId.JoinWorld);*/
        }
    }
}
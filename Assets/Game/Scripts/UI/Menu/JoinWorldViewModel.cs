using Game.Services;
using VContainer;

namespace Game.MVVM.Menu
{
    public class JoinWorldViewModel : ViewModel
    {
        private ViewsService _viewsService;
        private LobbiesService _lobbiesService;

        public string Code { get; set; }

        public void Init(IBindable joinButton)
        {
            Binder.CreateButtonTrigger<Click>(joinButton, OnJoined);
        }

        [Inject]
        private void Construct(ViewsService viewsService, LobbiesService lobbiesService)
        {
            _viewsService = viewsService;
            _lobbiesService = lobbiesService;
        }

        private async void OnJoined()
        {
            if (await _lobbiesService.TryJoinLobby(Code))
            {
                _viewsService.Close();
                _viewsService.Close();
                _viewsService.Open(ViewId.Lobby);
            }
        }
    }
}
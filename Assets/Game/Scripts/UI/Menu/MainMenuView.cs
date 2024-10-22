using Game.Services;
using UnityEngine;
using Zenject;

namespace Game.MVVM.Menu
{
    public class MainMenuView : View<MainMenuViewModel>
    {
        [SerializeField] private Button _playButton;

        public override string Id => ViewIds.MAIN_MENU;

        public override void Init()
        {
            ViewModel.Init(_playButton);
        }
    }

    public class MainMenuViewModel : ViewModel
    {
        private ViewsService _viewsService;

        [Inject]
        private void Construct(ViewsService viewsService)
        {
            _viewsService = viewsService;
        }

        public void Init(IBindable playButton)
        {
            Binder.CreateButtonTrigger<Click>(playButton, OnPlayClicked);
        }

        private void OnPlayClicked()
        {
            _viewsService.Close();
            _viewsService.Open(ViewIds.SESSION_SETTINGS);
        }
    }
}

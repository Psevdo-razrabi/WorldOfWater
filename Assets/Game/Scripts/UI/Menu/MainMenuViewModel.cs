using Game.Services;
using VContainer;

namespace Game.MVVM.Menu
{
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
        }
    }
}

using Game.Services;
using VContainer;

namespace Game.MVVM.Menu
{
    public class WorldSetupViewModel : ViewModel
    {
        private ViewsService _viewsService;

        [Inject]
        private void Construct(ViewsService viewsService)
        {
            _viewsService = viewsService;
        }

        public void Init(IBindable create, IBindable load, IBindable join)
        {
            Binder.CreateButtonTriggers<Click>(new()
            {
                new(create, OnCreateWorld),
                new(load, OnLoadWorld),
                new(join, OnJoinWorld)
            });
        }

        private void OnCreateWorld()
        {

        }

        private void OnLoadWorld()
        {

        }

        private void OnJoinWorld()
        {/*
            _viewsService.Close();
            _viewsService.Open(ViewIds.SESSION_SETTINGS);*/
        }
    }
}

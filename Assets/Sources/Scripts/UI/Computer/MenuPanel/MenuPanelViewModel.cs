using Game.Services;
using Zenject;

namespace Game.MVVM.Computer
{
    public class MenuPanelViewModel : ViewModel
    {
        private ViewsService _viewsService;

        [Inject]
        private void Construct(ViewsService viewsService)
        {
            _viewsService = viewsService;
        }

        public void Init(IBindable contractsButton, IBindable mapButton, IBindable shopButton, IBindable consoleButton)
        {
            Binder.CreateButtonTriggers<Click>(new()
            {
                new(contractsButton, OnClickedContractsButton),
                new(mapButton, OnClickedMapButton),
                new(shopButton, OnClickedShopButton),
                new(consoleButton, OnClickedConsoleButton)
            });
        }

        private void OnClickedContractsButton()
        {
            Binder.TriggerView();
            _viewsService.Close();
            _viewsService.Open(ViewIds.CONTRACTS);
        }

        private void OnClickedMapButton()
        {
            _viewsService.Close();
            _viewsService.Open(ViewIds.MAP);
        }

        private void OnClickedShopButton()
        {
            _viewsService.Close();
            _viewsService.Open(ViewIds.SHOP);
        }

        private void OnClickedConsoleButton()
        {
            _viewsService.Close();
            _viewsService.Open(ViewIds.CONSOLE);
        }
    }
}

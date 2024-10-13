using Game.Services;
using UnityEngine;
using Zenject;

namespace Game.MVVM.MenuPanel
{
    public class MenuPanelView : View
    {
        [SerializeField] private Button _contractsButton;
        [SerializeField] private Button _mapButton;
        [SerializeField] private Button _consoleButton;

        private MenuPanelViewModel _viewModel;

        public override void Init(ViewModelFactory viewModelFactory)
        {
            Id = ViewIds.MENU_PANEL;

            _viewModel = viewModelFactory.Create<MenuPanelViewModel>();

            _viewModel.Init(_contractsButton, _mapButton, _consoleButton);
        }
    }

    public class MenuPanelViewModel : ViewModel
    {
        private ViewsService _viewsService;

        [Inject]
        private void Construct(ViewsService viewsService)
        {
            _viewsService = viewsService;
        }

        public void Init(IBindable contractsButton, IBindable mapButton, IBindable consoleButton)
        {
            Binder.CreateButtonEvent<ClickBinderEvent>(contractsButton, OnClickedContractsButton);
            Binder.CreateButtonEvent<ClickBinderEvent>(mapButton, OnClickedMapButton);
            Binder.CreateButtonEvent<ClickBinderEvent>(consoleButton, OnClickedConsoleButton);
        }

        private void OnClickedContractsButton()
        {
            _viewsService.Open(ViewIds.CONTRACTS);
        }

        private void OnClickedMapButton()
        {
            _viewsService.Open(ViewIds.MAP);
        }

        private void OnClickedConsoleButton()
        {
            _viewsService.Open(ViewIds.CONSOLE);
        }
    }
}

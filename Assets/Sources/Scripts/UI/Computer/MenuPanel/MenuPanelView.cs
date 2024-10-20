using UnityEngine;

namespace Game.MVVM.Computer
{
    public class MenuPanelView : View
    {
        [SerializeField] private Button _contractsButton;
        [SerializeField] private Button _mapButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _consoleButton;

        private MenuPanelViewModel _viewModel;

        public override void Init(ViewModelFactory viewModelFactory)
        {
            Id = ViewIds.MENU_PANEL;
            IsActivedOnStart = true;

            _viewModel = viewModelFactory.Create<MenuPanelViewModel>();

            _viewModel.Init(_contractsButton, _mapButton, _shopButton, _consoleButton);
        }
    }
}

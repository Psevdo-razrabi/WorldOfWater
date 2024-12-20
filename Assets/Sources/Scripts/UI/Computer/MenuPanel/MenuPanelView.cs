using UnityEngine;

namespace Game.MVVM.Computer
{
    public class MenuPanelView : View<MenuPanelViewModel>
    {
        [SerializeField] private Button _contractsButton;
        [SerializeField] private Button _mapButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _consoleButton;

        public override void Open()
        {
            ViewModel.Init(_contractsButton, _mapButton, _shopButton, _consoleButton);

            SubscribeUpdateView(UpdateView);
        }

        private void UpdateView()
        {
            Debug.Log("Update");
        }
    }
}

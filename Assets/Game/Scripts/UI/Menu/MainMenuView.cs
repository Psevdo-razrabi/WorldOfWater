using UnityEngine;
using UnityEngine.Serialization;

namespace Game.MVVM.Menu
{
    public class MainMenuView : View<MainMenuViewModel>
    {
        [SerializeField] private Button _singleplayerButton;
        [SerializeField] private Button _multiplayerButton;

        public override ViewId Id => ViewId.MainMenu;

        public override void Initialize()
        {
            ViewModel.Init(_singleplayerButton, _multiplayerButton);
        }
    }
}

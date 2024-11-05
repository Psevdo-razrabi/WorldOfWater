using UnityEngine;
using UnityEngine.Serialization;

namespace Game.MVVM.Menu
{
    public class MainMenuView : View<MainMenuViewModel>
    {
        [SerializeField] private Button _singleplayerButton;
        [SerializeField] private Button _multiplayerButton;


        public override void Open()
        {
            ViewModel.Init(_singleplayerButton, _multiplayerButton);
        }
    }
}

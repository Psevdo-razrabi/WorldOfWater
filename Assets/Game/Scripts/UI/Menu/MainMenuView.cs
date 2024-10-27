using UnityEngine;

namespace Game.MVVM.Menu
{
    public class MainMenuView : View<MainMenuViewModel>
    {
        [SerializeField] private Button _playButton;

        public override ViewId Id => ViewId.MainMenu;

        public override void Init()
        {
            ViewModel.Init(_playButton);
        }
    }
}

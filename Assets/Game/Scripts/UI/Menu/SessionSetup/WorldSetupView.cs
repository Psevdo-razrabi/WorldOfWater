using UnityEngine;
using VContainer;

namespace Game.MVVM.Menu
{
    public class WorldSetupView : View<WorldSetupViewModel>
    {
        [SerializeField] private Button _createWorldButton;
        [SerializeField] private Button _loadWorldButton;
        [SerializeField] private Button _joinWorldButton;

        public override ViewId Id => ViewId.WorldSetup;

        public override void Init()
        {
            ViewModel.Init(_createWorldButton, _loadWorldButton, _joinWorldButton);
        }
    }
}

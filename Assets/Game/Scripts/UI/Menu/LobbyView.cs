using TMPro;
using UnityEngine;

namespace Game.MVVM.Menu
{
    public class LobbyView : View<LobbyViewModel>
    {
        [SerializeField] private Button _leaveButton;
        [SerializeField] private TMP_InputField _codeField;
        
        public override ViewId Id => ViewId.Lobby;
        public override void Initialize()
        {
            ViewModel.Init(_leaveButton);

            _codeField.text = ViewModel.Code;
        }
    }
}
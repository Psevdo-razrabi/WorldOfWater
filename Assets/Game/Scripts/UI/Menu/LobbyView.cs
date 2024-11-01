using TMPro;
using UnityEngine;

namespace Game.MVVM.Menu
{
    public class LobbyView : View<LobbyViewModel>
    {
        [SerializeField] private Button _leaveButton;
        [SerializeField] private TMP_InputField _codeField;
        
        public override void Open()
        {
            ViewModel.Init(_leaveButton);

            _codeField.text = ViewModel.Code;
        }
    }
}
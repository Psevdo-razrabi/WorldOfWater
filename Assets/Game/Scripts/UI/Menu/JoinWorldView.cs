using TMPro;
using UnityEngine;

namespace Game.MVVM.Menu
{
    public class JoinWorldView : View<JoinWorldViewModel>
    {
        [SerializeField] private Button _joinButton;
        [SerializeField] private TMP_InputField _codeInputField;
        
        public override ViewId Id => ViewId.JoinWorld;
        public override void Initialize()
        {
            ViewModel.Init(_joinButton);

            _codeInputField.onValueChanged.AddListener(n => ViewModel.Code = n);
            _codeInputField.text = "Empty";
            ViewModel.Code = _codeInputField.text;
        }
    }
}
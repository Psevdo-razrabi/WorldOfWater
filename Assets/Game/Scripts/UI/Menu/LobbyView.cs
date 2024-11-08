using R3;
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

            Binder.ViewTriggered.Subscribe(UpdateView).AddTo(Binder.Disposable);
        }

        private void UpdateView()
        {
            _codeField.text = ViewModel.JoinCode;
        }
    }
}
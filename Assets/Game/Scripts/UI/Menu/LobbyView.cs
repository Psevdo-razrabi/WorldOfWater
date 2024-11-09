using Game.Services;
using R3;
using TMPro;
using UnityEngine;
using VContainer;

namespace Game.MVVM.Menu
{
    public class LobbyView : View<LobbyViewModel>
    {
        [SerializeField] private Button _leaveButton;
        [SerializeField] private TMP_InputField _codeField;

        private LobbiesService _lobbiesService;

        [Inject]
        public void Construct(LobbiesService lobbiesService)
        {
            _lobbiesService = lobbiesService;
        }

        public override void Open()
        {
            ViewModel.Init(_leaveButton);

            _lobbiesService.Connected.Subscribe(UpdateView).AddTo(Binder.Disposable);
        }

        private void UpdateView()
        {
            _codeField.text = _lobbiesService.JoinCode;
        }
    }
}
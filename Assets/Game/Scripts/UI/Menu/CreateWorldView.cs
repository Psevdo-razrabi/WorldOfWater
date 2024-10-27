using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.MVVM.Menu
{
    public class CreateWorldView : View<CreateWorldViewModel>
    {
        [SerializeField] private Button _createButton;

        [SerializeField] private TMP_InputField _worldNameInputField;
        [SerializeField] private Toggle _isOnlineToggle;

        public override ViewId Id => ViewId.CreateWorld;

        public override void Init()
        {
            ViewModel.Init(_createButton);
            SubscribeUpdateView(UpdateView);

            _worldNameInputField.onValueChanged.AddListener(n => ViewModel.WorldName = n);
        }

        private void UpdateView()
        {

        }
    }
}

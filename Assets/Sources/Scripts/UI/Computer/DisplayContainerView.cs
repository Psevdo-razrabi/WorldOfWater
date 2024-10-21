using System.Collections.Generic;
using UnityEngine;

namespace Game.MVVM
{
    public class DisplayContainerView : View
    {
        [SerializeField] private Button _button;

        private DisplayContainerViewModel _viewModel;
        private List<Binder> _binders = new();

        public override void Init(ViewModelFactory viewModelFactory)
        {
            _viewModel = viewModelFactory.Create<DisplayContainerViewModel>();

            _viewModel.Init(_button);

            _viewModel.SubscribeUpdateView(UpdateView);
        }

        private void UpdateView()
        {
            Debug.Log("View");
        }

        private void OnDestroy()
        {
            foreach (var binder in _binders)
            {
                binder.Dispose();
            }
            _viewModel.Binder.Dispose();
            _binders.Clear();
        }
    }
}
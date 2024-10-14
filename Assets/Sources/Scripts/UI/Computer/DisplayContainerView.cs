using System.Collections.Generic;
using UniRx;
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

            _viewModel.Binder.ViewTriggered.Subscribe(UpdateView).AddTo(_viewModel.Binder.Disposable);
        }

        private void UpdateView()
        {
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
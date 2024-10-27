using System.Collections.Generic;
using UnityEngine;

namespace Game.MVVM
{
    public class DisplayContainerView : View<DisplayContainerViewModel>
    {
        [SerializeField] private Button _button;

        private List<Binder> _binders = new();

        public override void Initialize()
        {
            ViewModel.Init(_button);

            SubscribeUpdateView(UpdateView);
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
            ViewModel.Binder.Dispose();
            _binders.Clear();
        }

        public override ViewId Id => ViewId.Display;
    }
}
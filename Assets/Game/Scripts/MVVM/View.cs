using R3;
using System;
using Unity.Netcode;
using UnityEngine;
using VContainer;

namespace Game.MVVM
{
    public abstract class View<T> : View where T : ViewModel, new()
    {
        protected T ViewModel; 
        protected Binder Binder;

        [Inject]
        public void Construct(ViewModelFactory viewModelFactory)
        {
            ViewModel = viewModelFactory.Create<T>();
            Binder = ViewModel.Binder;
        }

        protected void SubscribeUpdateView(Action action)
        {
            Binder.ViewTriggered.Subscribe(action).AddTo(Binder.Disposable);
        }

        public override void Close()
        {
            Binder.Dispose();
        }
    }

    public abstract class View : NetworkBehaviour
    {
        public virtual void Open() {}
        public virtual void Close() {}
    }
}
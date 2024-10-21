using System;
using R3;

namespace Game.MVVM
{
    public abstract class ViewModel
    {
        public Binder Binder { get; set; } = new();

        public void SubscribeUpdateView(Action action)
        {
            Binder.ViewTriggered.Subscribe(action).AddTo(Binder.Disposable);
        }
    }
}
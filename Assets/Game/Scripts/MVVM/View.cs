﻿using Game.MVVM;
using R3;
using System;
using UnityEngine;
using VContainer;

namespace Game.MVVM
{
    public abstract class View<T> : View where T : ViewModel, new()
    {
        protected T ViewModel; 
        protected Binder Binder;

        [Inject]
        private void Construct(ViewModelFactory viewModelFactory)
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

    public abstract class View : MonoBehaviour
    {
        public abstract void Open();
        public virtual void Close() {}
    }
}
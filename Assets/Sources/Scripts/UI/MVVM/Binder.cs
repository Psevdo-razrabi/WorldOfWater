using R3;
using System;
using System.Collections.Generic;

namespace Game.MVVM
{
    public class Binder : IDisposable
    {
        private readonly Dictionary<Type, List<Binding>> _bindings = new();
        public ReactiveCommand ViewTriggered { get; } = new();
        public CompositeDisposable Disposable { get; } = new();

        public void CreateButtonEvent<T>(IBindable bindable, Action action) where T : BinderEvent
        {
            if (_bindings.TryGetValue(typeof(T), out var bindings))
            {
                bindings.Add(new Binding(bindable, action));
            }
            else
            {
                _bindings.Add(typeof(T), new List<Binding> { new(bindable, action) });
            }
            
            bindable.Bind(this);
        }

        public void TriggerButtonEvent<T>(IBindable bindable) where T : BinderEvent
        {
            if (_bindings.TryGetValue(typeof(T), out var bindings))
            {
                for(int i = 0; i < bindings.Count; i++)
                {
                    if (bindings[i].Bindable == bindable)
                    {
                        bindings[i].Action?.Invoke();
                    }
                }
            }
        }

        public void TriggerView()
        {
            ViewTriggered.Execute();
        }

        public void Dispose()
        {
            _bindings.Clear();
            Disposable.Dispose();
        }
    }
}

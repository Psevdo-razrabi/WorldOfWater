using System;
using System.Collections.Generic;
using UniRx;

namespace Game.MVVM
{
    public class Binder : IDisposable
    {
        private readonly Dictionary<Type, List<Binding>> _bindings = new();

        public ReactiveCommand ViewTriggered { get; } = new();
        public CompositeDisposable Disposable { get; } = new();

        public void CreateButtonEvent<T>(IBindable bindable, Action action) where T : BinderEvent
        {
            if (_bindings.TryGetValue(typeof(T), out var actions))
            {
                actions.Add(new Binding(bindable, action));
            }
            else
            {
                _bindings.Add(typeof(T), new List<Binding> {new(bindable, action)});
            }
            
            bindable.Bind(this);
        }

        public void TriggerButtonEvent<T>(IBindable bindable) where T : BinderEvent
        {
            if(_bindings.TryGetValue(typeof(T), out var bindings))
            {
                foreach (var binding in bindings)
                {
                    if (binding.Bindable == bindable)
                    {
                        binding.Action?.Invoke();
                    }
                }
            }
        }

        public void Dispose()
        {
            _bindings.Clear();
            Disposable.Dispose();
        }
    }
}

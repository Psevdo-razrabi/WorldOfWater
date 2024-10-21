using System;

namespace Game.MVVM
{
    public struct Binding
    {
        public IBindable Bindable { get; }
        public Action Action { get; }

        public Binding(IBindable bindable, Action action)
        {
            Bindable = bindable;
            Action = action;
        }
    }
}
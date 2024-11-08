using System;
using System.Collections.Generic;
using VContainer;

namespace Game.MVVM
{
    public class ViewModelFactory
    {
        private readonly IObjectResolver _container;
        private readonly Dictionary<Type, ViewModel> _viewModels = new();

        public ViewModelFactory(IObjectResolver container)
        {
            _container = container;
        }

        public T Create<T>() where T : ViewModel, new()
        {
            if (_viewModels.ContainsKey(typeof(T)))
            {
                return _viewModels[typeof(T)] as T;
            }
            
            var viewModel = new T();
            _container.Inject(viewModel);
            _viewModels.Add(typeof(T), viewModel);
            return viewModel;
        }
    }
}

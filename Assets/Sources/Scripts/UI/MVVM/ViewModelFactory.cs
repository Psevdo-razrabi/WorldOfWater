using System;
using System.Collections.Generic;
using Zenject;

namespace Game.MVVM
{
    public class ViewModelFactory
    {
        private readonly DiContainer _container;
        private readonly Dictionary<Type, ViewModel> _viewModels = new();

        public ViewModelFactory(DiContainer container)
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

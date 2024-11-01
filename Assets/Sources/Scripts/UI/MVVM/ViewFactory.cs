using System.Linq;
using UnityEngine;
using VContainer;

namespace Game.MVVM
{
    public class ViewFactory
    {
        private readonly IObjectResolver _container;
        private readonly ViewsConfig _viewsConfig;
        private Transform _parent;

        public ViewFactory(IObjectResolver container, ViewsConfig viewsConfig)
        {
            _container = container;
            _viewsConfig = viewsConfig;
        }

        public void Initialize()
        {
            _parent = Object.Instantiate(_viewsConfig.Canvas, null).transform;
        }

        public View Create<T>() where T : View
        {
            var prefab = _viewsConfig.Views.First(v => v is T);
            var view = Object.Instantiate(prefab, _parent);
            _container.Inject(view);
            return view;
        }
    }
}

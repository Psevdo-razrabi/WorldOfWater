using System;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Game.MVVM
{
    public class ViewFactory : IInitializable
    {
        private readonly IObjectResolver _container;
        private readonly ViewsConfig _viewsConfig;
        private readonly Dictionary<ViewId, View> _views = new();
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

        public async UniTask<View> Create(ViewId viewId)
        {
            if (_views.TryGetValue(viewId, out var view))
            {
                Debug.LogException(new Exception("View created again!"));
            }

            var newView = await _viewsConfig.LoadView(viewId, _parent);
            _container.Inject(newView);
            _views.Add(viewId, newView);
            return newView;
        }
    }
}

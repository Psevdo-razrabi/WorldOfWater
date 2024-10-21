using System;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Loader
{
    public class LoaderFromResources : IDisposable
    {
        private CompositeDisposable _compositeDisposable = new();
        
        public void LoadResources<T>(string path, Action<Object> action = null) where T : Object
        {
            Resources.LoadAsync<T>(path).AsAsyncOperationObservable().Subscribe(resources => OperationResources(resources, action)).AddTo(_compositeDisposable);
        }

        private void OperationResources(ResourceRequest resources, Action<Object> action = null)
        {
            if (resources.asset != null)
            {
                action?.Invoke(resources.asset);
            }
        }

        public void Dispose()
        {
            _compositeDisposable?.Clear();
            _compositeDisposable?.Dispose();
        }
    }
}
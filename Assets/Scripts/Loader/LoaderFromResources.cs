using System;
using UnityEngine;
using Object = UnityEngine.Object;
using UniTask = Cysharp.Threading.Tasks.UniTask;

namespace Loader
{
    public class LoaderFromResources
    {
        public async UniTask LoadResources(string path, Type type, Action<Object> action = null)
        {
            var resources = Resources.LoadAsync(path, type);
            await resources;
            OperationResources(resources, action);
        }
        
        private void OperationResources(ResourceRequest resources, Action<Object> action = null)
        {
            if (resources.asset == null) throw new Exception();
            
            action?.Invoke(resources.asset);
        }
    }
}
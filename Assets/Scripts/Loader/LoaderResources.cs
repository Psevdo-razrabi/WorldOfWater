using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Loader
{
    public class LoaderResources : MonoBehaviour
    {
        private readonly LoaderFromAddressables _loaderFromAddressables = new();
        private readonly LoaderFromResources _loaderFromResources = new();

        public static void Initialize() => Addressables.InitializeAsync();

        public async UniTask LoadAssetFromResources(string path, Type type, Action<Object> action)
        {
            await _loaderFromResources.LoadResources(path, type, action);
        }

        public async UniTask LoadAllAssetWithLabel<T>(AssetLabelReference labelReference, Action<UploadedResourcesList<T>> action) where T : Object
        {
            var task = _loaderFromAddressables.LoadAllResourcesUseLabel<T>(labelReference);
            var resources = await task;
            action.Invoke(resources);
        }
        
        public async UniTask LoadAllAssetWithLabel<T>(AssetReferenceT<T> labelReference, Action<UploadedResources<T>> action) where T : Object
        {
            var task = _loaderFromAddressables.LoadResourcesUsingReference(labelReference);
            var resources = await task;
            action.Invoke(resources);
        }
    }
}
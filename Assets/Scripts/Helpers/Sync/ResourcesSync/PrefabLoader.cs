using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Loader;
using R3;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Sync
{
    public class PrefabLoader : ILoader
    {
        public IReadOnlyList<Func<UniTask>> Loaders => _list;
        public ReactiveProperty<bool> IsLoaded { get; } = new();
        private LoaderResources _loaderResources;
        private List<Func<UniTask>> _list = new();

        public PrefabLoader(LoaderResources loaderResources)
        {
            _loaderResources = loaderResources;
        }

        public void SetProperties(string key, string path)
        {
            _list.Add(() => LoadFromResources(path, key));
        }

        public async UniTask LoadFromResources(string path, string key)
        {
            await _loaderResources.LoadAssetFromResources(path, typeof(GameObject), (obj) => ResourceManager.Instance.SaveResources(key, obj));
            await UniTask.Yield();
        }

        public async UniTask LoadFromAddressablesWithLabel(AssetLabelReference labelReference, string key)
        {
            await _loaderResources.LoadAllAssetWithLabel<GameObject>(labelReference, 
                (resources) => ResourceManager.Instance.SaveResources(key, resources));
        }

        public async UniTask LoadFromAddressablesWithReference<T>(AssetReferenceT<T> labelReference, string key) where T : Object
        {
            await UniTask.Yield();
        }
    }
}
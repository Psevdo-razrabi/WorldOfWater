using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Loader;
using R3;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Sync
{
    public class ConfigLoader : ILoader
    {
        public ReactiveProperty<bool> IsLoaded { get; } = new();
        public IReadOnlyList<Func<UniTask>> Loaders => _list;
        private LoaderResources _loaderResources;
        private readonly List<Func<UniTask>> _list = new();

        public ConfigLoader(LoaderResources loaderResources)
        {
            _loaderResources = loaderResources;
        }
        
        public void SetPropertiesForLoadToResources(string key, string path)
        {
            _list.Add(() => LoadFromResources(path, key));
        }        
        
        public void SetPropertiesForLoadToAddressables(string key, AssetLabelReference reference)
        {
            _list.Add(() => LoadFromAddressablesWithLabel(reference, key));
        }  

        public async UniTask LoadFromResources(string path, string key)
        {
            await _loaderResources.LoadAssetFromResources(path, typeof(ScriptableObject), (obj) => ResourceManager.Instance.SaveResources(key, obj));
        }

        public async UniTask LoadFromAddressablesWithLabel(AssetLabelReference labelReference, string key)
        {
            await _loaderResources.LoadAllAssetWithLabel<ScriptableObject>(labelReference, 
                (resources) => ResourceManager.Instance.SaveResources(key, resources));
        }
    }
}
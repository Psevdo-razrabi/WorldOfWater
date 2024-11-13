using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Helpers;
using R3;
using UnityEngine;

namespace Sync
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance { get; private set; }
        public Observable<bool> OnAllLoaded => _onAllLoaded;
        private Dictionary<TypeSync, ILoader> _loaders = new();
        private readonly Subject<bool> _onAllLoaded = new();
        private IDisposable _disposable;
        private readonly Dictionary<ResourcesKey, object> _resources = new();
        private readonly Dictionary<string, ResourcesKey> _keyRegistry = new();
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
                _disposable.Dispose();
            }
        }

        public bool SaveResources(string key, object resources)
        {
            Preconditions.CheckNotNull(resources);

            return _resources.TryAdd(GetOrRegisterKey(key), resources);
        }
        
        public ResourcesKey GetOrRegisterKey(string keyName)
        {
            Preconditions.CheckNotNull(keyName);
    
            if (_keyRegistry.TryGetValue(keyName, out ResourcesKey key) == false)
            {
                key = new ResourcesKey(keyName);
                _keyRegistry[keyName] = key;
            }
    
            return key;
        }
        
        public T GetResources<T>(ResourcesKey key)
        {
            if (_resources.TryGetValue(key, out var resource))
            {
                return (T)resource;
            }

            throw new KeyNotFoundException();
        }
        
        public T GetResources<T, K>(string name) where T : class
        {
            var key = GetOrRegisterKey(name);
            return GetResources<K>(key) as T;
        }

        public void RegisterLoader(TypeSync key, ILoader value)
        {
            if (_loaders.ContainsKey(key) == false)
            {
                Preconditions.CheckNotNull(value);
                _loaders.Add(key, value);
            }
        }

        public async void LoadAll()
        {
            var loadTask = _loaders.Values.SelectMany(loader => loader.Loaders);
            await UniTask.WhenAll(loadTask.Select(func => func.Invoke()));

            foreach (var loaders in _loaders.Values)
            {
                loaders.IsLoaded.Value = true;
            }

            foreach (var type in _loaders.Keys)
            {
                ProjectActions.OnTypeLoad.OnNext(type);
            }
        }

        public void MergeProperties()
        {
            List<ReactiveProperty<bool>> isLoader = new List<ReactiveProperty<bool>>();
            _loaders.Values.ToList().ForEach(x => isLoader.Add(x.IsLoaded));
            _disposable = isLoader.Merge()
                .Skip(1)
                .Subscribe(_ =>
                {
                    var loaded = IsAllLoaded();
                    _onAllLoaded.OnNext(loaded);
                });
        }

        private bool IsAllLoaded()
        {
            return _loaders.Values.All(loader => loader.IsLoaded.Value);
        }
    }
}
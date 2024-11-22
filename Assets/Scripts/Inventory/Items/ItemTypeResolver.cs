using System.Collections.Generic;
using Loader;
using Sync;
using UnityEngine;
using Zenject;

namespace Inventory
{
    public class ItemTypeResolver : IInitializable
    {
        public IReadOnlyDictionary<EItemType, ItemDescription> ItemConfig => _itemConfigs;
        private readonly Dictionary<EItemType, ItemDescription> _itemConfigs = new();
        
        public ItemDescription FindItemAfterType(EItemType type)
        {
            if (_itemConfigs.TryGetValue(type, out var value))
            {
                return value;
            }

            throw new KeyNotFoundException();
        }
        
        public void Initialize()
        {
            LoadResources();
        }

        private void LoadResources()
        {
            var listItem = ResourceManager.Instance.GetResources<UploadedResourcesList<ScriptableObject>>(
                ResourceManager.Instance.GetOrRegisterKey(ResourcesName.ItemsConfigs)).resources;
            
            FillingDictionary(listItem);
        }

        private void FillingDictionary(List<ScriptableObject> items)
        {
            foreach (var scriptable in items)
            {
                var itemSO = scriptable as ItemDescription;

                if (_itemConfigs.TryAdd(itemSO.ItemType, itemSO) == false)
                {
                    Debug.LogError($"Cant add item {itemSO.ItemType} to dictionary");
                }
            }
        }
    }
}
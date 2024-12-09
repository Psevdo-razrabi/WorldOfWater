using Factory;
using Helpers.PoolObject;
using Loader;
using Sync;
using UnityEngine;
using Zenject;

namespace Inventory
{
    public class ObjectPoolInventory : IInitializable
    {
        private FactoryComponentWithMonoBehaviour _factory;
        private ItemsPrefabs _itemsPrefabs;

        public ObjectPoolInventory(FactoryComponentWithMonoBehaviour factory)
        {
            _factory = factory;
        }

        public (GameObject, ItemType) GetObject(string key)
        {
            return _factory.CreateWithPoolObject<ItemType>(key);
        }

        public void Initialize()
        {
            LoadResources();
            CreatePoolObject(PoolKeyName.ClothPrefab, _itemsPrefabs.ClothPrefab, 5);
            CreatePoolObject(PoolKeyName.MetalPrefab, _itemsPrefabs.MetalPrefab, 5);
            CreatePoolObject(PoolKeyName.PlasticPrefab, _itemsPrefabs.PlasticPrefab, 5);
            CreatePoolObject(PoolKeyName.WoodPrefab, _itemsPrefabs.WoodPrefab, 5);
        }

        private void CreatePoolObject(string namePrefab, GameObject prefab, int count)
        {
            _factory.UpdateProperties(true, namePrefab, count);
            _factory.CreatePool<ItemType>(prefab);
        }

        private void LoadResources()
        {
            _itemsPrefabs = ResourceManager.Instance.GetResources<UploadedResources<ScriptableObject>>(
                ResourceManager.Instance.GetOrRegisterKey(ResourcesName.ItemsPrefabConfigs)).resources as ItemsPrefabs;
        }
    }
}
using Cysharp.Threading.Tasks;
using Loader;
using UnityEngine;
using Zenject;

namespace Sync
{
    public class ResourcesBootstrap : MonoBehaviour
    {
        [SerializeField] private AddressablesReferences _addressablesReferences;
        private LoaderResources _loaderResources;
        private ConfigLoader _configLoader;
        private PrefabLoader _prefabLoader;
        
        [Inject]
        private void Construct(LoaderResources loaderResources)
        {
            _loaderResources = loaderResources;
        }

        private void CreateLoaders()
        {
            _configLoader = new ConfigLoader(_loaderResources);
            _prefabLoader = new PrefabLoader(_loaderResources);
        }
        
        private async void Start()
        {
            CreateLoaders();
            
            ResourceManager.Instance.RegisterLoader(TypeSync.Config, _configLoader);
            ResourceManager.Instance.RegisterLoader(TypeSync.Prefab, _prefabLoader);
            _prefabLoader.SetProperties(ResourcesName.Slot, "Slot");
            _prefabLoader.SetProperties(ResourcesName.Icon, "GhostIcon");
            _configLoader.SetPropertiesForLoadToResources(ResourcesName.PlayerHelpers, "PlayerHelpers");
            _configLoader.SetPropertiesForLoadToResources(ResourcesName.PlayerController, "Controller");
            _configLoader.SetPropertiesForLoadToAddressablesWithLabel(ResourcesName.ItemsConfigs, _addressablesReferences.ItemsReferences);
            _configLoader.SetPropertiesForLoadToAddressablesWithReference(ResourcesName.ItemsPrefabConfigs, _addressablesReferences.ItemsPrefabs);

            await UniTask.WaitForSeconds(0.5f);
            ResourceManager.Instance.MergeProperties();
            ResourceManager.Instance.LoadAll();
        }
    }
}
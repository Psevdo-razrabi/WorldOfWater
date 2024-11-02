using Helpers.Sync;
using Loader;
using SceneManagment;
using Sync;
using UnityEngine;

namespace DI
{
    public class ProjectInject : BaseBindings
    {
        [SerializeField] private LoaderResources _loaderResources;
        [SerializeField] private ResourceManager _resourceManager;
        [SerializeField] private ResourcesBootstrap _resourcesBootstrap;
        [SerializeField] private SyncManager _syncManager;
        [SerializeField] private SceneLoader _sceneLoader;
        
        public override void InstallBindings()
        {
            BindLoader();
        }

        private void BindLoader()
        {
            BindNewInstance<SceneResources>();
            BindNewInstance<SceneManager>();
            BindInstance(_sceneLoader);
            BindNewInstance<LoaderPresenters>();
            BindInstance(_loaderResources);
            BindInstance(_resourceManager);
            BindInstance(_resourcesBootstrap);
            BindInstance(_syncManager);
            LoaderResources.Initialize();
        }
    }
}
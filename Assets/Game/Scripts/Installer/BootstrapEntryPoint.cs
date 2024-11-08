using Game.Services;
using Unity.Netcode;
using UnityEngine;
using VContainer;

namespace Game.DI
{
    public class BootstrapEntryPoint : MonoBehaviour
    {
        [SerializeField] private BootstrapLifetimeScope _lifetimeScope;
        
        private async void Awake()
        {
            DontDestroyOnLoad(_lifetimeScope);
            var scenesService = _lifetimeScope.Container.Resolve<ScenesService>();
            //scenesService.LoadSceneAsync(SceneType.MainMenu);
            await scenesService.LoadScene(SceneType.MainMenu);

            var networkManager = Resources.LoadAsync("NetworkManager");
            Instantiate(networkManager.asset, null);
        }
    }
}
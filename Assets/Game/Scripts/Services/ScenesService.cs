using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Game.Services
{
    public class ScenesService
    {
        private AsyncOperationHandle<SceneInstance> _previousScene;

        public void LoadSceneNetwork(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            var status = NetworkManager.Singleton.SceneManager.LoadScene(sceneName, loadSceneMode);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load {sceneName} " +
                                 $"with a {nameof(SceneEventProgressStatus)}: {status}");
            }
        }
        
        public async UniTask LoadSceneAsync(string scenePath, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            var scene = Addressables.LoadSceneAsync(scenePath, loadSceneMode);
            await UniTask.WaitUntil(() => scene.IsDone);
        }

        public async UniTask  LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            await SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        }
    }
}
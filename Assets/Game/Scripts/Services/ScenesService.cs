using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Services
{
    public class ScenesService
    {
        public ScenesService()
        {
        }

        public async void LoadScene()
        {
            await UniTask.WaitUntil(() => NetworkManager.Singleton.SceneManager != null);
            
            var sceneByName = "Gameplay";
            if (!string.IsNullOrEmpty(sceneByName))
            {
                var status = NetworkManager.Singleton.SceneManager.LoadScene(sceneByName, LoadSceneMode.Additive);
                if (status != SceneEventProgressStatus.Started)
                {
                    Debug.LogWarning($"Failed to load {sceneByName} " +
                                     $"with a {nameof(SceneEventProgressStatus)}: {status}");
                }
                else
                {
                    Debug.Log("game loaded");
                }
            }
        }
    }
}
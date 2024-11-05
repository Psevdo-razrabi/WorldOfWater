using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectSceneManager : NetworkBehaviour
{
    /*public override void OnNetworkSpawn()
    {
        var sceneByName = SceneManager.GetSceneByName("Game");
        if (!string.IsNullOrEmpty(sceneByName.name))
        {
            var status = NetworkManager.Singleton.SceneManager.LoadScene(sceneByName.name, LoadSceneMode.Additive);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load {sceneByName.name} " +
                                 $"with a {nameof(SceneEventProgressStatus)}: {status}");
            }
        }
    }*/
}

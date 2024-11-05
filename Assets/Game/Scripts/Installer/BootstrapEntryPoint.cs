using Game.Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Game.DI
{
    public class BootstrapEntryPoint : MonoBehaviour
    {
        [SerializeField] private BootstrapLifetimeScope _lifetimeScope;
        
        private void Awake()
        {
            DontDestroyOnLoad(_lifetimeScope);
            SceneManager.LoadScene("MainMenu");
            
            Instantiate(Resources.Load("NetworkManager"), null);
        }
    }
}
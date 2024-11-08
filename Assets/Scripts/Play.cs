using Loader;
using SceneManagment;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Play : MonoBehaviour
{
    private Button _button;
    private SceneLoader _sceneLoader;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(LoadScene);
    }

    [Inject]
    private void Construct(SceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }
    
    private async void LoadScene()
    {
        await _sceneLoader.LoadScene(TypeScene.Game);
    }
}

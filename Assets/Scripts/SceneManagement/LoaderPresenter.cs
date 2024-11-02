using System;

namespace SceneManagment
{
    public class LoaderPresenters : IDisposable
    {
        private SceneLoader _sceneLoader;
        private SceneManager _sceneManager;

        public LoaderPresenters(SceneManager sceneManager, SceneLoader sceneLoader)
        {
            _sceneManager = sceneManager;
            _sceneLoader = sceneLoader;
            
            Initialize();
        }

        public void Dispose()
        {
            _sceneManager.OnLoadScene -= _sceneLoader.Animation;
            _sceneManager.OnFadeOut -= _sceneLoader.FadeOut;
        }

        public void Initialize()
        {
            _sceneManager.OnLoadScene += _sceneLoader.Animation;
            _sceneManager.OnFadeOut += _sceneLoader.FadeOut;
        }
    }
}
using System;
using Eflatun.SceneReference;
using Loader;

namespace SceneManagment
{
    [Serializable]
    public struct SceneData
    {
        public SceneReference scene;
        public TypeScene typeScene;
        public string Name => scene.Name;
    }
}
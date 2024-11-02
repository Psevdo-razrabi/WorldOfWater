using System;

namespace SceneManagment
{
    public class LoadingProgress : IProgress<float>
    {
        public event Action<float> Progressed;
        private float ratio = 1f;
        
        public void Report(float value)
        {
            Progressed?.Invoke(value / ratio);
        }
    }
}
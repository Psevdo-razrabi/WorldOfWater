namespace Helpers.Shader
{
    public static class ShaderHelper
    {
        public static void SetGlobalFloat(int id, float value) 
            => UnityEngine.Shader.SetGlobalFloat(id, value);
    }

    public static class ShaderParameterId
    {
        public static readonly int BlurXId = UnityEngine.Shader.PropertyToID("_BlurX");
        public static readonly int BlurYId = UnityEngine.Shader.PropertyToID("_BlurY");
    }
}
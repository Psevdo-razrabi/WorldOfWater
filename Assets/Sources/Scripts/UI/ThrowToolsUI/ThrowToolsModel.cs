using Sources.Scripts.Configs;
using Sources.Scripts.Configs.Interfaces;

namespace Sources.Scripts.UI.ThrowToolsUI.Hook
{
    public abstract class ThrowToolsModel
    {
        public abstract void HookThrowed(float circleFillValue);
        public abstract void LoadConfig(IThrowToolConfig config);
        public abstract bool IsThrowed { get; protected set; }
        public abstract IThrowToolConfig ToolConfig { get; protected set; }
    }
}
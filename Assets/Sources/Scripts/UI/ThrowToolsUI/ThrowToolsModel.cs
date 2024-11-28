using Sources.Scripts.Configs;
using Sources.Scripts.Configs.Interfaces;
using UniRx;

namespace Sources.Scripts.UI.ThrowToolsUI.Hook
{
    public abstract class ThrowToolsModel : ICircleFillState
    {
        public abstract void ToolThrowed(float circleFillValue);
        public abstract void LoadConfig(IThrowToolConfig config);
        public abstract void SwitchToolThrowState(bool isThrow);

        public abstract bool IsThrowed { get; protected set; }
        public abstract IThrowToolConfig ToolConfig { get; protected set; }
        
        private ReactiveProperty<float> _circleFillValue = new();
        public IReactiveProperty<float> CircleFillValue => _circleFillValue;
    }
}
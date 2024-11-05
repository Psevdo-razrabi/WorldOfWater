using Sources.Scripts.Configs;
using Sources.Scripts.Configs.Interfaces;
using Sources.Scripts.UI.ThrowToolsUI.Hook;
using UniRx;

public class HookModel : ThrowToolsModel
{
    private ReactiveProperty<float> _circleFillValue = new();
    public IReactiveProperty<float> CircleFillValue => _circleFillValue;
    
    public HookConfig HookConfig { get; private set; }
    public override bool IsThrowed { get; protected set; }
    public override IThrowToolConfig ToolConfig { get; protected set; }

    public override void LoadConfig(IThrowToolConfig toolConfig)
    {
        if (toolConfig is HookConfig hookConfig)
        {
            HookConfig = hookConfig;
            ToolConfig = hookConfig;
        }
    }

    public override void HookThrowed(float circleFillValue)
    {
        SwitchHookThrowState(true);
        _circleFillValue.Value = circleFillValue;
    }

    public void SwitchHookThrowState(bool isThrow)
    {
        IsThrowed = isThrow;
    }
}

using Sources.Scripts.Configs;
using Sources.Scripts.Configs.Interfaces;
using Sources.Scripts.UI.ThrowToolsUI.Hook;
using UniRx;

public class HookModel : ThrowToolsModel
{
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

    public override void ToolThrowed(float circleFillValue)
    {
        SwitchToolThrowState(true);
        CircleFillValue.Value = circleFillValue;
    }
    public override void SwitchToolThrowState(bool isThrow)
    {
        IsThrowed = isThrow;
    }
}

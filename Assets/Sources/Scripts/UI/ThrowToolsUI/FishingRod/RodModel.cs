using Sources.Scripts.Configs;
using Sources.Scripts.Configs.Interfaces;
using Sources.Scripts.UI.ThrowToolsUI.Hook;

namespace Sources.Scripts.UI.ThrowToolsUI.FishingRod
{
    public class RodModel : ThrowToolsModel
    {
        public RodConfig RodConfig { get; private set; }
        public override bool IsThrowed { get; protected set; }
        public override IThrowToolConfig ToolConfig { get; protected set; }
        public override void ToolThrowed(float circleFillValue)
        {
            SwitchToolThrowState(true);
            CircleFillValue.Value = circleFillValue;
        }

        public override void LoadConfig(IThrowToolConfig config)
        {
            if (config is RodConfig hookConfig)
            {
                RodConfig = hookConfig;
                ToolConfig = hookConfig;
            }
        }

        public override void SwitchToolThrowState(bool isThrow)
        {
            IsThrowed = isThrow;
        }


    }
}
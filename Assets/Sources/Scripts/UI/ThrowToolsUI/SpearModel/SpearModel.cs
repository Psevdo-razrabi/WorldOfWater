using Sources.Scripts.Configs;
using Sources.Scripts.Configs.Interfaces;
using Sources.Scripts.UI.ThrowToolsUI.Hook;

namespace Sources.Scripts.UI.ThrowToolsUI.SpearModel
{
    public class SpearModel : ThrowToolsModel
    {
        public SpearConfig SpearConfig { get; private set; }
        public override void ToolThrowed(float circleFillValue)
        {
            SwitchToolThrowState(true);
            CircleFillValue.Value = circleFillValue;
        }

        public override void LoadConfig(IThrowToolConfig config)
        {
            if (config is SpearConfig hookConfig)
            {
                SpearConfig = hookConfig;
                ToolConfig = hookConfig;
            }
        }

        public override void SwitchToolThrowState(bool isThrow)
        {
            IsThrowed = isThrow;
        }

        public override bool IsThrowed { get; protected set; }
        public override IThrowToolConfig ToolConfig { get; protected set; }
    }
}
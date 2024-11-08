using Helpers;
using NewInput;
using Sync;
using UnityEngine;
using Zenject;

namespace Data
{
    public class PlayerData : ILateTickable
    {
        public PlayerHelpersConfig PlayerHelpersConfig { get; private set; }
        public PlayerControllerConfig PlayerControllerConfig { get; private set; }
        public Player Player { get; private set; }
        public RaycastHelper RaycastHelper { get; private set; }
        public PlayerGroundHelper PlayerGroundHelper { get; private set; }
        public PlayerInputReader PlayerInputReader { get; private set; }
        
        [Inject]
        private void Construct(Player player)
        {
            InitResources();
            
            Player = player;
            Player.Init();
            RaycastHelper = new RaycastHelper(player.transform, PlayerHelpersConfig.Raycast);
            PlayerGroundHelper = new PlayerGroundHelper(Player, PlayerHelpersConfig.GroundHelper, RaycastHelper);
            PlayerInputReader = new PlayerInputReader();
        }

        private void InitResources()
        {
            PlayerHelpersConfig =
                ResourceManager.Instance.GetResources<PlayerHelpersConfig, ScriptableObject>(
                    ResourcesName.PlayerHelpers);
            PlayerControllerConfig = ResourceManager.Instance.GetResources<PlayerControllerConfig, ScriptableObject>(
                ResourcesName.PlayerController);
        }

        public void LateTick()
        {
            PlayerGroundHelper.LateUpdate();
        }
    }
}
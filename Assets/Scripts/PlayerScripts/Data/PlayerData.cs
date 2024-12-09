using Cysharp.Threading.Tasks;
using Helpers;
using Inventory;
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
        public CeilingDetector CeilingDetector { get; private set; }
        public InventoryModel InventoryModel { get; private set; } //через медиатор сделать
        public ItemOperationMediator Mediator { get; private set; }

        public bool IsInit { get; private set; } = false;
        
        [Inject]
        private async void Construct(Player player, Inventory.Inventory inventory, PlayerInput playerInput)
        {
            InitResources();
            
            Player = player;
            Player.Init();
            RaycastHelper = new RaycastHelper(player.transform, PlayerHelpersConfig.Raycast);
            PlayerGroundHelper = new PlayerGroundHelper(Player, PlayerHelpersConfig.GroundHelper, RaycastHelper);
            PlayerInputReader = new PlayerInputReader(playerInput);
            CeilingDetector = Player.CeilingDetector;
            
            await UniTask.WaitUntil(() => inventory.IsInit);
            InventoryModel = inventory.InventoryStorage.InventoryModel;
            Mediator = inventory.ItemOperationMediator;
            IsInit = true;
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
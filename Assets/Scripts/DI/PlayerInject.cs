using Data;
using Inventory;
using Inventory.QuickSlot.Mediators;
using NewInput;
using QuickSlot;
using UnityEngine;

namespace DI
{
    public class PlayerInject : BaseBindings
    {
        [SerializeField] private Player _player;
        [SerializeField] private Inventory.Inventory _inventory;
        [SerializeField] private QuickBarSlotView _quickBarSlotView;
        public override void InstallBindings()
        {
            BindPlayer();
            BindInput();
            BindInventory();
            BindQuickSlot();
            BindData();
        }

        private void BindPlayer()
        {
            BindInstance(_player);
        }

        private void BindData()
        {
            BindNewInstanceWithArgument<PlayerData, Player>(_player);
        }

        private void BindInventory()
        {
            BindInstance(_inventory);
            BindNewInstance<ItemTypeResolver>();
            BindNewInstance<ItemCreator>();
            BindNewInstance<ItemOperationMediator>();
            BindNewInstance<ObjectPoolInventory>();
        }

        private void BindQuickSlot()
        {
            BindInstance(_quickBarSlotView);
            BindNewInstance<QuickBarSlotsModel>();
            BindNewInstance<QuickSlotInputMediator>();
            BindNewInstance<QuickBarSlotPresenter>();
        }

        private void BindInput()
        {
            BindNewInstance<PlayerInput>();
            BindNewInstance<UiInput>();
        }
    }
}
using Data;
using Inventory;
using NewInput;
using UnityEngine;

namespace DI
{
    public class PlayerInject : BaseBindings
    {
        [SerializeField] private Player _player;
        [SerializeField] private Inventory.Inventory _inventory;
        public override void InstallBindings()
        {
            BindPlayer();
            BindInput();
            BindInventory();
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

        private void BindInput()
        {
            BindNewInstance<PlayerInput>();
            BindNewInstance<UiInput>();
        }
    }
}
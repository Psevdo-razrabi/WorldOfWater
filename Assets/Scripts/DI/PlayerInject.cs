using Data;
using Inventory;
using UnityEngine;

namespace DI
{
    public class PlayerInject : BaseBindings
    {
        [SerializeField] private Player _player;
        [SerializeField] private Inventory.Inventory _inventory;
        public override void InstallBindings()
        {
            BindInventory();
            BindData();
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
        }
    }
}
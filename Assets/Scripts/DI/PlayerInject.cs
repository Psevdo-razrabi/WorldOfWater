using Data;
using UnityEngine;

namespace DI
{
    public class PlayerInject : BaseBindings
    {
        [SerializeField] private Player _player;
        public override void InstallBindings()
        {
            BindData();
        }

        private void BindData()
        {
            BindNewInstanceWithArgument<PlayerData, Player>(_player);
        }
    }
}
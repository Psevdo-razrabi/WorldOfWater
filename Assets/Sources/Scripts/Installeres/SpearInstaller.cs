using Sources.Scripts.Items;
using Sources.Scripts.Tools.Hook.Spear;
using Sources.Scripts.UI.ThrowToolsUI;
using Sources.Scripts.UI.ThrowToolsUI.SpearModel;
using UnityEngine;

namespace Game.DI
{
    public class SpearInstaller : BaseBindings
    {
        [SerializeField] private Spear _spear;
        [SerializeField] private ThrowSpear _throwSpear;
    
        public override void InstallBindings()
        {
            BindSpearLogic();
            BindUISpear();
        }

        private void BindSpearLogic()
        { 
            BindInstance(_spear);
            BindInstance(_throwSpear);
        }
        private void BindUISpear()
        {
            BindNewInstance<SpearModel>();
        }
    }
}
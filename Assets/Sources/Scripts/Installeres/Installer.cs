using Game.MVVM;
using Game.Services;
using Sources.Scripts.Hook.InputControllers;
using UnityEngine;

namespace Game.DI
{
    public class Installer : BaseBindings
    {
        
        public override void InstallBindings()
        {
            BindServices();
            BindFactories();
        }

        private void BindServices()
        {
            BindNewInstance<ContractsService>();
            BindNewInstance<ViewsService>();
        }

        private void BindFactories()
        {
            BindNewInstance<ViewModelFactory>();
        }
    }
}

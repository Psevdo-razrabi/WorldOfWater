using Game.MVVM;
using Game.Services;
using UnityEngine;

namespace Game.DI
{
    public class Installer : BaseBindings
    {
        [SerializeField] private ComputerViewsContainer _computerViewsContainer;

        public override void InstallBindings()
        {
            BindServices();
            BindFactories();
            BindViewsContainers();
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

        private void BindViewsContainers()
        {
            BindInstance(_computerViewsContainer);
        }
    }
}

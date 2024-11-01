using Game.MVVM;
using Game.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.DI
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private ViewsConfig _viewsConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterConfigs(builder);
            RegisterFactories(builder);
            RegisterServices(builder);
        }

        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<ViewModelFactory>(Lifetime.Singleton);
            builder.Register<ViewFactory>(Lifetime.Singleton);
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<ViewsService>(Lifetime.Singleton);
            builder.Register<LobbiesService>(Lifetime.Singleton);
            builder.Register<AuthenticationsService>(Lifetime.Singleton);
        }

        private void RegisterConfigs(IContainerBuilder builder)
        {
            builder.RegisterInstance(_viewsConfig);
        }
    }
}

using Game.MVVM;
using Game.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.DI
{
    public class BootstrapLifetimeScope : LifetimeScope
    {
        [SerializeField] private ViewsConfig _viewsConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterConfigs(builder);
            RegisterFactories(builder);
            RegisterServices(builder);
        }

        private void RegisterConfigs(IContainerBuilder builder)
        {
            builder.RegisterInstance(_viewsConfig);
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
            builder.Register<ScenesService>(Lifetime.Singleton);
            builder.Register<ChatService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
    }
}
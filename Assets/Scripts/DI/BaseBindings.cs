using Factory;
using Zenject;

public abstract class BaseBindings : MonoInstaller
{
    protected void BindNewInstance<T>() => Container
        .BindInterfacesAndSelfTo<T>()
        .AsSingle()
        .NonLazy();

    protected void BindInstance<T>(T instance) =>
        Container
            .BindInterfacesAndSelfTo<T>()
            .FromInstance(instance)
            .AsSingle()
            .NonLazy();

    protected void BindNewInstanceWithArgument<TK, T>(T argument) =>
        Container
            .BindInterfacesAndSelfTo<TK>()
            .AsSingle()
            .WithArguments(argument)
            .NonLazy();
    
}

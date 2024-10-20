using Game.Player;
using Unity.Netcode;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<PlayerMovementService>(Lifetime.Singleton);
        builder.Register<PlayerFactory>(Lifetime.Singleton);
    }
}

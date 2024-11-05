using Sources.Scripts.DI;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<Test>(Lifetime.Scoped);
    }
}

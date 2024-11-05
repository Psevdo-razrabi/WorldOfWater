using VContainer;

namespace Sources.Scripts.DI
{
    public class DependencyManager
    {
        private GameLifetimeScope _gameLifetimeScope;
        
        [Inject]
        public DependencyManager(GameLifetimeScope gameLifetimeScope)
        {
            _gameLifetimeScope = gameLifetimeScope;
        }

        public T GetDependency<T>() where T : class
        {
           return _gameLifetimeScope.Container.Resolve<T>();
        }
    }
}
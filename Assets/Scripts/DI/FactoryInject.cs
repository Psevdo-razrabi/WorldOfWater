using Factory;

namespace DI
{
    public class FactoryInject : BaseBindings
    {
        public override void InstallBindings()
        {
            BindNewInstance<PoolObject>();
            BindNewInstance<FactoryComponentWithMonoBehaviour>();
        }
    }
}
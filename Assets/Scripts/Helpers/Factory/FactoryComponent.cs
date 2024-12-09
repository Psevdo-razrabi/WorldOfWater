using Zenject;

namespace Factory
{
    public class FactoryComponent : Factory
    {
        public FactoryComponent(DiContainer diContainer) : base(diContainer)
        {
        }

        public T CreateComponentFromNew<T>() where T: class, new()
        {
            return new T();
        }
    }
}
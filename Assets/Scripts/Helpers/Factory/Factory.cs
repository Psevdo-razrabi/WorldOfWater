using Zenject;

namespace Factory
{
    public abstract class Factory
    {
        protected readonly DiContainer DiContainer;

        protected Factory(DiContainer diContainer)
        {
            DiContainer = diContainer;
        }
        
        public T CreateWithDiContainer<T>() where T : class
        {
            return DiContainer.Resolve<T>();
        }
    }
}
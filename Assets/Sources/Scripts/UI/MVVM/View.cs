using Game.MVVM;
using UnityEngine;
using Zenject;

public abstract class View<T> : View where T : ViewModel, new()
{
    protected T ViewModel; 
    protected Binder Binder;

    [Inject]
    private void Construct(ViewModelFactory viewModelFactory)
    {
        ViewModel = viewModelFactory.Create<T>();
        Binder = ViewModel.Binder;
        Init();
    }
}

public abstract class View : MonoBehaviour
{
    public virtual bool IsAlwaysActivated { get; }
    public abstract string Id { get; }
    public abstract void Init();
}
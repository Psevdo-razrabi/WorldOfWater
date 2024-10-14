using Game.MVVM;
using Game.Services;
using UnityEngine;
using Zenject;

public abstract class View : MonoBehaviour
{
    protected string Id;

    [Inject]
    private void Construct(ViewModelFactory viewModelFactory)
    {
        Init(viewModelFactory);
    }

    public abstract void Init(ViewModelFactory viewModelFactory);
}
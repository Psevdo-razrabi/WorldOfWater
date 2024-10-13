using Game.MVVM;
using UnityEngine;
using Zenject;

public abstract class View : MonoBehaviour
{
    [Inject]
    private void Construct(ViewModelFactory viewModelFactory)
    {
        Init(viewModelFactory);
    }

    public abstract void Init(ViewModelFactory viewModelFactory);
}
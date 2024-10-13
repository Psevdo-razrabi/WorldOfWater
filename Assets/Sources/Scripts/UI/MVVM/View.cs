using Game.MVVM;
using UnityEngine;
using Zenject;

public abstract class View : MonoBehaviour
{
    public string Id { get; set; }
    public bool IsActivedOnStart { get; set; }

    [Inject]
    private void Construct(ViewModelFactory viewModelFactory)
    {
        Init(viewModelFactory);
    }

    public abstract void Init(ViewModelFactory viewModelFactory);
}
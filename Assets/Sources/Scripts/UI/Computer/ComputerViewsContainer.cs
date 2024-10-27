using Game.Services;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Game.MVVM
{
    public class ComputerViewsContainer : MonoBehaviour
    {
        [SerializeField] private List<View> _views;

        [Inject]
        private void Construct(ViewsService viewsService)
        {
        }
    }
}
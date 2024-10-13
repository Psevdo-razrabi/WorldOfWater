using Game.Services;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.MVVM
{
    public class ComputerViewsContainer : MonoBehaviour
    {
        [SerializeField] private List<View> _views;

        [Inject]
        private void Construct(ViewsService viewsService)
        {
            viewsService.Create(_views);
        }
    }
}
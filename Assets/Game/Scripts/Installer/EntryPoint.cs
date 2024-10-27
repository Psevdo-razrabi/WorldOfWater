using Game.MVVM;
using Game.Services;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.DI
{
    public class EntryPoint : MonoBehaviour
    {
        public void Start()
        {
        }

        /*[Inject]
        private async void Inject(IViewFactory viewFactory)
        {
            var view = await viewFactory.Create(ViewId.CreateWorld);
            view.Init();
        }*/
    }
}

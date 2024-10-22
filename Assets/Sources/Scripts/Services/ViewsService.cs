using Cysharp.Threading.Tasks;
using Game.MVVM;
using System.Collections.Generic;

namespace Game.Services
{
    public class ViewsService
    {
        private readonly Dictionary<string, View> _views = new();
        private string _currentViewId;

        public async void Create(List<View> views)
        {
            foreach (var view in views)
            {
                await UniTask.WaitUntil(() => view.Id != string.Empty);

                view.gameObject.SetActive(view.IsAlwaysActivated);
                _views.Add(view.Id, view);
            }

            Open(ViewIds.CONTRACTS);
        }

        public void Open(string id)
        {
            if (_views.TryGetValue(id, out var view))
            {
                _currentViewId = id;
                view.gameObject.SetActive(true);
            }
        }

        public void Close()
        {
            if (_views.TryGetValue(_currentViewId, out var view))
            {
                view.gameObject.SetActive(view.IsAlwaysActivated);
            }
        }
    }
}

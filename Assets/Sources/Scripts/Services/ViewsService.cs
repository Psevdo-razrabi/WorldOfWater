using Game.MVVM;
using System.Collections.Generic;
using VContainer.Unity;

namespace Game.Services
{
    public class ViewsService : IInitializable
    {
        private readonly ViewFactory _viewFactory;
        private readonly Dictionary<ViewId, View> _views = new();
        private readonly Stack<View> _viewsStack = new();

        public ViewsService(ViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
        }
        
        public void Initialize()
        {
            Open(ViewId.CreateWorld);
        }

        public async void Open(ViewId id)
        {
            if (_views.TryGetValue(id, out var view))
            {
                view.gameObject.SetActive(true);
                _viewsStack.Push(view);
            }
            else
            {
                var newView = await _viewFactory.Create(id);
                newView.gameObject.SetActive(true);
                _views.Add(id, newView);
                _viewsStack.Push(newView);
            }
        }

        public void Close()
        {
            if(_viewsStack.TryPop(out var view))
            {
                view.gameObject.SetActive(false);
            }
        }
    }
}

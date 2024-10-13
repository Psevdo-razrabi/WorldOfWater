using System.Collections.Generic;

namespace Game.Services
{
    public class ViewsService
    {
        private readonly Dictionary<string, View> _views = new();
        private string _currentViewId;

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
                view.gameObject.SetActive(true);
            }
        }
    }
}

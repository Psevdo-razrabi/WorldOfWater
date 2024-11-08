using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.MVVM
{
    public class Button : MonoBehaviour, IBindable, IPointerClickHandler
    {
        private Binder _binder;

        public void Bind(Binder binder)
        {
            _binder = binder;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            _binder?.TriggerButtonEvent<Click>(this);
        }
    }
}
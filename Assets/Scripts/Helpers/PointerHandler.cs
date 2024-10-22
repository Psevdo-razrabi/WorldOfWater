using UnityEngine;
using UnityEngine.EventSystems;

namespace Helpers
{
    public class PointerHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public PointerEventData PointerEventData { get; private set; }
        public void OnBeginDrag(PointerEventData eventData)
        {
            PointerEventData = eventData;
            Debug.LogWarning(eventData.delta);
        }

        public void OnDrag(PointerEventData eventData)
        {
            PointerEventData = eventData;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            PointerEventData = eventData;
        }
    }
}
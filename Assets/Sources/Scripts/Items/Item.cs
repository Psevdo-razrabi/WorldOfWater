using UnityEngine;

namespace Sources.Scripts.Items
{
    public class Item : MonoBehaviour, IDruggable
    {
        [SerializeField] private ItemType _itemType;

        public ItemType ItemType => _itemType;
        public Transform GetTransform()
        {
            return this.transform;
        }
    }
}
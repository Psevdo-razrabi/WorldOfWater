using Helpers;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class ItemDescription : ScriptableObject
    {
        [field: SerializeField] public string NameItem { get; private set; }
        [field: SerializeField] public int MaxStack { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        public GuidItem Id { get; private set; } = GuidItem.NewGuid();
    }
}

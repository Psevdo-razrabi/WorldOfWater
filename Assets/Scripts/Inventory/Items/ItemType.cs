using UnityEngine;

namespace Inventory
{
    public class ItemType : MonoBehaviour
    {
        [field: SerializeField] public EItemType Type { get; private set; } 
    }
}
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "Prefabs", menuName = "Inventory/ItemPrefabs")]
    public class ItemsPrefabs : ScriptableObject
    {
        [field: SerializeField] public GameObject MetalPrefab { get; private set; }
        [field: SerializeField] public GameObject ClothPrefab { get; private set; }
        [field: SerializeField] public GameObject WoodPrefab { get; private set; }
        [field: SerializeField] public GameObject PlasticPrefab { get; private set; }
    }
}
using UnityEngine;

namespace QuickSlot.Db
{
    [CreateAssetMenu(fileName = "QuickSlots", menuName = "QuickSlots/Parameters")]
    public class QuickSlotsParameters : ScriptableObject
    {
        [field: SerializeField] public QuickBarParameters Parameters { get; private set; }
    }
}
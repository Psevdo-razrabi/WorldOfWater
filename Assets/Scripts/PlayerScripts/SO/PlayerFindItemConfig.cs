using UnityEngine;

namespace PlayerScripts.SO
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/FindItem")]
    public class PlayerFindItemConfig : ScriptableObject
    {
        [field: SerializeField] public float Distance { get; private set; }
        [field: SerializeField] public LayerMask Layer { get; private set; }
    }
}
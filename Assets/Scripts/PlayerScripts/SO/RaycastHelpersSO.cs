using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/RaycastHelpers")]
public class RaycastHelpersSO : ScriptableObject
{
    [field: Range(0f, 100f)]
    [field: SerializeField] public float CastLength { get; private set; }
    [field: SerializeField] public LayerMask LayerMask { get; private set; }
}
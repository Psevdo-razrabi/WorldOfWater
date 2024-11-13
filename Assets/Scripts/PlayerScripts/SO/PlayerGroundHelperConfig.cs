using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/GroundHelpers")]
public class PlayerGroundHelperConfig : ScriptableObject
{
    [field: Range(0f, 1f)] 
    [field: SerializeField] public float StepHeightRatio { get; private set; }
    [field: SerializeField] public float ColliderHeight { get; private set; } 
    [field: SerializeField] public float ColliderThickness { get; private set; }
    [field: SerializeField] public Vector3 ColliderOffset { get; private set; }
    [field: SerializeField] public bool IsInDebugMode { get; private set; }
}
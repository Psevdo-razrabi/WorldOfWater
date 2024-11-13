using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/CeilingHelper")]
public class CeilingDetectorSO : ScriptableObject
{
    [field: SerializeField] public float CeilingAngleLimit { get; private set; }
    [field: SerializeField] public bool IsInDebugMode { get; private set; }
    [field: SerializeField] public float DebugDrawDuration { get; private set; }
}
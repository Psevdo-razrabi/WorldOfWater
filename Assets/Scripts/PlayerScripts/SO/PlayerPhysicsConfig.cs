using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/Physics")]
public class PlayerPhysicsConfig : ScriptableObject
{
    [field: SerializeField] public float AirFriction { get; private set; }
    [field: SerializeField] public float GroundFriction { get; private set; }
    [field: SerializeField] public float Gravity { get; private set; }
    [field: SerializeField] public float SlideGravity { get; private set; }
    [field: SerializeField] public float SlopeLimit { get; private set; }
}
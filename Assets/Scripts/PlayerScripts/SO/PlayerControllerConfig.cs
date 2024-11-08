using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/Controller")]
public class PlayerControllerConfig : ScriptableObject
{
    [field: SerializeField] public PlayerMovementConfig MovementConfig { get; private set; }
    [field: SerializeField] public PlayerPhysicsConfig PhysicsConfig { get; private set; }
}
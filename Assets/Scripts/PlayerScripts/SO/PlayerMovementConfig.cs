using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/Movement")]
public class PlayerMovementConfig : ScriptableObject
{
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float JumpSpeed { get; private set; }
    [field: SerializeField] public float JumpDuration { get; private set; }
    [field: SerializeField] public float AirControlRate { get; private set; }
}
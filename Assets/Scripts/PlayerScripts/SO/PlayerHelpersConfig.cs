using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/PlayerHelpersConfig")]
public class PlayerHelpersConfig : ScriptableObject
{
    [field: SerializeField] public RaycastHelpersSO Raycast { get; private set; }
    [field: SerializeField] public PlayerGroundHelperConfig GroundHelper { get; private set; }
}
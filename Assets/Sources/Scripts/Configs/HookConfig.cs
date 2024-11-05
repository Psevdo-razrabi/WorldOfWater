using UnityEngine;

namespace Sources.Scripts.Configs
{
    [CreateAssetMenu(fileName = "HookConfig", menuName = "Configs/New HookConfig")]
    public class HookConfig : ThrowToolConfig
    {
        [field: SerializeField] public float ReturnOnWaterSpeed;
    }
}
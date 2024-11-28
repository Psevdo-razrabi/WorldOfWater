using Sources.Scripts.UI.ThrowToolsUI.Hook;
using UnityEngine;

namespace Sources.Scripts.Configs
{
    [CreateAssetMenu(fileName = "SpearConfig", menuName = "Configs/New SpearConfig")]
    public class SpearConfig : ThrowToolConfig
    {
        [field: SerializeField] public float Damage;
    }
}
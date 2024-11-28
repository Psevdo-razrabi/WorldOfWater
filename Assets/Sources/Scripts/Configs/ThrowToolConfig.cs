using Sources.Scripts.Configs.Interfaces;
using UnityEngine;

namespace Sources.Scripts.Configs
{
    public abstract class  ThrowToolConfig : ScriptableObject, IThrowToolConfig
    {
        [field: SerializeField] public float ThrowForce { get; private set; }
        [field: SerializeField] public float SpeedFillCircle { get; private set; }
    }
}
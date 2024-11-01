using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Linq;

namespace Game.MVVM
{
    [CreateAssetMenu(fileName = "ViewsConfig", menuName = "Configs/ViewsConfig")]
    public class ViewsConfig : ScriptableObject
    {
        [field: SerializeField] public Canvas Canvas { get; private set; }
        [field: SerializeField] public List<View> Views { get; private set; }
    }
}

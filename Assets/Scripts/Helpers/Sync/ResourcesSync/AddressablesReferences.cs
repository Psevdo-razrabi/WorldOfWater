using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Sync
{
    public class AddressablesReferences : MonoBehaviour
    {
        [field: SerializeField] public AssetLabelReference ItemsReferences { get; private set; }
    }
}
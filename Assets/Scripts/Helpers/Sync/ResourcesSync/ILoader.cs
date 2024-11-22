using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Sync
{
    public interface ILoader
    {
        IReadOnlyList<Func<UniTask>> Loaders { get; }
        ReactiveProperty<bool> IsLoaded { get; }
        UniTask LoadFromResources(string path, string key);
        UniTask LoadFromAddressablesWithLabel(AssetLabelReference labelReference, string key);
    }
}
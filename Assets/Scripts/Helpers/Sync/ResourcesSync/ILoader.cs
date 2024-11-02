using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace Sync
{
    public interface ILoader
    {
        IReadOnlyList<Func<UniTask>> Loaders { get; }
        ReactiveProperty<bool> IsLoaded { get; }
        UniTask LoadFromResources(string path, string key);
        UniTask LoadFromAddressables();
    }
}
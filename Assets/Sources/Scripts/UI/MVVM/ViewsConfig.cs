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
        [field: SerializeField] public List<ViewData> Views { get; private set; }

        public async UniTask<View> LoadView(ViewId id, Transform parent)
        {
            var asset = Views.First(v => v.Id == id).Prefab;
            var prefab = await Addressables.InstantiateAsync(asset.AssetGUID, parent: parent);
            var view = prefab.GetComponent<View>();
            return view;
        }
    }

    [Serializable]
    public class ViewData
    {
        [field: SerializeField] public ViewId Id { get; private set; }
        [field: SerializeField] public AssetReference Prefab { get; private set; }
    }
}

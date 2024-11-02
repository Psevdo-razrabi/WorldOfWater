using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Inventory
{
    public class UiAnimation
    {
        public async UniTaskVoid AnimationWithScale(float startScale, float endValue, Transform transform, float duration, CancellationToken token)
        {
            await DOTween.
                To(() => startScale, x => transform.localScale = new Vector3(x, x, x), endValue, duration)
                .WithCancellation(token);
        }
    }
}
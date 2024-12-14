using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class UiAnimation
    {
        private Tween _scaleAnimation;
        private Tween _fadeAnimation;
        private Tween _positionXAnimation;
        private Tween _positionYAnimation;
        private bool _punchScaleAnimation;
        private bool _isPositionXAnimation;
        
        public async UniTaskVoid AnimationWithScale(float startScale, float endValue, Transform transform, float duration, CancellationToken token)
        {
            await DOTween.
                To(() => startScale, x => transform.localScale = new Vector3(x, x, x), endValue, duration)
                .WithCancellation(token);
        }

        public async UniTaskVoid AnimationWithPunch(Transform transform, Vector3 punch, float duration, Ease ease, int vibrato = 10, float elasticity = 1f)
        {
            if(_punchScaleAnimation) return;
            
            _punchScaleAnimation = true;
            _scaleAnimation?.Kill();
            _scaleAnimation = transform.DOPunchScale(punch, duration, vibrato, elasticity)
                .SetEase(ease)
                .OnComplete(() => _punchScaleAnimation = false);

            await _scaleAnimation;
        }

        public void AnimationWithAlpha(Image image, float alpha, float duration, Ease ease)
        {
            _fadeAnimation?.Kill();

            _fadeAnimation = image.DOFade(alpha, duration).SetEase(ease);
        }

        public void AnimationWithPositionX(RectTransform transform, float offset, float animationDuration, Ease ease)
        {
            if(_isPositionXAnimation) return;

            _isPositionXAnimation = true;
            _positionXAnimation?.Kill();
            
            _positionXAnimation = transform.DOAnchorPosX(offset, animationDuration)
                .SetEase(ease)
                .OnComplete(() => _isPositionXAnimation = false);
        }
        
        public void AnimationWithPositionY(RectTransform transform, float offset, float animationDuration, Ease ease)
        {
            _positionYAnimation?.Kill();
            
            _positionYAnimation = transform.DOAnchorPosX(offset, animationDuration).SetEase(ease);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CircleLoadbarView : MonoBehaviour
{
    [SerializeField] private Image _loadingBar;
    private Tween fillTween;
    private Tween rotateTween;
    
    public float FillAmount => _loadingBar.fillAmount;
    
    public void StartLoading(float fillSpeed)
    {
        _loadingBar.fillAmount = 0f;
        
        fillTween = _loadingBar.DOFillAmount(1f, fillSpeed)
            .SetEase(Ease.Linear);
    }
    
    public void CancelLoading()
    {
        if (fillTween != null && fillTween.IsActive())
            fillTween.Kill();

        _loadingBar.fillAmount = 0f;
    }
}

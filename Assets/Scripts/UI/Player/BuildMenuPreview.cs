using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class BuildMenuPreview : MonoBehaviour
{
    public Image image;
    public bool selected;
    [Header("Animation")]
    public float selectDuration = 0.5f;
    public float deselectDuration = 0.5f;
    public float selectDesiredScale = 1.2f;
    public float deselectDesiredScale = 1f;
    public Ease selectEase = Ease.OutBack;
    public Ease deselectEase = Ease.OutBack;
    private Tween tweenCurrent;
    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }
    public void Select()
    {
        selected = true;
        AnimateSelection(selectDesiredScale, selectDuration, selectEase);
    }
    public void Deselect()
    {
        selected = false;
        AnimateSelection(deselectDesiredScale, deselectDuration, deselectEase);
    }
    private void AnimateSelection(float targetScale, float duration, Ease ease)
    {
        tweenCurrent?.Kill();

        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            tweenCurrent = GetComponent<RectTransform>().DOScale(targetScale, duration).SetEase(ease);
        }
    }

    private void OnDestroy()
    {
        tweenCurrent?.Kill();
    }

    private void OnDisable()
    {
        tweenCurrent?.Kill();
    }
}

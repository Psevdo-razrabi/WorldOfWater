using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[CreateAssetMenu]
public class PlayerSystemConfig : ScriptableObject
{
    [Header("Main")]
    public string instanceName;
    public float maxValue;
    public float minValue;
    public float initValue;
    public bool clampValue;


    [Header("UI")]
    public Slider slider;


    [Header("Animation")]
    public bool isAnimate;
    public Ease animationEase;
    public float animationDuration;
}

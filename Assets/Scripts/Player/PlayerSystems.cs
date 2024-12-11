using UnityEngine;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;
using DG.Tweening;

public class PlayerSystems : MonoBehaviour
{
    public struct BaseSystem
    {
        public string instanceName;
        public float value { get; private set; }
        public float maxValue { get; private set; }
        public float minValue { get; private set; }
        public Action OnValueUpdate;
        private bool clampValue;

        public void InitSystem(PlayerSystemConfig playerSystemConfig)
        {
            instanceName = playerSystemConfig.instanceName;

            value = playerSystemConfig.initValue;
            maxValue = playerSystemConfig.maxValue;
            minValue = playerSystemConfig.minValue;

            clampValue = playerSystemConfig.clampValue;

        }

        public void SetMaxValue(float newMaxValue)
        {
            maxValue = newMaxValue;
        }

        public void SetMinValue(float newMinValue)
        {
            minValue = newMinValue;
        }

        public void SetMaxAndMinValues(float newMaxValue, float newMinValue)
        {
            SetMaxValue(newMaxValue);
            SetMinValue(newMinValue);
        }
        
        public void SetValue(float newValue)
        {
            float prevValue = value;
            value = newValue;

            if(clampValue)
            {
                value = Mathf.Clamp(value, minValue, maxValue);
            }
            else if(value > maxValue || value < minValue)
            {
                Debug.LogError("New Value Is Out Of Range In " + instanceName + ". Value Is Set To Previous State.");
                value = prevValue;
                return;
            }

            OnValueUpdate?.Invoke();
        }

        public void AddToValue(float newValue)
        {
            value += newValue;

            if(clampValue)
            {
                value = Mathf.Clamp(value, minValue, maxValue);
            }
            else if(value > maxValue)
            {
                Debug.LogError("New Value Is Out Of Range In " + instanceName + ". Value Is Set To Previous State.");
                value -= newValue;
                return;
            }

            OnValueUpdate?.Invoke();
        }

        public void SubstractFromValue(float newValue)
        {
            value -= newValue;

            if(clampValue)
            {
                value = Mathf.Clamp(value, minValue, maxValue);
            }
            else if(value < minValue)
            {
                Debug.LogError("New Value Is Out Of Range In " + instanceName + ". Value Is Set To Previous State.");
                value += newValue;
                return;
            }

            OnValueUpdate?.Invoke();
        }
    }

    public struct BaseSystemUI
    {
        public Action OnSliderAnimationComplete;

        private Slider slider;
        private bool isAnimate;
        private PlayerSystemConfig playerSystemConfig;
        private Tween currentAnimation;



        public void SetUI(BaseSystem baseSystem)
        {
            if(slider == null)
            {
                Debug.LogError("No Slider Attached To " + baseSystem.instanceName + ".");
            }
            else
            {
                if(isAnimate)
                {
                    AnimateSlider(baseSystem);
                }
                else
                {
                    slider.value = baseSystem.value;
                }
            }
        }

        private void AnimateSlider(BaseSystem baseSystem)
        {
            if (currentAnimation != null && currentAnimation.IsPlaying())
            {
                currentAnimation.Kill();
            }


            BaseSystemUI baseSystemUI = this;

            currentAnimation = slider.DOValue(baseSystem.value, playerSystemConfig.animationDuration)
                .SetEase(playerSystemConfig.animationEase)
                .OnComplete(() => 
                {
                    baseSystemUI.OnSliderAnimationComplete?.Invoke();
                });

        }



        public void InitUI(PlayerSystemConfig newPlayerSystemConfig, BaseSystem newBaseSystem, Slider newSlider)
        {
            playerSystemConfig = newPlayerSystemConfig;

            slider = newSlider;

            slider.maxValue = newPlayerSystemConfig.maxValue;
            slider.minValue = newPlayerSystemConfig.minValue;
            slider.value = newPlayerSystemConfig.initValue;

            isAnimate = newPlayerSystemConfig.isAnimate;

        }

    }
}

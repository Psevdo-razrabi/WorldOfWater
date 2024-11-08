using System;
using System.Collections.Generic;
using System.ComponentModel;
using DG.Tweening;
using UnityEngine;


public interface IAnimationElement
{
    Tween Play(Action<Action> onFinish, Action onFinishCallback, Action onUpdate);

    string GetName();
    void SetName(string newName);
    string GetDefaultName();

}


public class VectorAnimation : MonoBehaviour
{
    public List<AnimationElement> animationSequence;
    public static string defaultInpectorName = "New Animation";
    public string inspectorName = defaultInpectorName;
    public bool isPlaying;
    public bool updateConnects;
    public List<VectorConnect> connects = new List<VectorConnect>();

    public Action OnComplete;

    public List<Tween> tweens = new List<Tween>();
    
    private int index = 0;

    public void Play()
    {
        if(isPlaying) return;

        tweens = new List<Tween>();
        isPlaying = true;
        index = 0;
        PlaySequence(index);
    }

    private void PlaySequence(int index)
    {
        if(index < animationSequence.Count)
        {
            List<Tween> tempTweens = new List<Tween>();
            tempTweens = animationSequence[index].Play(MoveIndex, updateConnects, connects);

            tweens.AddRange(tempTweens);
        }
        else
        {
            OnComplete?.Invoke();
            Stop();
        }
    }

    private void MoveIndex()
    {
        index++;
        PlaySequence(index);
    }

    public void Stop()
    {
        isPlaying = false;
        for(int i = 0; i < tweens.Count; i++)
        {
            tweens[i].Kill();
        }

        tweens.Clear();
    }

}

[Serializable]
public struct AnimationElement
{
    public enum AnimationType
    {
        Move, Width, Rotate, ChangeRadius, Color
    }


    [SerializeReference]
    public List<IAnimationElement> animationElements;

    public LineRenderer lineRenderer;
    public bool isMoveElement;
    public AnimationType animationType;
    public bool isRotateElement;
    public RotateElement rotateElement;
    public bool isAnimation;

    public bool updateConnects;
    public List<VectorConnect> connects;
    

    private int completedAnimations;


    public List<Tween> Play(Action onFinish, bool updateConnects, List<VectorConnect> connects)
    {
        this.updateConnects = updateConnects;
        this.connects = connects;


        completedAnimations = 0;
        var self = this;

        List<Tween> tweens = new List<Tween>();

        for(int i = 0 ; i < animationElements.Count; i++)
        {
            tweens.Add(animationElements[i].Play(state => self.UpdateAnimationState(onFinish), onFinish, OnUpdate));
        }

        return tweens;
    }

    public void OnUpdate()
    {
        if(updateConnects)
        {
            foreach(VectorConnect connect in connects)
            {
                connect.UpdatePosition();
            }
        }
    }

    public void AddAnimation<T>(AnimationType newAnimationType, AnimationType animationCompare, T animationClass) where T : IAnimationElement
    {
        if(newAnimationType == animationCompare)
        {
            animationElements.Add(animationClass);
        }
    }

    private void UpdateAnimationState(Action callback)
    {
        completedAnimations++;
        if(completedAnimations == animationElements.Count)
        {
            callback?.Invoke();
        }
    }

}


[Serializable]
public abstract class AnimateElement : IAnimationElement
{
    public string name;
    public string defaultInpectorName;
    public float duration;
    public Ease ease;

    public string GetName()
    {
        return name;
    }

    public void SetName(string newName)
    {
        name = newName;
    }

    public string GetDefaultName()
    {
        return defaultInpectorName;
    }


    public abstract void UpdateLine(float lerpVal);
    public abstract Tween Play(Action<Action> onFinish, Action onFinishCallback, Action onUpdate);
    protected void ExecuteUpdate(float lerpVal, Action onUpdate)
    {
        onUpdate?.Invoke();
        UpdateLine(lerpVal);
    }
}

[Serializable]
public class MoveElement : AnimateElement
{
    [ReadOnly(true)] public Vector3 startPosition;
    [ReadOnly(true)] public Vector3 currentPosition;
    public int point;
    public LineRenderer lineRenderer;
    public Vector3 endPosition;

    public MoveElement()
    {
        defaultInpectorName = "New Move Animation";
    }


    public override Tween Play(Action<Action> onFinish, Action onFinishCallback, Action onUpdate)
    {
        startPosition = lineRenderer.GetPosition(point);
        float lerpVal = 0;

        return DOTween.To(x => lerpVal = x, 0, 1, duration).OnUpdate(() => ExecuteUpdate(lerpVal, onUpdate)).OnComplete(() => onFinish(onFinishCallback)).SetEase(ease);
    }

    public override void UpdateLine(float lerpVal)
    {
        lineRenderer.SetPosition(point, Vector3.Lerp(startPosition, endPosition, lerpVal));
        currentPosition = lineRenderer.GetPosition(point);


    }

}

[Serializable]
public class WidthElement : AnimateElement
{
    public LineRenderer lineRenderer;
    public float startWidthStart;
    public float startWidthEnd;
    public float endWidthStart;
    public float endWidthEnd;
    public bool isConstant;
    public bool isAdding;
    public bool isSubtracting;

    public WidthElement()
    {
        isConstant = true;
        defaultInpectorName = "New Width Animation";
        SetName(defaultInpectorName);
    }

    public override Tween Play(Action<Action> onFinish, Action onFinishCallback, Action onUpdate)
    {
        startWidthStart = lineRenderer.startWidth;
        startWidthEnd = lineRenderer.endWidth;


        float lerpVal = 0f;

        return DOTween.To(x => lerpVal = x, 0, 1, duration).OnUpdate(() => ExecuteUpdate(lerpVal, onUpdate)).OnComplete(() => onFinish(onFinishCallback)).SetEase(ease);
    }

    public override void UpdateLine(float lerpVal)
    {
        if(isAdding)
        {
            lineRenderer.startWidth = Mathf.Lerp(startWidthStart, startWidthStart + endWidthStart, lerpVal);
            lineRenderer.endWidth = Mathf.Lerp(startWidthEnd, startWidthEnd + endWidthEnd, lerpVal);
        }
        else if(isSubtracting)
        {
            lineRenderer.startWidth = Mathf.Lerp(startWidthStart, startWidthStart - endWidthStart, lerpVal);
            lineRenderer.endWidth = Mathf.Lerp(startWidthEnd, startWidthEnd - endWidthEnd, lerpVal);
        }
        else
        {
            lineRenderer.startWidth = Mathf.Lerp(startWidthStart, endWidthStart, lerpVal);
            lineRenderer.endWidth = Mathf.Lerp(startWidthEnd, endWidthEnd, lerpVal);
        }
        
    }


}

[Serializable]
public class ChangeRadius : AnimateElement
{
    public VectorCircle vectorCircle;
    public LineRenderer lineRenderer;
    public float startRadius;
    public float endRadius;
    public bool isAdding;
    public bool isSubtracting;

    public ChangeRadius()
    {
        defaultInpectorName = "New ChangeRadius Animation";
        SetName(defaultInpectorName);
    }

    public override Tween Play(Action<Action> onFinish, Action onFinishCallback, Action onUpdate)
    {
        startRadius = vectorCircle.radius;

        float lerpVal = 0f;

        return DOTween.To(x => lerpVal = x, 0, 1, duration).OnUpdate(() => ExecuteUpdate(lerpVal, onUpdate)).OnComplete(() => onFinish(onFinishCallback)).SetEase(ease);
        
    }


    
    public override void UpdateLine(float lerpVal)
    {
        if(isAdding)
        {
            vectorCircle.radius = Mathf.Lerp(startRadius, startRadius + endRadius, lerpVal);
        }
        else if(isSubtracting)
        {
            vectorCircle.radius = Mathf.Lerp(startRadius, startRadius - endRadius, lerpVal);
        }
        else
        {
            vectorCircle.radius = Mathf.Lerp(startRadius, endRadius, lerpVal);
        }
        vectorCircle.Draw(false);
    }
}

[Serializable]
public class ColorElement : AnimateElement
{
    public Color startColorStart;
    public Color startColorEnd;
    public Color endColorStart;
    public Color endColorEnd;
    public bool isConstant;
    public LineRenderer lineRenderer;

    public ColorElement()
    {
        isConstant = true;
        defaultInpectorName = "New Color Animation";
        SetName(defaultInpectorName);
    }

    public override Tween Play(Action<Action> onFinish, Action onFinishCallback, Action onUpdate)
    {
        startColorStart = lineRenderer.startColor;
        startColorEnd = lineRenderer.endColor;


        float lerpVal = 0f;

        return DOTween.To(x => lerpVal = x, 0, 1, duration).OnUpdate(() => ExecuteUpdate(lerpVal, onUpdate)).OnComplete(() => onFinish(onFinishCallback)).SetEase(ease);

    }

    public override void UpdateLine(float lerpVal)
    {
        lineRenderer.startColor = Color.Lerp(startColorStart, endColorStart, lerpVal);
        lineRenderer.endColor = Color.Lerp(startColorEnd, endColorEnd, lerpVal);
    }
}

[Serializable]
public class RotateElement : AnimateElement
{



    public override Tween Play(Action<Action> onFinish, Action onFinishCallback, Action onUpdate)
    {
        return null;
    }

    public override void UpdateLine(float lerpVal)
    {
       
    }
}

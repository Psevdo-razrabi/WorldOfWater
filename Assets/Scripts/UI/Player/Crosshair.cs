using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Crosshair : Singleton<Crosshair>
{
    public enum State { Default, Interact, CanInteract, CantInteract }


    public VectorAnimation DefaultStateAnimation;
    public VectorAnimation DefaultSizeStateAnimation;
    public VectorAnimation InteractStateAnimation;
    public VectorAnimation CanInteractStateAnimation;
    public VectorAnimation CantInteractStateAnimation;
    public VectorAnimation Click;
    public State CurrentState { get; private set; }

    public void SetState(State newState)
    {
        if(newState != CurrentState)
        {
            CurrentState = newState;
            StartCoroutine(ChangeStateCoroutine());
        }
    }

    private IEnumerator ChangeStateCoroutine()
    {
        yield return new WaitUntil(() => AnimationsStop(true));

        SetAnimation(CurrentState);

        yield return null;
    }


    private void SetAnimation(State newState)
    {
        switch (newState)
        {
            case State.Default:
                DefaultStateAnimation.Play();
                break;
            case State.Interact:
                InteractStateAnimation.Play();
                break;
            case State.CanInteract:
                InteractStateAnimation.Play();
                CanInteractStateAnimation.Play();
                break;
            case State.CantInteract:
                DefaultSizeStateAnimation.Play();
                CantInteractStateAnimation.Play();
                break;

        }
    }

    public void Update()
    {
        if(Input.GetMouseButtonDown(0) && CurrentState == State.Default)
        {
            Click.Stop();
            Click.Play();
        }
    }

    public VectorAnimation[] GetAllAnimations()
    {
        VectorAnimation[] animations = {
            DefaultStateAnimation,
            DefaultSizeStateAnimation,
            InteractStateAnimation,
            CanInteractStateAnimation,
            CantInteractStateAnimation,
            Click
        };
        return animations;
    }


    public bool AnimationsStop(bool stop)
    {
        bool confirm = true;

        foreach(var animation in GetAllAnimations())
        {
            if(animation.tweens.Count > 0)
            {
                if(stop)
                {
                    animation.Stop();
                }
                confirm = false;
            }
        }


        return confirm;

    }



}

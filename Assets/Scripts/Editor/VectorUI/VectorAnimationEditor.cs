using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

using static AnimationElement;


[CustomEditor(typeof(VectorAnimation))]
public class VectorAnimationEditor : Editor
{
    private SerializedProperty animationSequenceProperty;
    private List<List<bool>> foldoutStates;

    private void OnEnable()
    {
        animationSequenceProperty = serializedObject.FindProperty("animationSequence");
    }

    public override void OnInspectorGUI()
    {
        VectorAnimation vectorAnimation = (VectorAnimation)target;

        serializedObject.Update();

        if(vectorAnimation.inspectorName == "")
        {
            vectorAnimation.inspectorName = VectorAnimation.defaultInpectorName;
        }

        EditorGUILayout.LabelField(vectorAnimation.inspectorName, EditorStyles.boldLabel);

        vectorAnimation.inspectorName = EditorGUILayout.TextField("Animation Name", vectorAnimation.inspectorName);

        vectorAnimation.updateConnects = EditorGUILayout.Toggle("Update Connects", vectorAnimation.updateConnects);

        if(vectorAnimation.updateConnects)
        {
            if(GUILayout.Button("Add Connect"))
            {
                vectorAnimation.connects.Add(null);
            }

            for(int i = 0; i < vectorAnimation.connects.Count; i++)
            {

                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.BeginHorizontal();

                vectorAnimation.connects[i] = (VectorConnect)EditorGUILayout.ObjectField("VectorConnect", vectorAnimation.connects[i], typeof(VectorConnect), true);

                if(GUILayout.Button("Remove"))
                {
                    vectorAnimation.connects.RemoveAt(i);
                    return;
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();



            }
        }


        if(vectorAnimation.animationSequence == null)
        {
            vectorAnimation.animationSequence = new List<AnimationElement>();
        }

        if(GUILayout.Button("Add Animation Sequence"))
        {
            vectorAnimation.animationSequence.Add(new AnimationElement());
        }



        if(vectorAnimation.animationSequence.Count == 0)
        {
            EditorGUILayout.HelpBox("Animation sequence is empty.", MessageType.Warning);
            serializedObject.ApplyModifiedProperties();

            return;
        }

        if(foldoutStates == null || foldoutStates.Count != vectorAnimation.animationSequence.Count)
        {
            InitializeFoldoutListSequence(vectorAnimation);
        }

        for(int i = 0; i < vectorAnimation.animationSequence.Count; i++)
        {

            AnimationElement animationElement = vectorAnimation.animationSequence[i];

            EditorGUILayout.BeginVertical("box");


            if(animationElement.animationElements == null)
            {
                animationElement.animationElements = new List<IAnimationElement>();
            }

            

            animationElement.lineRenderer = (LineRenderer)EditorGUILayout.ObjectField("Line Renderer", animationElement.lineRenderer, typeof(LineRenderer), true);

            if(animationElement.lineRenderer == null)
            {
                EditorGUILayout.HelpBox("Line renderer is not set.", MessageType.Warning);
            }
            else
            {
                AnimationType animationType = (AnimationType)EditorGUILayout.EnumPopup("Animation Type", animationElement.animationType);
                if(animationType != animationElement.animationType)
                {
                    animationElement.animationType = animationType;
                }

                if(GUILayout.Button("Add Animation Element"))
                {
                    animationElement.AddAnimation(animationType, AnimationType.Move, new MoveElement());
                    animationElement.AddAnimation(animationType, AnimationType.Width, new WidthElement());
                    animationElement.AddAnimation(animationType, AnimationType.ChangeRadius, new ChangeRadius());
                    animationElement.AddAnimation(animationType, AnimationType.Color, new ColorElement());

                }
                
                if(animationElement.animationElements.Count == 0)
                {
                    EditorGUILayout.HelpBox("Animation elements is empty.", MessageType.Warning);
                }


                if(foldoutStates[i] == null || foldoutStates[i].Count != animationElement.animationElements.Count)
                {
                    InitializeFoldoutListElements(i, animationElement.animationElements.Count);
                }
                

                for(int j = 0; j < animationElement.animationElements.Count; j++)
                {
                    EditorGUILayout.BeginVertical("box");

                    

                    foldoutStates[i][j] = EditorGUILayout.Foldout(foldoutStates[i][j], animationElement.animationElements[j].GetName());

                    if(foldoutStates[i][j])
                    {

                        animationElement.animationElements[j] = TuneAnimation(animationElement, animationElement.animationElements[j]);

                        animationElement.isAnimation = vectorAnimation.animationSequence[i].isAnimation;

                        if(GUILayout.Button("Remove Animation Element"))
                        {
                            animationElement.animationElements.RemoveAt(j);
                            break;
                        }
                    }

                    EditorGUILayout.EndVertical();

                }
            }
                

            if(GUILayout.Button("Play Animation"))
            {
                animationElement.isAnimation = true;
                vectorAnimation.Play();
            }

            if(GUILayout.Button("Remove Animation Sequence"))
            {
                vectorAnimation.animationSequence.RemoveAt(i);
                EditorGUILayout.EndVertical();
                break;
            }


            EditorGUILayout.EndVertical();

            vectorAnimation.animationSequence[i] = animationElement;


        }


        serializedObject.ApplyModifiedProperties();



        if(GUI.changed)
        {
            EditorUtility.SetDirty(vectorAnimation);
        }
        

    }

    private T TuneAnimation<T>(AnimationElement animationElement, T animationElementType) where T : IAnimationElement
    {
        if(animationElementType is MoveElement moveElement)
        {
            return (T)(IAnimationElement)TuneMoveAnimation(animationElement, moveElement);
        }
        if(animationElementType is WidthElement widthElement)
        {
            return (T)(IAnimationElement)TuneWidthAnimation(animationElement, widthElement);
        }
        if(animationElementType is ChangeRadius changeRadius)
        {
            return (T)(IAnimationElement)TuneChangeRadiusAnimation(animationElement, changeRadius);
        }
        if(animationElementType is ColorElement colorElement)
        {
            return (T)(IAnimationElement)TuneColorAnimation(animationElement, colorElement);
        }


        return animationElementType;

    }

    private MoveElement TuneMoveAnimation(AnimationElement animationElement, MoveElement moveElement)
    {
        if(moveElement.name == "")
        {
            moveElement.name = "New Move Element";
        }

        moveElement.name = EditorGUILayout.TextField("Name", moveElement.name);

        

        int point = EditorGUILayout.IntSlider("Point Index", moveElement.point, 0, animationElement.lineRenderer.positionCount - 1);

        moveElement.lineRenderer = animationElement.lineRenderer;
        moveElement.point = point;

        

        if(!animationElement.isAnimation)
        {
            moveElement.startPosition = animationElement.lineRenderer.GetPosition(point);
        }

        EditorGUILayout.Vector3Field("Start Position", moveElement.startPosition);
        moveElement.endPosition = EditorGUILayout.Vector3Field("End Position", moveElement.endPosition);
        EditorGUILayout.Vector3Field("Current Position", moveElement.currentPosition);

        moveElement.ease = (Ease)EditorGUILayout.EnumPopup("Animation Ease", moveElement.ease);

        moveElement.duration = EditorGUILayout.FloatField("Animation Duration", moveElement.duration);

        return moveElement;
    }
    private WidthElement TuneWidthAnimation(AnimationElement animationElement, WidthElement widthElement)
    {
        if(widthElement.name == "")
        {
            widthElement.name = "New Width Element";
        }

        widthElement.name = EditorGUILayout.TextField("Name", widthElement.name);

        widthElement.isConstant = EditorGUILayout.Toggle("Is Symetrical Width", widthElement.isConstant);
        widthElement.isAdding = EditorGUILayout.Toggle("Add width", widthElement.isAdding);
        widthElement.isSubtracting = EditorGUILayout.Toggle("Subtract width", widthElement.isSubtracting);

        widthElement.lineRenderer = animationElement.lineRenderer;

        if(widthElement.isConstant)
        {   
            widthElement.endWidthStart = EditorGUILayout.FloatField("End Width", widthElement.endWidthStart);
            widthElement.endWidthEnd = widthElement.endWidthStart;
        }
        else
        {
            widthElement.endWidthStart = EditorGUILayout.FloatField("End Width For Line Start", widthElement.endWidthStart);
            widthElement.endWidthEnd = EditorGUILayout.FloatField("End Width For Line End", widthElement.endWidthEnd);
        }

        if(!animationElement.isAnimation)
        {
            widthElement.startWidthStart = animationElement.lineRenderer.startWidth;
            widthElement.startWidthEnd = animationElement.lineRenderer.endWidth;
        }

        widthElement.ease = (Ease)EditorGUILayout.EnumPopup("Animation Ease", widthElement.ease);

        widthElement.duration = EditorGUILayout.FloatField("Animation Duration", widthElement.duration);



        return widthElement;
    }
    private ChangeRadius TuneChangeRadiusAnimation(AnimationElement animationElement, ChangeRadius changeRadius)
    {
        if(changeRadius.name == "")
        {
            changeRadius.name = "New ChangeRadius Element";
        }

        changeRadius.name = EditorGUILayout.TextField("Name", changeRadius.name);
        changeRadius.lineRenderer = animationElement.lineRenderer;
        changeRadius.vectorCircle = (VectorCircle)EditorGUILayout.ObjectField("Vector Circle", changeRadius.vectorCircle, typeof(VectorCircle), true);

        if(changeRadius.vectorCircle != null)
        {
            changeRadius.isAdding = EditorGUILayout.Toggle("Add radius", changeRadius.isAdding);
            changeRadius.isSubtracting = EditorGUILayout.Toggle("Subtract radius", changeRadius.isSubtracting);
            changeRadius.endRadius = EditorGUILayout.FloatField("End radius", changeRadius.endRadius);
            changeRadius.ease = (Ease)EditorGUILayout.EnumPopup("Animation Ease", changeRadius.ease);
            changeRadius.duration = EditorGUILayout.FloatField("Animation Duration", changeRadius.duration);
        }

        return changeRadius;


    }
    private ColorElement TuneColorAnimation(AnimationElement animationElement, ColorElement colorElement)
    {
        if(colorElement.name == "")
        {
            colorElement.name = "New Color Element";
        }

        colorElement.name = EditorGUILayout.TextField("Name", colorElement.name);

        colorElement.isConstant = EditorGUILayout.Toggle("Is Symetrical Color", colorElement.isConstant);

        colorElement.lineRenderer = animationElement.lineRenderer;

        if(colorElement.isConstant)
        {   
            colorElement.endColorStart = EditorGUILayout.ColorField("End Color", colorElement.endColorStart);
            colorElement.endColorEnd = colorElement.endColorStart;
        }
        else
        {
            colorElement.endColorStart = EditorGUILayout.ColorField("End Color For Line Start", colorElement.endColorStart);
            colorElement.endColorEnd = EditorGUILayout.ColorField("End Color For Line End", colorElement.endColorEnd);
        }


        colorElement.ease = (Ease)EditorGUILayout.EnumPopup("Animation Ease", colorElement.ease);

        colorElement.duration = EditorGUILayout.FloatField("Animation Duration", colorElement.duration);



        return colorElement;
    }

    private void InitializeFoldoutListSequence(VectorAnimation vectorAnimation)
    {
        foldoutStates = new List<List<bool>>();

        for(int i = 0; i < vectorAnimation.animationSequence.Count; i++)
        {
            foldoutStates.Add(new List<bool>());
        }
    }

    private void InitializeFoldoutListElements(int index, int capacity)
    {

        foldoutStates[index] = new List<bool>();

        for(int i = 0; i < capacity; i++)
        {
            foldoutStates[index].Add(false);
        }
    }
}
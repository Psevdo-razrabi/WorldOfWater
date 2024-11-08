using UnityEngine;
using UnityEditor;


public abstract class VectorElementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        VectorElement vectorElement = (VectorElement)target;

        vectorElement.elementSettings = (ElementSettings)EditorGUILayout.ObjectField("Element settings", vectorElement.elementSettings, typeof(ElementSettings), true);

        if(GUILayout.Button("Create line renderer"))
        {
            vectorElement.Draw(true);
        }

        vectorElement.fillEndCap = EditorGUILayout.Toggle("Fill end cap", vectorElement.fillEndCap);


        vectorElement.constantColor = EditorGUILayout.Toggle("Constant color", vectorElement.constantColor);
        if(vectorElement.constantColor)
        {
            vectorElement.color = EditorGUILayout.ColorField("Color", vectorElement.color);
        }
        else
        {
            vectorElement.color_start = EditorGUILayout.ColorField("Color start", vectorElement.color_start);
            vectorElement.color_end = EditorGUILayout.ColorField("Color end", vectorElement.color_end);
        }


        vectorElement.constantWidth = EditorGUILayout.Toggle("Constant width", vectorElement.constantWidth);
        if(vectorElement.constantWidth)
        {
            vectorElement.width = EditorGUILayout.FloatField("Width", vectorElement.width);
            if(vectorElement.width < 0) vectorElement.width = 0;
        }
        else
        {
            vectorElement.width_start = EditorGUILayout.FloatField("Width start", vectorElement.width_start);
            vectorElement.width_end = EditorGUILayout.FloatField("Width end", vectorElement.width_end);
        }


    }

    protected abstract void DrawCustomInspector();

    protected void HandleGUIChange()
    {
        if(GUI.changed)
        {
            EditorUtility.SetDirty(target);

            if(target is VectorElement vectorElement)
            {
                vectorElement.Draw(true);
            }
        }
    }
}

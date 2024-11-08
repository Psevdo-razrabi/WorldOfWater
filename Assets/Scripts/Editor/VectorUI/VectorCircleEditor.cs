using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(VectorCircle))]
public class VectorCircleEditor : VectorElementEditor
{
    protected override void DrawCustomInspector()
    {
        VectorCircle vectorCircle = (VectorCircle)target;

        vectorCircle.radius = EditorGUILayout.FloatField("Radius", vectorCircle.radius);

        vectorCircle.segments = EditorGUILayout.IntField("Resolution", vectorCircle.segments);
        if(vectorCircle.segments < 3) vectorCircle.segments = 3;

        vectorCircle.isArc = EditorGUILayout.Toggle("Arc", vectorCircle.isArc);
        if(vectorCircle.isArc)
        {
            vectorCircle.angle_start = EditorGUILayout.Slider("Start angle", vectorCircle.angle_start, -vectorCircle.angle_end, vectorCircle.angle_end);
            vectorCircle.angle_end = EditorGUILayout.Slider("End angle", vectorCircle.angle_end, vectorCircle.angle_start, 360);
        }

        EditorGUILayout.Space();

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawCustomInspector();
        HandleGUIChange();
    }
}

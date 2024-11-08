using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VectorLine))]
public class VectorLineEditor : VectorElementEditor
{
    protected override void DrawCustomInspector()
    {
        VectorLine vectorLine = (VectorLine)target;

        Vector3 newStart = EditorGUILayout.Vector3Field("Start position", vectorLine.linePos_start);
        Vector3 newEnd = EditorGUILayout.Vector3Field("End position", vectorLine.linePos_end);

        if(newStart != vectorLine.linePos_start || newEnd != vectorLine.linePos_end)
        {
            vectorLine.linePos_start = newStart;
            vectorLine.linePos_end = newEnd;
        }
        
        
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawCustomInspector();
        HandleGUIChange();
    }
}

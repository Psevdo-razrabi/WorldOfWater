using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreateGrid))]
public class CreateGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CreateGrid createGrid = (CreateGrid)target;

        DrawDefaultInspector();

        if(GUILayout.Button("Update"))
        {
            createGrid.GenerateGrid();
        }
    }
}

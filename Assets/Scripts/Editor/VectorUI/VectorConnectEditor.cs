using UnityEngine;
using UnityEditor;




[CustomEditor(typeof(VectorConnect))]

public class VectorConnectEditor : Editor
{

    public override void OnInspectorGUI()
    {
        VectorConnect vectorConnect = (VectorConnect)target;

        vectorConnect.lineRendererOrigin = (LineRenderer)EditorGUILayout.ObjectField("Line Renderer Origin", vectorConnect.lineRendererOrigin, typeof(LineRenderer), true);

        if(vectorConnect.lineRendererOrigin)
        {
            if(GUILayout.Button("Add Connection"))
            {
                vectorConnect.connections.Add(new Connection());
            }

            if(GUILayout.Button("Update"))
            {
                vectorConnect.UpdatePosition();
            }
        }



        for(int i = 0; i < vectorConnect.connections.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");

            float buttonWidth = 20f; // Desired width
            float buttonHeight = 20f; // Desired height

            GUI.color = new Color(0.7098f, 0.2824f, 0.2824f, 1f);

            // Create a custom button with specific size
            if (GUILayout.Button("X", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            {
                vectorConnect.connections.RemoveAt(i);
                continue;
            }

            GUI.color = Color.white;

            Connection connection = vectorConnect.connections[i];

            connection.lineRendererOrigin = vectorConnect.lineRendererOrigin;

            if(connection.lineRendererOrigin)
            {
                connection.pointOrigin = EditorGUILayout.IntSlider("Origin Point", connection.pointOrigin, 0, connection.lineRendererOrigin.positionCount - 1);
            }

            
            connection.lineRendererDestination = (LineRenderer)EditorGUILayout.ObjectField("Line Renderer Destination", connection.lineRendererDestination, typeof(LineRenderer), true);

            if(connection.lineRendererDestination)
            {
                connection.pointDest = EditorGUILayout.IntSlider("Destination Point", connection.pointDest, 0, connection.lineRendererDestination.positionCount - 1);
            }


            vectorConnect.connections[i] = connection;


            EditorGUILayout.EndVertical();
            
            GUILayout.Space(5);

        }



        if(GUI.changed)
        {
            EditorUtility.SetDirty(vectorConnect);
            vectorConnect.UpdatePosition();
        }


    }
}

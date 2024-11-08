using System;
using System.Collections.Generic;
using UnityEngine;

public class VectorCircle : VectorElement
{
    [Header("Circle Properties")]
    public float radius = 100;
    public bool isArc;
    public float angle_start;
    public float angle_end = 360;
    public int segments = 100;
    
    public override void SetElementSettings(bool setColor)
    {
        base.SetElementSettings(setColor);

        lineRenderer.positionCount = segments + 1;

        float angle = 0f;

        if(isArc)
        {
            angle = angle_start;
        }

        lineRenderer.loop = !isArc;

        for (int i = 0; i <= segments; i++)
        {

            angle += isArc ? (angle_end - angle_start) / segments : 360f / segments;

            float rad = angle * Mathf.Deg2Rad;
            float x = Mathf.Cos(rad) * radius;
            float y = Mathf.Sin(rad) * radius;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
        
    }



}

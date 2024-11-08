using System;
using UnityEngine;

public class VectorLine : VectorElement
{
    [Header("Line Properties")]
    public Vector3 linePos_start = Vector3.zero;
    [SerializeReference]
    public Vector3 linePos_end = Vector3.right;


    public override void SetElementSettings(bool setColor)
    {
        base.SetElementSettings(setColor);
        
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, linePos_start);
        lineRenderer.SetPosition(1, linePos_end);

    }
}

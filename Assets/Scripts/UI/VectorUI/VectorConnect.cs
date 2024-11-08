using System;
using System.Collections.Generic;
using UnityEngine;


public class VectorConnect : MonoBehaviour
{
    [SerializeReference] public List<Connection> connections = new List<Connection>();
    public LineRenderer lineRendererOrigin;



    public void UpdatePosition()
    {
        foreach(Connection c in connections)
        {
            c.UpdatePoint();
        }
    }
    
}


[Serializable]
public class Connection
{
    public LineRenderer lineRendererOrigin;
    public int pointOrigin;

    public LineRenderer lineRendererDestination;
    public int pointDest;

    public void UpdatePoint()
    {
        if(lineRendererDestination && lineRendererOrigin)
        {
            lineRendererOrigin.SetPosition(pointOrigin, lineRendererDestination.GetPosition(pointDest));
        }
    }

}

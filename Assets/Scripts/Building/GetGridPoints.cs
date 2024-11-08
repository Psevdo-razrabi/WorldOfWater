using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GetGridPoints : MonoBehaviour
{
    [SerializeField] int requiredPoints;
    public bool canBuild;
    public List<GridPiece> points = new List<GridPiece>();
    List<GameObject> collidingObjects = new List<GameObject>();
    Vector3 previousPos = Vector3.zero;
    Quaternion previousRotation = Quaternion.identity;
    [SerializeField] LayerMask layerMask;


    void OnTriggerEnter(Collider collision)
    {
        if(previousPos != transform.position || previousRotation != transform.rotation)
        {
            canBuild = false;
            points.Clear();
            collidingObjects.Clear();
            previousPos = transform.position;
            previousRotation = transform.rotation;
        }
        if ((layerMask.value & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            if(!collidingObjects.Contains(collision.gameObject))
            {
                points.Add(collision.gameObject.GetComponent<GridPointData>().connectedGridPiece);
                collidingObjects.Add(collision.gameObject);
                if(points.Count == requiredPoints)
                {
                    bool isEmpty = true;
                    foreach(GridPiece piece in points)
                        if(!piece.isEmpty) isEmpty = false;
                    canBuild = isEmpty;
                }
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {

        if(collidingObjects.Contains(collision.gameObject))
        {
            collidingObjects.Remove(collision.gameObject);
        }
    } 
}

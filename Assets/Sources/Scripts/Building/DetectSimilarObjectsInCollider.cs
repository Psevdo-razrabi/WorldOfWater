using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class DetectSimilarObjectsInCollider : MonoBehaviour
{
    [SerializeField] public LayerMask layerMask;
    [SerializeField] public LayerMask floorLayer;
    public bool isDetected;
    int collidingObjectsOnLayerCount;
    [SerializeField] List<GameObject> collidingObjects = new List<GameObject>();

    public GameObject collidingObject;

    

    public bool OnFloorCheck()
    {
        Collider collider = GetComponent<Collider>();
        if(collider == null)
        {
            Debug.LogError("No collider found on the object " + gameObject.name);
            return false;
        }

        Bounds bounds = collider.bounds;

        Vector3[] corners = new Vector3[4];

        corners[0] = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z); // Bottom-left
        corners[1] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z); // Bottom-right
        corners[2] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z); // Top-left
        corners[3] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z); // Top-right

        foreach (Vector3 corner in corners)
        {
            if (!IsPointOnFloor(corner))
            {
                return false;
            }
        }

        return true;
    }


    bool IsPointOnFloor(Vector3 point)
    {
        RaycastHit hit;
        if (Physics.Raycast(point + Vector3.up * 0.1f, Vector3.down, out hit, 1f, floorLayer))
        {
            return true;
        }
        return false;
    }

    void OnTriggerStay(Collider collision)
    {
        if ((layerMask.value & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            isDetected = true;
            collidingObject = collision.gameObject;
        }
    }



    void OnTriggerExit(Collider collision)
    {
        if ((layerMask.value & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            ClearList();
        }
    }

    public void ClearList()
    {
        isDetected = false;
    }
}

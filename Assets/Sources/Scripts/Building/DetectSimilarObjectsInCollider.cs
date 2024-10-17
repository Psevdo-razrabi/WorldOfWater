using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectSimilarObjectsInCollider : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    public bool isDetected;
    int collidingObjectsOnLayerCount;
    [SerializeField] List<GameObject> collidingObjects = new List<GameObject>();

    void OnTriggerEnter(Collider collision)
    {
        collidingObjects.Add(collision.gameObject);
        if ((layerMask.value & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            isDetected = true;
            collidingObjectsOnLayerCount++;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if(collidingObjects.Contains(collision.gameObject))
        {
            collidingObjects.Remove(collision.gameObject);
        }
        if (((1 << collision.gameObject.layer) & layerMask) != 0)
        {
            collidingObjectsOnLayerCount--;
            if(collidingObjectsOnLayerCount == 0)
            {
                isDetected = false;
            }
        }
    }
}

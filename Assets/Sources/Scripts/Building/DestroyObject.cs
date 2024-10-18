using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [Header("Raycast set")]
    [SerializeField] float rayDistance;
    [SerializeField] LayerMask layerMaskForRaycast;
    [SerializeField] PlatformsController platformsController;



    public void Destroy()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, layerMaskForRaycast))
        {

            Destroyable destroyable = hit.collider.gameObject.GetComponent<Destroyable>();
            if(destroyable != null)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    platformsController.UpdatePlatformCount(destroyable.gameObject);
                    destroyable.Destroy();
                }
            }
        }
    }
}

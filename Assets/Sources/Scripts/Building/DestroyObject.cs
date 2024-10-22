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
            ObjectContainer objectContainer = hit.collider.gameObject.GetComponent<ObjectContainer>();
            if(objectContainer != null && objectContainer.objects.Count != 0)
            {
                return;
            }
            CreateGrid createGrid = hit.collider.gameObject.GetComponent<CreateGrid>();
            Destroyable destroyable = hit.collider.gameObject.GetComponent<Destroyable>();
            if(destroyable != null)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    if(createGrid != null && !createGrid.haveNextFloor)
                    {
                        foreach(GridPiece piece in createGrid.wallsHoldingSecondFloor)
                        {
                            piece.isHoldingSecondFloor = false;
                        }
                    }
                    if(destroyable.attachedGridPiece[0].isHoldingSecondFloor)
                    {
                        return;
                    }
                    if(destroyable.objectContainer != null)
                    {
                        destroyable.objectContainer.RemoveObject(destroyable.gameObject);
                    }
                    platformsController.UpdatePlatformCount(destroyable.gameObject);
                    destroyable.Destroy();
                }
            }
        }
    }
}

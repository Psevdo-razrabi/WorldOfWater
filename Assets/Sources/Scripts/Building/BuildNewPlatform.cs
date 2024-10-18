using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildNewPlatform : MonoBehaviour
{
    [Header("Raycast to place new platform set")]
    [SerializeField] float rayDistance;
    [SerializeField] LayerMask layerMaskForRaycast;
    [Header("Creating new platform set")]
    [SerializeField] GameObject plotPrefab;
    [SerializeField] GameObject platformsHolder;
    [SerializeField] PlatformsController platformsController;
    [SerializeField] float yOffset;

    [Header("Platform preview set")]
    [SerializeField] GameObject platformPreview;
    [SerializeField] Color colorCanPlace, colorCantPlace;
    DetectSimilarObjectsInCollider detectSimilarObjectsInCollider;
    Material platformPreview_Mat;
    
    void Start()
    {
        GetDataFromPreviewObj();
    }

    void GetDataFromPreviewObj()
    {
        detectSimilarObjectsInCollider = platformPreview.GetComponent<DetectSimilarObjectsInCollider>();
        platformPreview_Mat = platformPreview.GetComponent<MeshRenderer>().material;
    }

    public void Build(CreateGrid closestPlatformToPlayer)
    {

        platformPreview.SetActive(true);

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, layerMaskForRaycast))
        {
            platformPreview_Mat.color = colorCanPlace;
            platformPreview.transform.position = new Vector3(hit.point.x, hit.point.y + yOffset, hit.point.z);

            if(closestPlatformToPlayer != null)
            {
                SnapPlatform(closestPlatformToPlayer);
            }

            if(detectSimilarObjectsInCollider.isDetected)
            {
                platformPreview_Mat.color = colorCantPlace;
                return;
            }
            if(Input.GetMouseButtonDown(0))
            {
                GameObject newPlot = Instantiate(plotPrefab, new Vector3(platformPreview.transform.position.x, platformPreview.transform.position.y, platformPreview.transform.position.z), Quaternion.identity);

                newPlot.transform.parent = platformsHolder.transform;
            }
        }

        platformsController.UpdatePlatformCount();
    }

    public void CancelBuild()
    {
        platformPreview.SetActive(false);
        detectSimilarObjectsInCollider.ClearList();
    }




    void SnapPlatform(CreateGrid platform)
    {
        Vector3 newPlatformPos = Vector3.zero;
        Vector3 camDirection = Camera.main.transform.forward;
        camDirection.y = 0;
        camDirection.Normalize();

        if (Mathf.Abs(camDirection.x) > Mathf.Abs(camDirection.z))
        {
            if (camDirection.x > 0)
            {
                newPlatformPos.x = platform.sizeOfObject; // Move to the right
            }
            else if (camDirection.x < 0)
            {
                newPlatformPos.x = -platform.sizeOfObject; // Move to the left
            }
        }
        else {
            if (camDirection.z > 0)
            {
                newPlatformPos.z = platform.sizeOfObject; // Move forward
            }
            else if (camDirection.z < 0)
            {
                newPlatformPos.z = -platform.sizeOfObject; // Move backward
            }
        }

        platformPreview.transform.position = new Vector3(platform.transform.position.x + newPlatformPos.x, platform.transform.position.y, platform.transform.position.z + newPlatformPos.z);
    }

}

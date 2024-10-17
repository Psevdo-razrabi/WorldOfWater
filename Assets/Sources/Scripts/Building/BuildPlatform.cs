using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BuildPlatform : MonoBehaviour
{
    [SerializeField] float rayDistance;
    [SerializeField] GameObject plotPrefab;
    [SerializeField] GameObject platformPreview;
    [SerializeField] GameObject platformsHolder;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float yOffset;
    [SerializeField] BuildingGrid buildingGrid;

    [SerializeField] List<GameObject> platforms = new List<GameObject>();
    [SerializeField] bool snapToPlatform;
    [SerializeField] bool isBuilding;
    [SerializeField] bool isBuildingPlatform;
    DetectSimilarObjectsInCollider detectSimilarObjectsInCollider;

    void Awake()
    {
        buildingGrid.OnPlatformsCountUpdate += UpdatePlatformCount;
        buildingGrid.OnBuildModeChange += ChangeBuildMode;

        detectSimilarObjectsInCollider = platformPreview.GetComponent<DetectSimilarObjectsInCollider>();
    }
    void Update()
    {
        if(isBuilding && isBuildingPlatform)
        {
            Build();
        }

        if(Input.GetKeyDown(KeyCode.N) && isBuilding)
        {
            isBuildingPlatform = !isBuildingPlatform;
            if(!isBuildingPlatform)
            {
                platformPreview.SetActive(false);
            }
        }

    }

    void Build()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, layerMask))
        {
            platformPreview.SetActive(true);
            platformPreview.transform.position = new Vector3(hit.point.x, hit.point.y + yOffset, hit.point.z);


            // CreateGrid closestPlatformToCursor = FindClosestPlatformToCursor();
            CreateGrid closestPlatformToCursor = buildingGrid.FindClosestPlatform(gameObject);
            if(closestPlatformToCursor != null)
            {
                SnapPlatform(closestPlatformToCursor);
            }

            
            if(Input.GetMouseButtonDown(0))
            {
                if(detectSimilarObjectsInCollider.isDetected)
                {
                    Debug.Log("cant build here!");
                    return;
                }

                GameObject newPlot = Instantiate(plotPrefab, new Vector3(platformPreview.transform.position.x, platformPreview.transform.position.y, platformPreview.transform.position.z), Quaternion.identity);
                newPlot.transform.parent = platformsHolder.transform;
                
            }
        }
    }
    void UpdatePlatformCount()
    {
        platforms = buildingGrid.platforms;
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

    void ChangeBuildMode(bool isBuilding)
    {
        this.isBuilding = isBuilding;
        if(!isBuilding)
        {
            platformPreview.SetActive(false);
        }
    }




}

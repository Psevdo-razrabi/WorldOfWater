using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BuildPlatform : MonoBehaviour
{
    [Header("Raycast to place new platform set")]
    [SerializeField] float rayDistance;
    [SerializeField] LayerMask layerMaskForRaycast;

    [Header("Platform preview set")]
    [SerializeField] GameObject platformPreview;
    [SerializeField] Color colorCanPlace, colorCantPlace;
    [Header("Creating new platform set")]
    [SerializeField] GameObject plotPrefab;
    [SerializeField] GameObject platformsHolder;
    [SerializeField] float yOffset;
    [Header("Dependent variables")]
    [SerializeField] BuildingGrid buildingGrid;
    [SerializeField] List<GameObject> platforms = new List<GameObject>();
    
    [Tooltip("НЕ МЕНЯТЬ!")]
    [SerializeField] bool isBuilding;
    [Tooltip("НЕ МЕНЯТЬ!")]
    [SerializeField] bool isBuildingPlatform;
    DetectSimilarObjectsInCollider detectSimilarObjectsInCollider;
    Material platformPreview_Mat;

    void Awake()
    {
        buildingGrid.OnPlatformsCountUpdate += UpdatePlatformCount;
        buildingGrid.OnBuildModeChange += ChangeBuildMode;

        detectSimilarObjectsInCollider = platformPreview.GetComponent<DetectSimilarObjectsInCollider>();

        platformPreview_Mat = platformPreview.GetComponent<MeshRenderer>().material;
        platformPreview_Mat.color = colorCanPlace;
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
                detectSimilarObjectsInCollider.ClearList();
            }
            else
            {
                platformPreview.SetActive(true);

            }
        }

    }

    void Build()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, layerMaskForRaycast))
        {
            platformPreview_Mat.color = colorCanPlace;
            platformPreview.transform.position = new Vector3(hit.point.x, hit.point.y + yOffset, hit.point.z);


            CreateGrid closestPlatformToCursor = buildingGrid.FindClosestPlatform(gameObject);
            if(closestPlatformToCursor != null)
            {
                SnapPlatform(closestPlatformToCursor);
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
            detectSimilarObjectsInCollider.ClearList();
            isBuildingPlatform = false;
        }
    }




}

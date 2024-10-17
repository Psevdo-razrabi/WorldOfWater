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
    [SerializeField] GameObject tempSphere;
    [SerializeField] BuildingGrid buildingGrid;

    [SerializeField] List<GameObject> platforms = new List<GameObject>();
    [SerializeField] bool isBuilding;
    [SerializeField] bool isBuildingPlatform;
    DetectSimilarObjectsInCollider detectSimilarObjectsInCollider;

    void Start()
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

        if(Input.GetKeyDown(KeyCode.N))
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
            tempSphere.transform.position = hit.point;
            platformPreview.transform.position = new Vector3(hit.point.x, hit.point.y + yOffset, hit.point.z);

            if(detectSimilarObjectsInCollider.isDetected)
            {
                Debug.Log("Can't build platform!");
                return;
            }


            if(Input.GetMouseButtonDown(0))
            {
                GameObject newPlot = Instantiate(plotPrefab, new Vector3(hit.point.x, hit.point.y + yOffset, hit.point.z), Quaternion.identity);
                newPlot.transform.parent = platformsHolder.transform;
                
            }
        }
    }
    void UpdatePlatformCount()
    {
        platforms = buildingGrid.platforms;
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

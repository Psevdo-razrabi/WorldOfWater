using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BuildNewBuilding : MonoBehaviour
{
    [Header("Buildings preview set")]
    [SerializeField] GameObject[] buildingsPrefab;
    [SerializeField] GameObject objectPool;
    [SerializeField] Material newMaterial;
    [SerializeField] Color colorCanBuild, colorCantBuild;
    [SerializeField] int angleIncrement;
    [SerializeField] LayerMask layerMaskForBaking, floorLayerForBaking;
    [Header("Raycast set")]
    [SerializeField] float rayDistance;
    [SerializeField] LayerMask layerMaskForRaycast;
    [Header("Animation set")]
    [SerializeField] bool isAnimate;
    [SerializeField] float animationSpeed;


    [SerializeField] List<GameObject> buildingsPreview = new List<GameObject>();
    int selectedBuildingObject;

    [SerializeField] GameObject currentBuildingPreview;


    void Start()
    {
        BakeWallsToPreview();
    }

    void BakeWallsToPreview()
    {
        for(int i = 0; i < buildingsPrefab.Length; i++)
        {
            GameObject newPreviewObject = Instantiate(buildingsPrefab[i], Vector3.zero, Quaternion.identity);
            newPreviewObject.SetActive(false);
            newPreviewObject.transform.SetParent(objectPool.transform);
            PreviewObject previewObject = newPreviewObject.AddComponent<PreviewObject>();
            previewObject.newMaterial = newMaterial;
            previewObject.colorCanBuild = colorCanBuild;
            previewObject.colorCantBuild = colorCantBuild;
            previewObject.BakePrefab();
            Rigidbody rigidbody = newPreviewObject.AddComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.useGravity = false;
            rigidbody.freezeRotation = true;
            BoxCollider boxCollider = newPreviewObject.GetComponent<BoxCollider>();
            if(boxCollider != null)
            {
                boxCollider.isTrigger = true;
            }
            DetectSimilarObjectsInCollider detectSimilarObjectsInCollider = newPreviewObject.AddComponent<DetectSimilarObjectsInCollider>();
            detectSimilarObjectsInCollider.layerMask = layerMaskForBaking;
            detectSimilarObjectsInCollider.floorLayer = floorLayerForBaking;
            buildingsPreview.Add(newPreviewObject);
        }
    }



    public void Build(CreateGrid closestPlatformToPlayer)
    {
        ScrollBuildingsWithMouseWheel();

        if(currentBuildingPreview != null && currentBuildingPreview != buildingsPreview[selectedBuildingObject])
            currentBuildingPreview.SetActive(false);
        currentBuildingPreview = buildingsPreview[selectedBuildingObject];
        currentBuildingPreview.SetActive(true);

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, layerMaskForRaycast))
        {
            currentBuildingPreview.transform.position = hit.point;
            RotateSelectedBuildingWithMouse(currentBuildingPreview.transform);

            if(!currentBuildingPreview.GetComponent<DetectSimilarObjectsInCollider>().isDetected && currentBuildingPreview.GetComponent<DetectSimilarObjectsInCollider>().OnFloorCheck())
            {
                currentBuildingPreview.GetComponent<PreviewObject>().CanBuild();
                if(Input.GetMouseButtonDown(0))
                {
                    GameObject newBuilding = Instantiate(buildingsPrefab[selectedBuildingObject], currentBuildingPreview.transform.position, currentBuildingPreview.transform.rotation);
                    newBuilding.transform.SetParent(closestPlatformToPlayer.transform);
                    closestPlatformToPlayer.GetComponent<ObjectContainer>().AddObject(newBuilding);
                    newBuilding.GetComponent<Destroyable>().objectContainer = closestPlatformToPlayer.GetComponent<ObjectContainer>();
                    if(isAnimate)
                    {
                        AnimateSpawn(newBuilding);
                    }
                }
            }
            else
            {
                currentBuildingPreview.GetComponent<PreviewObject>().CantBuild();
            }
        }
    }

    void AnimateSpawn(GameObject obj)
    {
        Vector3 initScale = obj.transform.localScale;
        obj.transform.localScale = initScale / 2;
        obj.transform.DOScale(initScale, animationSpeed).SetEase(Ease.OutBack);
    }

    public void CancelBuild()
    {
        currentBuildingPreview.SetActive(false);
        currentBuildingPreview.GetComponent<DetectSimilarObjectsInCollider>().ClearList();
    }


    

    // temporary
    void ScrollBuildingsWithMouseWheel()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if(!Input.GetKey(KeyCode.LeftControl)) return;
        if(scrollInput > 0f)
        {
            selectedBuildingObject++;
        }
        if(scrollInput < 0f)
        {
            selectedBuildingObject--;
        }

        if(selectedBuildingObject < 0)
        {
            selectedBuildingObject = buildingsPrefab.Length - 1;
        }
        else if(selectedBuildingObject > buildingsPrefab.Length - 1)
        {
            selectedBuildingObject = 0;
        }
    }


    void RotateSelectedBuildingWithMouse(Transform rotateTransform)
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if(Input.GetKey(KeyCode.LeftControl)) return;
        if(scrollInput > 0f)
        {
            rotateTransform.rotation = Quaternion.Euler(0, rotateTransform.rotation.eulerAngles.y + angleIncrement, 0);
        }
        if(scrollInput < 0f)
        {
            rotateTransform.rotation = Quaternion.Euler(0, rotateTransform.rotation.eulerAngles.y - angleIncrement, 0);
        }
    }
}

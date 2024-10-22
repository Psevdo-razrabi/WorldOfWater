using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class BuildNewWall : MonoBehaviour
{
    [Header("Walls preview set")]
    [SerializeField] GameObject[] wallsPrefab;
    [SerializeField] GameObject objectPool;
    [SerializeField] Material newMaterial;
    [SerializeField] Color colorCanBuild, colorCantBuild;
    [SerializeField] LayerMask layerMaskForBaking, floorLayerForBaking;


    [Header("Raycast set")]
    [SerializeField] float rayDistance;
    [SerializeField] LayerMask layerMaskForRaycast;
    [Header("Animation set")]
    [SerializeField] bool isAnimate;
    [SerializeField] float animationSpeed;



    [SerializeField] List<GameObject> wallsPreview = new List<GameObject>();
    int selectedWallObject;
    List<CreateGrid> closestPlatformToPlayer = new List<CreateGrid>();

    [SerializeField] GameObject currentWallPreview;

    void Start()
    {
        BakeWallsToPreview();
    }

    void BakeWallsToPreview()
    {
        for(int i = 0; i < wallsPrefab.Length; i++)
        {
            GameObject newPreviewObject = Instantiate(wallsPrefab[i], Vector3.zero, Quaternion.identity);
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
            BoxCollider[] boxColliders = previewObject.GetComponents<BoxCollider>();
            foreach(BoxCollider boxCollider in boxColliders)
            {
                boxCollider.isTrigger = true;
            }
            DetectSimilarObjectsInCollider detectSimilarObjectsInCollider = newPreviewObject.AddComponent<DetectSimilarObjectsInCollider>();
            detectSimilarObjectsInCollider.layerMask = layerMaskForBaking;
            detectSimilarObjectsInCollider.floorLayer = floorLayerForBaking;
            wallsPreview.Add(newPreviewObject);
        }
    }

    public void Build(List<CreateGrid> closestPlatformToPlayer)
    {
        if(this.closestPlatformToPlayer != null && this.closestPlatformToPlayer != closestPlatformToPlayer)
        {
            foreach(CreateGrid platform in this.closestPlatformToPlayer)
                platform.ExitBuildMode();
        }
        this.closestPlatformToPlayer = closestPlatformToPlayer;

        foreach(CreateGrid platform in closestPlatformToPlayer)
            platform.EnterBuildMode();

        ScrollWallsWithMouseWheel();


        if(currentWallPreview != null && currentWallPreview != wallsPrefab[selectedWallObject])
            currentWallPreview.SetActive(false);
        currentWallPreview = wallsPreview[selectedWallObject];
        currentWallPreview.SetActive(true);

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, layerMaskForRaycast))
        {
            float minDistance = float.MaxValue; 
            GridPiece closestGridPiece = null;
            CreateGrid closestPlatformToCursor = closestPlatformToPlayer[0];
            foreach(CreateGrid platform in closestPlatformToPlayer)
            {
                float distance = Vector3.Distance(hit.point, platform.GetClosestPoint(hit.point).center);
                if(distance < minDistance)
                {
                    minDistance = distance;
                    closestGridPiece = platform.GetClosestPoint(hit.point);
                    closestPlatformToCursor = platform;
                }
            }

            if(currentWallPreview.transform.position != closestGridPiece.center)
            {
                currentWallPreview.transform.position = closestGridPiece.center;
                currentWallPreview.GetComponent<DetectSimilarObjectsInCollider>().ClearList();
            }
            RotateSelectedWallWithMouse(currentWallPreview.transform);

            if(closestGridPiece.isEmpty && !currentWallPreview.GetComponent<DetectSimilarObjectsInCollider>().isDetected && CanBuildMultiCellBuilding())
            {
                currentWallPreview.GetComponent<PreviewObject>().CanBuild();
                if(Input.GetMouseButtonDown(0))
                {
                    GameObject newWall = Instantiate(wallsPrefab[selectedWallObject], currentWallPreview.transform.position, currentWallPreview.transform.rotation);
                    Destroyable destroyable = newWall.GetComponent<Destroyable>();
                    newWall.transform.SetParent(closestPlatformToCursor.transform);

                    GetGridPoints getGridPoints = currentWallPreview.GetComponent<GetGridPoints>();
                    if(getGridPoints)
                    {
                        destroyable.attachedGridPiece.Clear();
                        foreach(GridPiece gridPiece in getGridPoints.points)
                        {
                            gridPiece.isEmpty = false;
                            destroyable.attachedGridPiece.Add(gridPiece);
                        }
                    }
                    else
                    {
                        destroyable.attachedGridPiece.Add(closestGridPiece);
                        closestGridPiece.isEmpty = false;
                    }
                    closestPlatformToCursor.GetComponent<ObjectContainer>().AddObject(newWall);
                    destroyable.objectContainer = closestPlatformToCursor.GetComponent<ObjectContainer>();
                    if(isAnimate)
                    {
                        AnimateSpawn(newWall);
                    }
                }

            }
            else
            {
                currentWallPreview.GetComponent<PreviewObject>().CantBuild();
            }

        }
        else
        {
            currentWallPreview.SetActive(false);
        }
    }

    bool CanBuildMultiCellBuilding()
    {
        GetGridPoints getGridPoints = currentWallPreview.GetComponent<GetGridPoints>();
        if(getGridPoints != null)
        {
            return getGridPoints.canBuild;
        }
        else
        {
            return true;
        }
    }

    void AnimateSpawn(GameObject obj)
    {
        Vector3 initScale = obj.transform.localScale;
        obj.transform.localScale = initScale / 2;
        obj.transform.DOScale(initScale, animationSpeed).SetEase(Ease.OutBack);
    }


    // temporary
    void ScrollWallsWithMouseWheel()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if(!Input.GetKey(KeyCode.LeftControl)) return;
        if(scrollInput > 0f)
        {
            selectedWallObject++;
        }
        if(scrollInput < 0f)
        {
            selectedWallObject--;
        }

        if(selectedWallObject < 0)
        {
            selectedWallObject = wallsPrefab.Length - 1;
        }
        else if(selectedWallObject > wallsPrefab.Length - 1)
        {
            selectedWallObject = 0;
        }
    }


    void RotateSelectedWallWithMouse(Transform rotateTransform)
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if(Input.GetKey(KeyCode.LeftControl)) return;
        if(scrollInput > 0f)
        {
            rotateTransform.rotation = Quaternion.Euler(0, rotateTransform.rotation.eulerAngles.y + 90, 0);
        }
        if(scrollInput < 0f)
        {
            rotateTransform.rotation = Quaternion.Euler(0, rotateTransform.rotation.eulerAngles.y - 90, 0);
        }
        if(scrollInput != 0)
        {
            currentWallPreview.GetComponent<DetectSimilarObjectsInCollider>().ClearList();
        }
    }
    public void CancelBuild()
    {
        currentWallPreview.SetActive(false);
        currentWallPreview.GetComponent<DetectSimilarObjectsInCollider>().ClearList();
        foreach(CreateGrid platform in this.closestPlatformToPlayer)
            platform.ExitBuildMode();
    }
}

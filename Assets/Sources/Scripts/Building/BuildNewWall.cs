using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BuildNewWall : Build, IBuild
{
    private List<CreateGrid> closestPlatformsToPlayer = new List<CreateGrid>();


    private void Start()
    {
        Bake(bakeParams);
    }

    public void Build(BuildParams buildParams)
    {
        UpdatePreviewPositionAndRotation();
        List<CreateGrid> currentClosestPlatforms = buildParams.ClosestPlatformsToPlayer;

        HandlePlatformExit(currentClosestPlatforms);
        if (currentClosestPlatforms == null)
        {
            HandleNoPlatforms();
            return;
        }

        HandlePlatformEntry(currentClosestPlatforms);
        UpdateCurrentPreview();

        if (TryGetRaycastHit(out RaycastHit hit))
        {
            HandleRaycastHit(hit, currentClosestPlatforms);
        }
        else
        {
            HandleNoHit();
        }
    }

    private void HandlePlatformExit(List<CreateGrid> currentClosestPlatforms)
    {
        if(closestPlatformsToPlayer != null && closestPlatformsToPlayer != currentClosestPlatforms)
        {
            foreach(CreateGrid platform in closestPlatformsToPlayer)
            {
                platform.ExitBuildMode();
            }
        }
        closestPlatformsToPlayer = currentClosestPlatforms;
    }

    private void HandlePlatformEntry(List<CreateGrid> currentClosestPlatforms)
    {
        foreach(CreateGrid platform in currentClosestPlatforms)
        {
            platform.EnterBuildMode();
        }
    }

    private void HandleNoPlatforms()
    {
        if(currentPreview != null)
        {
            currentPreview.SetActive(false);
        }
        Crosshair.Instance.SetState(Crosshair.State.CantInteract);
    }

    private void UpdateCurrentPreview()
    {
        if (currentPreview != null && currentPreview != bakeParams.prefabs[selectedObject])
        {
            currentPreview.SetActive(false);
        }
        currentPreview = previews[selectedObject];
        currentPreview.SetActive(true);
    }

    

    private void HandleRaycastHit(RaycastHit hit, List<CreateGrid> currentClosestPlatforms)
    {
        GridPiece closestGridPiece = GetClosestGridPiece(hit, currentClosestPlatforms);


        if (currentPreview.transform.position != closestGridPiece.center)
        {
            currentPreview.transform.position = closestGridPiece.center;
            currentPreview.GetComponent<DetectSimilarObjectsInCollider>().ClearList();
        }

        if (CanBuildAtPosition(closestGridPiece))
        {
            HandleCanBuild(closestGridPiece, currentClosestPlatforms);
        }
        else
        {
            HandleCantBuild();
        }
    }

    private GridPiece GetClosestGridPiece(RaycastHit hit, List<CreateGrid> currentClosestPlatforms)
    {
        float minDistance = float.MaxValue;
        GridPiece closestGridPiece = null;

        foreach (CreateGrid platform in currentClosestPlatforms)
        {
            float distance = Vector3.Distance(hit.point, platform.GetClosestPoint(hit.point).center);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestGridPiece = platform.GetClosestPoint(hit.point);
            }
        }

        return closestGridPiece;
    }

    private bool CanBuildAtPosition(GridPiece closestGridPiece)
    {
        return closestGridPiece.isEmpty && 
               !currentPreview.GetComponent<DetectSimilarObjectsInCollider>().isDetected && 
               CanBuildMultiCellBuilding();
    }

    private void HandleCanBuild(GridPiece closestGridPiece, List<CreateGrid> currentClosestPlatforms)
    {
        currentPreview.GetComponent<PreviewObject>().CanBuild();
        Crosshair.Instance.SetState(Crosshair.State.CanInteract);

        if (Input.GetMouseButtonDown(0))
        {
            InstantiateWall(closestGridPiece, currentClosestPlatforms);
        }
    }

    private void InstantiateWall(GridPiece closestGridPiece, List<CreateGrid> currentClosestPlatforms)
    {
        GameObject newWall = Instantiate(bakeParams.prefabs[selectedObject], currentPreview.transform.position, currentPreview.transform.rotation);
        Destroyable destroyable = newWall.GetComponent<Destroyable>();

        GetGridPoints getGridPoints = currentPreview.GetComponent<GetGridPoints>();
        if (getGridPoints)
        {
            destroyable.attachedGridPiece.Clear();
            foreach (GridPiece gridPiece in getGridPoints.points)
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

        CreateGrid closestPlatformToCursor = currentClosestPlatforms[0];
        closestPlatformToCursor.GetComponent<ObjectContainer>().AddObject(newWall);
        destroyable.objectContainer = closestPlatformToCursor.GetComponent<ObjectContainer>();
        GridPointsAnimation.Instance.SetPlatformsAndStart(currentClosestPlatforms, closestGridPiece);

        if (isAnimate)
        {
            AnimateSpawn(newWall);
        }
    }

    private void HandleCantBuild()
    {
        currentPreview.GetComponent<PreviewObject>().CantBuild();
        Crosshair.Instance.SetState(Crosshair.State.CantInteract);
    }

    private void HandleNoHit()
    {
        currentPreview.SetActive(false);
        Crosshair.Instance.SetState(Crosshair.State.CantInteract);
    }

    private bool CanBuildMultiCellBuilding()
    {
        GetGridPoints getGridPoints = currentPreview.GetComponent<GetGridPoints>();
        return getGridPoints == null || getGridPoints.canBuild;
    }

    private void AnimateSpawn(GameObject obj)
    {
        Vector3 initScale = obj.transform.localScale;
        obj.transform.localScale = initScale / 2;
        obj.transform.DOScale(initScale, animationSpeed).SetEase(Ease.OutBack);
    }

    public override void CancelBuild()
    {
        if (closestPlatformsToPlayer != null)
        {
            if (currentPreview != null)
            {
                currentPreview.SetActive(false);
                currentPreview.GetComponent<DetectSimilarObjectsInCollider>().ClearList();
            }

            foreach (CreateGrid platform in closestPlatformsToPlayer)
            {
                platform.ExitBuildMode();
            }

            selectedObject = 0;
        }

        Crosshair.Instance.SetState(Crosshair.State.Default);
    }

    public override void UpdatePreviewPositionAndRotation()
    {
        if(currentPreview != null)
        {
            RotatePreviewWithMouse(currentPreview.transform);
        }
    }
}

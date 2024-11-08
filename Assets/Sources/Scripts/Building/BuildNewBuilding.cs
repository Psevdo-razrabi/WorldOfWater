using UnityEngine;
using DG.Tweening;

public class BuildNewBuilding : Build, IBuild
{
    void Start()
    {
        BakeWalls();
    }

    void BakeWalls()
    {
        Bake(bakeParams);
    }
    public void Build(BuildParams buildParams)
    {
        UpdatePreviewPositionAndRotation();
        CreateGrid closestPlatformToPlayer = buildParams.ClosestPlatformToPlayer;

        HandleCurrentPreview();

        if(!TryGetRaycastHit(out RaycastHit hit))
        {
            HandleNoHit();
            return;
        }

        currentPreview.transform.position = hit.point;

        if(CanBuild())
        {
            HandleCanBuild(closestPlatformToPlayer);
        }
        else
        {
            HandleCantBuild();
        }
    }

    private void HandleCurrentPreview()
    {
        if(currentPreview != null && currentPreview != previews[selectedObject])
        {
            currentPreview.SetActive(false);
        }
        currentPreview = previews[selectedObject];
        currentPreview.SetActive(true);
    }

    private void HandleNoHit()
    {
        currentPreview.SetActive(false);
        Crosshair.Instance.SetState(Crosshair.State.CantInteract);
    }

    private bool CanBuild()
    {
        var detector = currentPreview.GetComponent<DetectSimilarObjectsInCollider>();
        return !detector.isDetected && detector.OnFloorCheck();
    }

    private void HandleCanBuild(CreateGrid closestPlatformToPlayer)
    {
        currentPreview.GetComponent<PreviewObject>().CanBuild();
        Crosshair.Instance.SetState(Crosshair.State.CanInteract);

        if(Input.GetMouseButtonDown(0))
        {
            InstantiateBuilding(closestPlatformToPlayer);
        }

    }

    private void InstantiateBuilding(CreateGrid closestPlatformToPlayer)
    {
        GameObject newBuilding = Instantiate(bakeParams.prefabs[selectedObject], currentPreview.transform.position, currentPreview.transform.rotation);
        closestPlatformToPlayer.GetComponent<ObjectContainer>().AddObject(newBuilding);
        newBuilding.GetComponent<Destroyable>().objectContainer = closestPlatformToPlayer.GetComponent<ObjectContainer>();

        if (isAnimate)
        {
            AnimateSpawn(newBuilding);
        }
    }

    private void HandleCantBuild()
    {
        currentPreview.GetComponent<PreviewObject>().CantBuild();
        Crosshair.Instance.SetState(Crosshair.State.CantInteract);
    }

    private void AnimateSpawn(GameObject obj)
    {
        Vector3 initScale = obj.transform.localScale;
        obj.transform.localScale = initScale / 2;
        obj.transform.DOScale(initScale, animationSpeed).SetEase(Ease.OutBack);
    }


}

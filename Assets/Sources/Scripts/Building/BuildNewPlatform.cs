using UnityEngine;

public class BuildNewPlatform : Build, IBuild
{
    [Header("Creating new platform set")]
    [SerializeField] private GameObject plotPrefab;
    [SerializeField] private GameObject platformsHolder;
    [SerializeField] private PlatformsController platformsController;
    [SerializeField] private float yOffset;

    [Header("Platform preview set")]
    [SerializeField] private GameObject platformPreview;
    [SerializeField] private Color colorCanPlace, colorCantPlace;

    private DetectSimilarObjectsInCollider detectSimilarObjectsInCollider;
    private Material platformPreviewMaterial;
    private Vector3 lastPosition;

    private void Start()
    {
        InitializePreviewData();
    }

    private void InitializePreviewData()
    {
        detectSimilarObjectsInCollider = platformPreview.GetComponent<DetectSimilarObjectsInCollider>();
        platformPreviewMaterial = platformPreview.GetComponent<MeshRenderer>().material;
    }

    public void Build(BuildParams buildParams)
    {
        CreateGrid closestPlatformToPlayer = buildParams.ClosestPlatformToPlayer;
        CreateGrid closestPlatformToCursor = buildParams.ClosestPlatformToCursor;

        if (TryGetRaycastHit(out RaycastHit hit))
        {
            UpdatePlatformPreview(hit);
            HandlePlatformPlacement(closestPlatformToPlayer, closestPlatformToCursor);
        }

        platformsController.UpdatePlatformCount();
    }

    private void UpdatePlatformPreview(RaycastHit hit)
    {
        platformPreviewMaterial.color = colorCanPlace;
        platformPreview.SetActive(true);
        platformPreview.transform.position = new Vector3(hit.point.x, hit.point.y + yOffset, hit.point.z);
    }

    private void HandlePlatformPlacement(CreateGrid closestPlatformToPlayer, CreateGrid closestPlatformToCursor)
    {
        if (closestPlatformToPlayer != null)
        {
            SnapPlatform(closestPlatformToPlayer, closestPlatformToCursor);
        }

        if (IsPlacementInvalid(closestPlatformToCursor))
        {
            platformPreviewMaterial.color = colorCantPlace;
            Crosshair.Instance.SetState(Crosshair.State.CantInteract);
            return;
        }

        Crosshair.Instance.SetState(Crosshair.State.CanInteract);

        if (Input.GetMouseButtonDown(0))
        {
            PlaceNewPlatform(closestPlatformToCursor);
        }
    }

    private bool IsPlacementInvalid(CreateGrid closestPlatformToCursor)
    {
        return detectSimilarObjectsInCollider.isDetected || 
               (transform.position.y > 1f && !closestPlatformToCursor.CanBuildSecondFloor());
    }

    private void PlaceNewPlatform(CreateGrid closestPlatformToCursor)
    {
        GameObject newPlot = Instantiate(plotPrefab, platformPreview.transform.position, Quaternion.identity);
        
        if (transform.position.y > 1f)
        {
            newPlot.GetComponent<CreateGrid>().wallsHoldingSecondFloor = closestPlatformToCursor.SetPointsHoldingSecondFloor(true);
        }

        newPlot.transform.parent = platformsHolder.transform;
    }

    private void SnapPlatform(CreateGrid platformToPlayer, CreateGrid platformToCursor)
    {
        Vector3 newPlatformPosition = CalculateSnapPosition(platformToPlayer, platformToCursor);
        platformPreview.transform.position = newPlatformPosition;

        if (lastPosition != newPlatformPosition)
        {
            detectSimilarObjectsInCollider.ClearList();
            lastPosition = newPlatformPosition;
        }
    }

    private Vector3 CalculateSnapPosition(CreateGrid platformToPlayer, CreateGrid platformToCursor)
    {
        Vector3 camDirection = Camera.main.transform.forward;
        camDirection.y = 0;
        camDirection.Normalize();

        Vector3 newPlatformPosition = Vector3.zero;

        if (Mathf.Abs(camDirection.x) > Mathf.Abs(camDirection.z))
        {
            newPlatformPosition.x = camDirection.x > 0 ? platformToPlayer.sizeOfObject : -platformToPlayer.sizeOfObject;
        }
        else
        {
            newPlatformPosition.z = camDirection.z > 0 ? platformToPlayer.sizeOfObject : -platformToPlayer.sizeOfObject;
        }

        if (transform.position.y > 1f && platformToCursor != null && platformToCursor.haveNextFloor)
        {
            return platformToCursor.pointSecondFloor;
        }

        return new Vector3(platformToPlayer.transform.position.x + newPlatformPosition.x, platformToPlayer.transform.position.y, platformToPlayer.transform.position.z + newPlatformPosition.z);
    }
}
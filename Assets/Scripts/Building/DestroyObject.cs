using UnityEngine;

public class DestroyObject : MonoBehaviour, IBuild
{
    [Header("Raycast set")]
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask layerMaskForRaycast;
    [SerializeField] private PlatformsController platformsController;

    public void Build(BuildParams buildParams)
    {
        if (TryGetRaycastHit(out RaycastHit hit))
        {
            HandleRaycastHit(hit);
        }
        else
        {
            Crosshair.Instance.SetState(Crosshair.State.CantInteract);
        }
    }

    public int SelectNextBuild(){return 0;}
    public int SelectPrevBuild(){return 0;}
    public Sprite GetPreviewSprite(int index){return null;}
    public int PreviewsCount(){return 0;}


    private bool TryGetRaycastHit(out RaycastHit hit)
    {
        return Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, layerMaskForRaycast);
    }

    private void HandleRaycastHit(RaycastHit hit)
    {
        ObjectContainer objectContainer = hit.collider.gameObject.GetComponent<ObjectContainer>();

        if (IsObjectContainerOccupied(objectContainer))
        {
            Crosshair.Instance.SetState(Crosshair.State.CantInteract);
            return;
        }

        CreateGrid createGrid = hit.collider.gameObject.GetComponent<CreateGrid>();
        Destroyable destroyable = hit.collider.gameObject.GetComponent<Destroyable>();

        if (destroyable != null)
        {
            if (IsDestroyableInvalid(destroyable, createGrid))
            {
                Crosshair.Instance.SetState(Crosshair.State.CantInteract);
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                HandleObjectDestruction(createGrid, destroyable);
            }
        }

        Crosshair.Instance.SetState(Crosshair.State.CanInteract);
    }

    private bool IsObjectContainerOccupied(ObjectContainer objectContainer)
    {
        return objectContainer != null && objectContainer.objects.Count != 0;
    }

    private bool IsDestroyableInvalid(Destroyable destroyable, CreateGrid createGrid)
    {
        return (destroyable.attachedGridPiece.Count != 0 && destroyable.attachedGridPiece[0].isHoldingSecondFloor) ||
               (createGrid != null && !createGrid.canDestroy);
    }

    private void HandleObjectDestruction(CreateGrid createGrid, Destroyable destroyable)
    {
        if (createGrid != null && !createGrid.haveNextFloor)
        {
            ReleaseWallsHoldingSecondFloor(createGrid);
        }

        if (destroyable.objectContainer != null)
        {
            destroyable.objectContainer.RemoveObject(destroyable.gameObject);
        }

        platformsController.UpdatePlatformCount(destroyable.gameObject);
        destroyable.Destroy();
    }

    private void ReleaseWallsHoldingSecondFloor(CreateGrid createGrid)
    {
        foreach (GridPiece piece in createGrid.wallsHoldingSecondFloor)
        {
            piece.isHoldingSecondFloor = false;
        }
    }

    public void CancelBuild()
    {
        Crosshair.Instance.SetState(Crosshair.State.Default);
    }
}

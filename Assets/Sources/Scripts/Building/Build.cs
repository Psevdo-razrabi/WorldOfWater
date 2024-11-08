using System.Collections.Generic;
using UnityEngine;

public abstract class Build : MonoBehaviour
{
    [Header("Common Building Settings")]
    public BakeParams bakeParams;
    public GameObject previewObjectPool;
    [Header("Raycast")]
    [SerializeField] protected float rayDistance;
    [SerializeField] protected LayerMask layerMaskForRaycast;
    [Header("Animation")]
    [SerializeField] protected bool isAnimate;
    [SerializeField] protected float animationSpeed;
    [Header("Rotate")]
    public int angleIncrement;



    protected GameObject currentPreview;
    [SerializeField] protected List<GameObject> previews = new List<GameObject>();
    protected int selectedObject = 0;
    
    public void Bake(BakeParams bakeParams)
    {
        for(int i = 0; i < bakeParams.prefabs.Length; i++)
        {
            GameObject newPreviewObject = Instantiate(bakeParams.prefabs[i], Vector3.zero, Quaternion.identity);
            newPreviewObject.SetActive(false);
            newPreviewObject.transform.SetParent(previewObjectPool.transform);
            PreviewObject previewObject = newPreviewObject.AddComponent<PreviewObject>();
            previewObject.newMaterial = bakeParams.material;
            previewObject.colorCanBuild = bakeParams.colorCanBuild;
            previewObject.colorCantBuild = bakeParams.colorCantBuild;
            previewObject.BakePrefab();
            Rigidbody rigidbody = newPreviewObject.AddComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.useGravity = false;
            rigidbody.freezeRotation = true;
            SetBoxCollidersToTrigger(newPreviewObject);
            
            DetectSimilarObjectsInCollider detectSimilarObjectsInCollider = newPreviewObject.AddComponent<DetectSimilarObjectsInCollider>();
            detectSimilarObjectsInCollider.layerMask = bakeParams.layerMaskBake;
            detectSimilarObjectsInCollider.floorLayer = bakeParams.layerMaskFloor;
            previews.Add(newPreviewObject);
        }
    }

    public int PreviewsCount()
    {
        return previews.Count;
    }

    public Sprite GetPreviewSprite(int index)
    {
        if(previews[index] != null && previews[index].GetComponent<ObjectData>() != null)
        {
            return previews[index].GetComponent<ObjectData>().previewSprite;
        }
        
        return null;
    }

    private void SetBoxCollidersToTrigger(GameObject obj)
    {
        BoxCollider[] boxColliders = obj.GetComponents<BoxCollider>();
        foreach (BoxCollider collider in boxColliders)
        {
            collider.isTrigger = true;
        }
        foreach (Transform child in obj.transform)
        {
            SetBoxCollidersToTrigger(child.gameObject);
        }
    }

    public int SelectNextBuild()
    {
        selectedObject++;
        return HandleSelectionChange();
    }

    public int SelectPrevBuild()
    {
        selectedObject--;
        return HandleSelectionChange();
    }

    private int HandleSelectionChange()
    {
        if(selectedObject == previews.Count)
        {
            selectedObject = 0;
        }
        else if(selectedObject == -1)
        {
            selectedObject = previews.Count - 1;
        }

        return selectedObject;
    }

    public virtual void UpdatePreviewPositionAndRotation()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, layerMaskForRaycast))
        {
            if(currentPreview != null)
            {
                currentPreview.transform.position = hit.point;
                RotatePreviewWithMouse(currentPreview.transform);
            }
            
        }
        else
        {
            if(currentPreview != null)
            {
                currentPreview.SetActive(false);
            }
        }
    }

    protected void RotatePreviewWithMouse(Transform previewTransform)
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if(scrollInput > 0f)
        {
            previewTransform.rotation = Quaternion.Euler(0, previewTransform.rotation.eulerAngles.y + angleIncrement, 0);
        }
        if(scrollInput < 0f)
        {
            previewTransform.rotation = Quaternion.Euler(0, previewTransform.rotation.eulerAngles.y - angleIncrement, 0);
        }
        if(scrollInput != 0)
        {
            currentPreview.GetComponent<DetectSimilarObjectsInCollider>().ClearList();
        }
    }

    protected bool TryGetRaycastHit(out RaycastHit hit)
    {
        return Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, layerMaskForRaycast);
    }

    public virtual void CancelBuild()
    {
        if(currentPreview != null)
        {
            currentPreview.SetActive(false);
            currentPreview.GetComponent<DetectSimilarObjectsInCollider>().ClearList();
        }


        selectedObject = 0;
        currentPreview = previews[selectedObject];
        

        Crosshair.Instance.SetState(Crosshair.State.Default);
    }

    public class BakedData
    {
        public List<GameObject> buildingPreviews;

        public BakedData()
        {
            buildingPreviews = new List<GameObject>();
        }
    }
}

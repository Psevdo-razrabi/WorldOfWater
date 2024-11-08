using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class BuildScroll : Singleton<BuildScroll>
{
    public VectorAnimation openAnim, closeAnim;
    public BuildNewObject buildNewObject;
    public GameObject tabObject;
    public GameObject previewPrefab;

    public GameObject previewHolder;

    [Header("Animation Open/Close")]
    public float animationDuration;
    public Ease animationEase = Ease.Linear;
    public RectTransform previewsRT;
    public Vector3 closePos;
    public Vector3 openPos;

    [Header("Animation Select")]
    public float selectAnimationDuration;
    public Ease selectAnimationEase = Ease.Linear;
    public float desiredScale;

    private PlayerControls controls;

    private IBuild currentBuildType;
    private int selectedBuild, selectedBuildPrev;

    private List<BuildMenuPreview> previews = new List<BuildMenuPreview>();

#region Init

    protected override void Awake()
    {
        base.Awake();

        closeAnim.OnComplete += Hide;


        controls = new PlayerControls();


        controls.Gameplay.SelectNextBuild.performed += ctx => SelectNextBuild();
        controls.Gameplay.SelectPrevBuild.performed += ctx => SelectPrevBuild();

        buildNewObject.OnBuildSet += InitiateBuilding;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

#endregion

#region Shell

    public void StartBuild()
    {
        if(CanOpen())
        {
            HandleCanBuild();
        }
        else
        {
            HandleCantBuild();
        }
    }

    public void ExitBuild()
    {
        CloseMenu();
        HandleCancelBuild();
    }

#endregion


#region Handlers

    private bool CanOpen()
    {
        return buildNewObject.selectedObjectToBuild == BuildNewObject.SelectedObjectToBuild.Building || buildNewObject.selectedObjectToBuild == BuildNewObject.SelectedObjectToBuild.Wall;
    }

    private void HandleCanBuild()
    {
        OpenMenu();


    }

    private void HandleCantBuild()
    {
        ExitBuild();
    }

    private void InitiateBuilding()
    {   
        currentBuildType = buildNewObject.currentBuild;

        SetPreviews();
        HandleSelectBuild();
    }

    private void HandleBuildChange()
    {
        HandleSelectBuild();
        HandleDeselectBuild(selectedBuildPrev);
    }

    private void HandleCancelBuild()
    {
        selectedBuild = 0;
        selectedBuildPrev = 0;
    }

    private void HandleSelectBuild()
    {
        if(previewHolder.transform.childCount > 0)
        {
            previews[selectedBuild].Select();
        }
    }

    private void HandleDeselectBuild(int index)
    {
        if(previewHolder.transform.childCount > 0)
        {
            previews[index].Deselect();
        }
    }

#endregion


#region Functional

    private void OpenMenu()
    {
        Show();
        Animate(previewsRT, openPos);
        HandleSelectBuild();
        closeAnim.Stop();
        openAnim.Play();
    }

    private void CloseMenu()
    {
        Animate(previewsRT, closePos);
        HandleDeselectBuild(selectedBuild);
        openAnim.Stop();
        closeAnim.Play();
    }

    private void Hide()
    {
        tabObject.SetActive(false);
    }

    private void Show()
    {
        tabObject.SetActive(true);
    }


    private void SelectNextBuild()
    {
        selectedBuildPrev = selectedBuild;
        selectedBuild = currentBuildType.SelectNextBuild();
        HandleBuildChange();
    }

    private void SelectPrevBuild()
    {
        selectedBuildPrev = selectedBuild;
        selectedBuild = currentBuildType.SelectPrevBuild();
        HandleBuildChange();
    }



#endregion

#region Animation
    Tween tween;
    private void Animate(RectTransform rt, Vector3 destination)
    {
        if(tween != null)
        {
            tween.Kill();
        }
        tween = rt.DOAnchorPos(destination, animationDuration).SetEase(animationEase);
    }


#endregion

#region BuildBar
    public Sprite defaultSprite;
    private void SetPreviews()
    {
        ClearPreviousPreviews();
        for(int i = 0; i < currentBuildType.PreviewsCount(); i++)
        {
            GameObject newPreview = Instantiate(previewPrefab, Vector3.zero, Quaternion.identity);
            RectTransform rt = newPreview.GetComponent<RectTransform>();
            rt.transform.SetParent(previewHolder.transform);
            rt.transform.localScale = Vector3.one;
            rt.transform.localRotation = Quaternion.Euler(0, 0, 0);
            rt.transform.localPosition = new Vector3(rt.transform.localPosition.x, rt.transform.localPosition.y, 0);

            Sprite newSprite = currentBuildType.GetPreviewSprite(i);
            if(newSprite == null)
            {
                newSprite = defaultSprite;
            }

            newPreview.GetComponent<BuildMenuPreview>().SetImage(newSprite);

            previews.Add(newPreview.GetComponent<BuildMenuPreview>());
        }
    }


    private void ClearPreviousPreviews()
    {
        previews.Clear();
        for(int i = 0; i < previewHolder.transform.childCount; i++)
        {
            Destroy(previewHolder.transform.GetChild(i).gameObject);
        }
    }


#endregion

}

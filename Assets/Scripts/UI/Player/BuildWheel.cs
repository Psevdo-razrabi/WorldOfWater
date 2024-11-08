using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections.Generic;

public class BuildWheel : MonoBehaviour
{
    [Header("Global")]
    public GameObject[] objects;
    public float animationDuration = 1;
    public BuildNewObject buildNewObject;
    
    [Header("Blur")]
    public GameObject blur;
    public Image blurFadeImg;
    public Color startColor;
    public Color endColor;
    public float blurDuration = 1;
    public Ease blurEase = Ease.Linear;
    private Image blurImg;
    private Material blurMat;

    [Header("Cursor")]
    
    public Canvas parentCanvas;
    public float cursorReturnDuration = 1;
    public Ease cursorEase = Ease.Linear;

    private RectTransform crosshairRT;

    [Header("Build Wheel")]
    [Tooltip(" None, Up, Down, Left, Right")] public Transform[] centers = new Transform[4];
    private enum ECenter { None = -1, Up = 0, Down = 1, Left = 2, Right = 3 }
    private int lastClosestCenter = -1, closestCenter;

[Space(10)]
    [Header("Sections settings")]


    [Header("Up settings")]
    public VectorAnimations UpAnimations = new VectorAnimations(); 

[Space(5)]
    [Header("Down settings")]
    public VectorAnimations DownAnimations = new VectorAnimations(); 

[Space(5)]
    [Header("Left settings")]
    public VectorAnimations LeftAnimations = new VectorAnimations(); 

[Space(5)]
    [Header("Right settings")]
    public VectorAnimations RightAnimations = new VectorAnimations(); 

[Space(10)]
    [Header("Icons settings")]

    public GameObject[] buildWheelIcons;
    public RectTransform[] buildWheelIconsDesired;
    public RectTransform[] buildWheelIconsDesiredSelected;
    public Ease iconsEase = Ease.Linear;

    private List<ObjectAnimation> objectAnimations = new List<ObjectAnimation>();

    private VectorAnimations[] allAnimations;

    [Serializable]
    public struct VectorAnimations
    {
        public VectorAnimation animOpen;
        public VectorAnimation animClose;
        public VectorAnimation animSelect;
        public VectorAnimation animDeselect;

        private void SetAnimationState(VectorAnimation animation, bool isPlay)
        {
            if(isPlay)
            {
                animation.Play();
            }
            else
            {
                animation.Stop();
            }
        }

        public void Open(bool isPlay)
        {
            SetAnimationState(animOpen, isPlay);
        }
        public void Close(bool isPlay)
        {
            SetAnimationState(animClose, isPlay);

        }
        public void Select(bool isPlay)
        {
            SetAnimationState(animSelect, isPlay);

        }
        public void Deselect(bool isPlay)
        {
            SetAnimationState(animDeselect, isPlay);

        }
    }

    [Serializable]
    public struct ObjectAnimation
    {
        public Vector2 initScale;
        public Vector2 endScale;

        public Vector2 initPos;
        public Vector2 endPos;

        public Vector2 selectPos;
        public Vector2 selectScale;

        public GameObject animationObject;

        public RectTransform rectTransform;

        private List<Tween> tweens;

        public ObjectAnimation(bool empty)
        {
            initScale = Vector2.zero;
            endScale = Vector2.zero;

            initPos = Vector2.zero;
            endPos = Vector2.zero;

            selectPos = Vector2.zero;
            selectScale = Vector2.zero;

            animationObject = null;
            rectTransform = null;

            tweens = new List<Tween>();
        }


        public List<Tween> Animate(Vector2 desiredPos, Vector2 desiredScale, float duration, Ease ease, bool isOpenOrClose, bool isSelect)
        {
            List<Tween> localTweens = new List<Tween>();

            StopTweens();
            
            localTweens.Add(rectTransform.DOAnchorPos(desiredPos, duration).SetEase(ease));
            
            localTweens.Add(rectTransform.DOSizeDelta(desiredScale, duration).SetEase(ease));

            tweens.AddRange(localTweens);

            return localTweens;
        }

        public void StopTweens()
        {
            foreach(Tween tween in tweens)
            {
                if(tween != null)
                {
                    tween.Kill();
                }
            }
            tweens.Clear();
        }



    }


    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.BuildMode.performed += ctx => EnterBuildMode();
        controls.Gameplay.BuildMode.canceled += ctx => ExitBuildMode();

        controls.Gameplay.QuitBuildMode.performed += ctx => CloseBuildMode();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    
#region Init
    private void Start()
    {
        blurImg = blur.GetComponent<Image>();
        blurMat = blurImg.material;

        crosshairRT = Crosshair.Instance.gameObject.GetComponent<RectTransform>();

        allAnimations = new VectorAnimations[] { UpAnimations, DownAnimations, LeftAnimations, RightAnimations };

        SetObjectAnimations();


    }

    private void SetObjectAnimations()
    {
        for(int i = 0; i < buildWheelIcons.Length; i++)
        {
            ObjectAnimation objectAnimation = new ObjectAnimation(true);

            objectAnimation.animationObject = buildWheelIcons[i];
            objectAnimation.rectTransform = buildWheelIcons[i].GetComponent<RectTransform>();

            objectAnimation.initScale = Vector3.zero;
            objectAnimation.endScale = buildWheelIconsDesired[i].GetComponent<RectTransform>().sizeDelta;

            objectAnimation.initPos = Vector3.zero;
            objectAnimation.endPos = buildWheelIconsDesired[i].GetComponent<RectTransform>().anchoredPosition;

            objectAnimation.selectPos = buildWheelIconsDesiredSelected[i].GetComponent<RectTransform>().anchoredPosition;
            objectAnimation.selectScale = buildWheelIconsDesiredSelected[i].GetComponent<RectTransform>().sizeDelta;


            objectAnimations.Add(objectAnimation);
        }
    }

#endregion

#region Main

    private void Update()
    {
        if(controls.Gameplay.BuildMode.IsPressed())
        {
            UpdateBuildMode();
        }
    }

    private void UpdateBuildMode()
    {
        CursorFolowMouse();
        if(!allAnimations[0].animClose.isPlaying)
        {
            SelectBuildMode();
        }
    }

    private void EnterBuildMode()
    {
        ActivateUI(true);

        ResetTweens();
        KillAnimationIcons();

        SetBlur(0.015f, endColor, null);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;




        CloseAnimationsVectorUI(false);

        
        OpenAnimationsIcons();
        OpenAnimationsVectorUI(true);


        CloseBuildMode();

    }
    private void ExitBuildMode()
    {

        ResetTweens();
        KillAnimationIcons();

        ResetCursor();
        SetBlur(0, startColor, ExitMenu);
        Cursor.lockState = CursorLockMode.Locked;

        lastClosestCenter = -1;


        OpenAnimationsVectorUI(false);
        SelectAnimationsVectorUI(false);
        DeselectAnimationsVectorUI(false);

        OpenBuildMode();

        CloseAnimationsIcons();
        CloseAnimationsVectorUI(true);




    }

#endregion


#region Animation

    private List<Tween> tweens = new List<Tween>();

    #region /   Icons

    private void OpenAnimationsIcons()
    {
        foreach(ObjectAnimation objectAnimation in objectAnimations)
        {
            tweens.AddRange(objectAnimation.Animate(objectAnimation.endPos, objectAnimation.endScale, animationDuration, iconsEase, true, false));
        }
    }

    private void CloseAnimationsIcons()
    {
        foreach(ObjectAnimation objectAnimation in objectAnimations)
        {
            tweens.AddRange(objectAnimation.Animate(objectAnimation.initPos, objectAnimation.initScale, animationDuration, iconsEase, true, false));
        }
    }

    private void SelectAnimationIcon(ObjectAnimation objectAnimation)
    {
        objectAnimation.Animate(objectAnimation.selectPos, objectAnimation.selectScale, animationDuration, iconsEase, false, true);
    }

    private void DeselectAnimationIcon(ObjectAnimation objectAnimation)
    {
        objectAnimation.Animate(objectAnimation.endPos, objectAnimation.endScale, animationDuration, iconsEase, false, false);
    }

    private void KillAnimationIcons()
    {
        foreach(ObjectAnimation objectAnimation in objectAnimations)
        {
            objectAnimation.StopTweens();
        }
    }

    #endregion

    #region /   VectorIU
    private void OpenAnimationsVectorUI(bool isOpen)
    {
        foreach(VectorAnimations animation in allAnimations)
        {
            animation.Open(isOpen);
        }
    }


    private void CloseAnimationsVectorUI(bool isOpen)
    {
        foreach(VectorAnimations animation in allAnimations)
        {
            animation.Close(isOpen);
        }
    }

    private void SelectAnimationsVectorUI(bool isOpen)
    {
        foreach(VectorAnimations animation in allAnimations)
        {
            animation.Select(isOpen);
        }
    }

    private void DeselectAnimationsVectorUI(bool isOpen)
    {
        foreach(VectorAnimations animation in allAnimations)
        {
            animation.Deselect(isOpen);
        }
    }

    #endregion


    private void ResetTweens()
    {
        foreach(Tween tween in tweens)
        {
            KillTween(tween);
        }

    }

    private void KillTween(Tween tween)
    {
        if(tween != null && tween.active)
        {
            tween.Kill();
        }
    }

    private void ActivateUI(bool isActivate)
    {
        foreach(GameObject obj in objects)
        {
            obj.SetActive(isActivate);
        }
    }

#endregion
    
#region Blur
    private void SetBlur(float endVal, Color blurColor, Action OnFinish)
    {
        blur.SetActive(true);
        Tween tween;
        tween = blurFadeImg.DOColor(blurColor, blurDuration).OnComplete(() => OnFinish?.Invoke()).SetEase(blurEase);
        tweens.Add(tween);

        float initVal = blurMat.GetFloat("_BlurX");
        float lerpVal = 0;
        tween = DOTween.To(x => lerpVal = x, initVal, endVal, blurDuration).SetEase(blurEase).OnUpdate(() => {
            blurMat.SetFloat("_BlurX", lerpVal);
            blurMat.SetFloat("_BlurY", lerpVal);
        });
        tweens.Add(tween);
    }


    private void ExitMenu()
    {
        blur.SetActive(false);
        ActivateUI(false);

    }

#endregion

#region Cursor

    private void CursorFolowMouse()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            mousePos, 
            parentCanvas.worldCamera,
            out localPoint
        );

        if(Vector3.Distance(localPoint, Vector3.zero) >= 250)
        {
            localPoint = localPoint.normalized * 250;
        }

        Vector3 newPos = new Vector3(localPoint.x, localPoint.y, crosshairRT.localPosition.z);

        crosshairRT.localPosition = newPos;

    }
    private void SelectBuildMode()
    {
        
        int closestCenter = FindClosestCenter();

        if(closestCenter == (int)ECenter.None)
        {
            DeselectBuildModeAnimationsOnNone(closestCenter);
            return;
        }

        if(closestCenter != lastClosestCenter)
        {
            DeselectBuildModeAnimations(closestCenter);

            SelectBuildModeAnimations(closestCenter);
        }

    }

    private void SelectBuildModeAnimations(int index)
    {
        allAnimations[index].Deselect(false);

        if(allAnimations[index].animOpen.isPlaying)
        {
            allAnimations[index].Open(false);
        }

        allAnimations[index].Select(true);

        SelectAnimationIcon(objectAnimations[index]);

        closestCenter = index;

    }

    private void DeselectBuildModeAnimations(int index)
    {
        if(lastClosestCenter != -1)
        {
            allAnimations[lastClosestCenter].Select(false);
            allAnimations[lastClosestCenter].Deselect(true);
            DeselectAnimationIcon(objectAnimations[lastClosestCenter]);

        }

        lastClosestCenter = index;
    }

    private void DeselectBuildModeAnimationsOnNone(int index)
    {
        if(lastClosestCenter != -1)
        {
            allAnimations[lastClosestCenter].Select(false);
            allAnimations[lastClosestCenter].Deselect(true);

            DeselectAnimationIcon(objectAnimations[lastClosestCenter]);

            lastClosestCenter = -1;
        }
    }

    private void OpenBuildMode()
    {
        BuildNewObject.SelectedObjectToBuild selectedObject;
        switch (closestCenter)
        {
            case 0:
                selectedObject = BuildNewObject.SelectedObjectToBuild.Platform;
                break;
            case 1:
                selectedObject = BuildNewObject.SelectedObjectToBuild.Destroy;
                break;
            case 2:
                selectedObject = BuildNewObject.SelectedObjectToBuild.Building;
                break;
            case 3:
                selectedObject = BuildNewObject.SelectedObjectToBuild.Wall;
                break;
            default:
                return;
        }

        buildNewObject.selectedObjectToBuild = selectedObject;
        
        buildNewObject.EnterBuild();

        BuildScroll.Instance.StartBuild();

    }

    private void CloseBuildMode()
    {
        buildNewObject.ExitBuild();

        BuildScroll.Instance.ExitBuild();
    }

    private int FindClosestCenter()
    {
        if(Vector3.Distance(crosshairRT.transform.localPosition, Vector3.zero) < 100)
        {
            return -1;
        }
        int center = 0;
        float closestDistance = float.MaxValue;
        for(int i = 0; i < centers.Length; i++)
        {
            float distance = Vector3.Distance(crosshairRT.transform.position, centers[i].transform.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                center = i;
            }
        }

        return center;
    }

    private void ResetCursor()
    {
        Tween tween = crosshairRT.transform.DOLocalMove(Vector3.zero, cursorReturnDuration).SetEase(cursorEase);
        tweens.Add(tween);
    }

#endregion



}

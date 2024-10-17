using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CreateGrid : MonoBehaviour
{

    [Header("Grid settings")]
    [SerializeField] GameObject plotPrefab;
    [SerializeField] float yOffsetForPlot;
    [SerializeField] int gridResolution;
    [SerializeField] float gridOffset;
    [SerializeField] GameObject gridPointPrefab;
    [SerializeField] bool isBuildMode;
    [Header("Animation settings")]
    [SerializeField] bool isAnimateSpawn; 
    [SerializeField] float animationSpeed;
    [SerializeField] float animationYOffset;
    [SerializeField] AnimationType animationType;
    
    enum AnimationType
    {
        Continuous, StepByStep
    }



    [NonSerialized] public float sizeOfObject;
    List<GridPiece> gridPieces = new List<GridPiece>();
    List<GameObject> plotPieces = new List<GameObject>();
    void Awake()
    {
        GenerateGrid();
        GenerateRaftPieces();
        HideMaterial();
        
    }
    
    void GenerateRaftPieces()
    {
        if(!isAnimateSpawn)
        {
            animationYOffset = 0;
        }
        System.Random rand = new System.Random();
        for(int i = 0; i < gridPieces.Count; i++)
        {
            Transform temp = Instantiate(plotPrefab, Vector3.zero, Quaternion.identity).transform;
            temp.SetParent(transform);
            temp.position = gridPieces[i].center;
            temp.position = new Vector3(temp.position.x, temp.position.y + yOffsetForPlot + animationYOffset, temp.position.z);
            temp.localRotation = Quaternion.Euler(0, 90 * rand.Next(0, 4), 0);


            plotPieces.Add(temp.gameObject);
        }

        if(isAnimateSpawn)
        {

            AnimateSpawn(0, animationSpeed);
        }
    }

    void HideMaterial()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    public void GenerateGrid()
    {
        sizeOfObject = gameObject.transform.localScale.x * 10; // remove * 10
        if(gridResolution < 2) gridResolution = 2;
        gridPieces.Clear();
        ClearPoints();

        Vector3 startPoint = new Vector3(transform.position.x - (sizeOfObject / 2) + (sizeOfObject / (gridResolution * 2)) + gridOffset, 0, transform.position.z - (sizeOfObject / 2) + (sizeOfObject / (gridResolution * 2)) + gridOffset);
        Vector3 endPoint = new Vector3(transform.position.x + (sizeOfObject / 2) - (sizeOfObject / (gridResolution * 2)) - gridOffset, 0, transform.position.z + (sizeOfObject / 2) - (sizeOfObject / (gridResolution * 2)) - gridOffset);
        int pointer = 0;
        for(int x = 0; x < gridResolution; x++)
        {
            for(int z = 0; z < gridResolution; z++)
            {
                Vector3 center = new Vector3(Mathf.Lerp(startPoint.x, endPoint.x, x / ((float)gridResolution - 1)), transform.position.y, Mathf.Lerp(startPoint.z, endPoint.z, z / ((float)gridResolution - 1)));
                float size = sizeOfObject / (float)gridResolution - (gridOffset / 2);


                GridPiece newGridPiece = new GridPiece(center, size);
                newGridPiece.gridPoint = Instantiate(gridPointPrefab);
                newGridPiece.gridPoint.transform.SetParent(transform, false);

                pointer++;
                newGridPiece.gridPoint.gameObject.name = pointer.ToString();

                newGridPiece.BakeGridPoint();

                gridPieces.Add(newGridPiece);
            }
        }

        

    }

    void AnimateSpawn(int pointer, float speed)
    {
        if(pointer < gridPieces.Count && pointer > -1)
        {
            if(animationType == AnimationType.Continuous)
            {
                Vector3 initScale = plotPieces[pointer].transform.localScale;
                plotPieces[pointer].transform.localScale = Vector3.zero;
                plotPieces[pointer].transform.DOScale(initScale, speed / 2).SetEase(Ease.OutBack);
                plotPieces[pointer].transform.DOMove(new Vector3(gridPieces[pointer].center.x, gridPieces[pointer].center.y + yOffsetForPlot, gridPieces[pointer].center.z), speed, false).SetEase(Ease.OutBack);
                AnimateSpawn(pointer + 1, animationSpeed * (pointer + 1));
            }
            else if(animationType == AnimationType.StepByStep)
            {
                plotPieces[pointer].transform.DOMove(new Vector3(gridPieces[pointer].center.x, gridPieces[pointer].center.y + yOffsetForPlot, gridPieces[pointer].center.z), speed, false).OnComplete(() => AnimateSpawn(pointer + 1, speed)).SetEase(Ease.OutBack);
            }

        }
    }

    void ClearPoints()
    {
        int childs = transform.childCount;
        for(int i = childs - 1; i > -1; i--)
        {
            if(transform.GetChild(i).gameObject.tag == "GridPoint")
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
    }

    


    public void EnterBuildMode()
    {
        if(isBuildMode) return;
        for(int i = 0; i < gridPieces.Count; i++)
        {
            gridPieces[i].ShowPoint();
        }
        if(Application.isPlaying)
            isBuildMode = true;
    }
    
    public void ExitBuildMode()
    {
        if(!isBuildMode) return;
        for(int i = 0; i < gridPieces.Count; i++)
        {
            gridPieces[i].HidePoint();
        }
        if(Application.isPlaying)
            isBuildMode = false;

    }

    GridPiece GetClosestPoint(Vector3 pos)
    {
        float lowestDistance = float.MaxValue;
        int pointNum = 0;
        for(int i = 0; i < gridPieces.Count; i++)
        {
            if(Vector3.Distance(pos, gridPieces[i].center) < lowestDistance)
            {
                lowestDistance = Vector3.Distance(pos, gridPieces[i].center);
                pointNum = i;
            }
        }

        return gridPieces[pointNum];
    }

}

class GridPiece
{
    public Vector3 center = Vector3.zero;
    public float size = .1f;
    public GameObject gridPoint;

    public GridPiece(Vector3 center, float size)
    {
        this.center = center;
        this.size = size;
    }


    public void BakeGridPoint()
    {
        gridPoint.transform.position = center;
        gridPoint.gameObject.SetActive(false);
    }

    public void HidePoint()
    {
        gridPoint.gameObject.SetActive(false);
    }

    public void ShowPoint()
    {
        gridPoint.gameObject.SetActive(true);
    }


}

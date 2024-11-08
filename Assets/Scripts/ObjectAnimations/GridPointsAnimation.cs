using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class GridPointsAnimation : Singleton<GridPointsAnimation>
{
    public bool enable;
    private List<CreateGrid> platforms = new List<CreateGrid>();
    private List<GridPiece> points = new List<GridPiece>();
    private List<GameObject> objects = new List<GameObject>();
    private GridPiece selectedPoint;

    public void SetPlatformsAndStart(List<CreateGrid> newPlatforms, GridPiece point)
    {
        if(!enable) return;
        platforms = newPlatforms;
        selectedPoint = point;
        InitParams();
    }

#region Init

    private void InitParams()
    {
        SetPoints();
        SetObjects();
        StartAnimations();
    }

    private void SetPoints()
    {
        points.Clear();
        for(int i = 0; i < platforms.Count; i++)
        {
            for(int j = 0; j < platforms[i].gridPieces.Count; j++)
            {
                points.Add(platforms[i].gridPieces[j]);
            }
        }
    }

    private void SetObjects()
    {
        objects.Clear();
        for(int i = 0; i < points.Count; i++)
        {
            objects.Add(points[i].gridPoint);
        }
    }

#endregion

#region Animation

    public float maxDistance = 2;
    public float maxDistanceToMove = 2;
    public float duration = 1;
    public float maxDelay = 1;
    private int animationIndex = 0;
    private bool isAnimation = false;
    private List<Tween> tweens = new List<Tween>();
    private void StartAnimations()
    {
        animationIndex = 0;
        isAnimation = true;

        StopAnimations();

        for(int i = 0; i < points.Count; i++)
        {
            Animate(objects[i], points[i]);
        }
    }

    private void StopAnimations()
    {
        foreach(Tween tween in tweens)
        {
            tween.Kill();
        }

        tweens.Clear();
    }

    private void IncreaseIndex()
    {
        animationIndex++;
        if(animationIndex == points.Count)
        {
            isAnimation = false;
        }
    }

    private void Animate(GameObject obj, GridPiece piece)
    {
        Vector3 newPos = IncreaseVectorLength(obj.transform.position);
        // Vector3 newPos = obj.transform.position;
        // newPos.y = obj.transform.position.y + CalculateDelay(obj.transform.position) + maxDistanceToMove;
        Sequence sequence = DOTween.Sequence();
        Tween startDelay = DOTween.To(x => x = 0, 0, 1, CalculateDelay(obj.transform.position));
        Tween moveToNewPos = obj.transform.DOMove(newPos, duration).SetEase(Ease.OutSine);
        Tween moveToCenter = obj.transform.DOMove(piece.center, duration).OnComplete(() => IncreaseIndex()).SetEase(Ease.InSine);

        sequence.Append(startDelay).Append(moveToNewPos);

        tweens.Add(sequence);

        sequence.Append(moveToCenter);
        
        sequence.Play();

    }

    private float CalculateDelay(Vector3 vector)
    {
        return Mathf.Lerp(0, maxDelay, CalculateMagnitude(vector) / (maxDistance - 1));
    }

    private float CalculateMagnitude(Vector3 vector)
    {
        return Vector3.Distance(vector, selectedPoint.gridPoint.transform.position);
    }


    private Vector3 IncreaseVectorLength(Vector3 vector)
    {

        Vector3 direction = selectedPoint.gridPoint.transform.position - vector;
        float length = direction.magnitude;

        float distanceFromPoint = CalculateMagnitude(vector);


        if(distanceFromPoint > maxDistance) return vector;

        if(length > 0)
        {
            direction.Normalize();

            Vector3 finalPosition = vector - direction * Mathf.Lerp(0, maxDistanceToMove, 1 - (distanceFromPoint / maxDistance));

            return finalPosition;
        }

        return vector;
    }


#endregion


}

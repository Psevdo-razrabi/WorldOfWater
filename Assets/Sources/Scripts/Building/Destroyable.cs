using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Destroyable : MonoBehaviour
{
    public GridPiece attachedGridPiece;
    public float animationSpeed;
    bool isDestroying;


    public void Destroy()
    {
        if(isDestroying) return;
        isDestroying = true;
        if(attachedGridPiece != null)
        {
            attachedGridPiece.isEmpty = true;
        }
        AnimateDestroy(gameObject);
    }

    void AnimateDestroy(GameObject obj)
    {
        Vector3 newScale = obj.transform.localScale / 2;
        obj.transform.DOScale(newScale, animationSpeed).OnComplete(() => Destroy(gameObject)).SetEase(Ease.InBack);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Destroyable : MonoBehaviour
{
    public List<GridPiece> attachedGridPiece = new List<GridPiece>();
    public float animationSpeed;
    bool isDestroying;
    public ObjectContainer objectContainer;


    public void Destroy()
    {
        if(isDestroying) return;
        isDestroying = true;
        if(attachedGridPiece != null)
        {
            foreach(GridPiece piece in attachedGridPiece)
            {
                piece.isEmpty = true;
            }
        }
        AnimateDestroy();
    }

    void AnimateDestroy()
    {
        Vector3 newScale = gameObject.transform.localScale / 2;
        if(gameObject == null) return;
        gameObject.transform.DOScale(newScale, animationSpeed).OnComplete(() => Destroy(gameObject)).SetEase(Ease.InBack);
    }

}

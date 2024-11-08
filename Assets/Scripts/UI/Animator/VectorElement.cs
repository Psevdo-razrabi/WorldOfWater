using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VectorElement : MonoBehaviour
{
    public ElementSettings elementSettings;
    public LineRenderer lineRenderer;
    public bool constantColor = true;
    public bool constantWidth = true;
    public Color color = Color.white;
    public Color color_start = Color.white;
    public Color color_end = Color.white;
    public float width = 1;
    public float width_start = 1;
    public float width_end = 1;
    public bool fillEndCap;

    public void Draw(bool setColor)
    {
        SetRectTransform();
        SetLineRenderer();
        SetElementSettings(setColor);
    }


    private void SetRectTransform()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        if(rectTransform)
        {
            rectTransform.sizeDelta = Vector2.zero;
        }

        int layerIndex = LayerMask.NameToLayer("VectorUI");
        gameObject.layer = layerIndex;
    }

    private void SetLineRenderer()
    {
        if(lineRenderer == null)
        {
            if(gameObject.GetComponent<LineRenderer>() != null)
            {
                lineRenderer = gameObject.GetComponent<LineRenderer>();
            }
            else
            {
                lineRenderer = gameObject.AddComponent<LineRenderer>();
            }
        }
    }

    public virtual void SetElementSettings(bool setColor)
    {
        lineRenderer.useWorldSpace = false;
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRenderer.material = elementSettings.material;

        if(setColor)
        {
            if(constantColor)
            {
                lineRenderer.startColor = color;
                lineRenderer.endColor = color;
            }
            else
            {
                lineRenderer.startColor = color_start;
                lineRenderer.endColor = color_end;
            }

            if(constantWidth)
            {
                lineRenderer.startWidth = width;
                lineRenderer.endWidth = width;
            }
            else
            {
                lineRenderer.startWidth = width_start;
                lineRenderer.endWidth = width_end;
            }
        }

        lineRenderer.numCapVertices = fillEndCap ? 100 : 0;
    }



}




using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    [SerializeField] public Material newMaterial;
    [SerializeField] public Color colorCanBuild, colorCantBuild;

    Material newMaterialOnObject;


    public void BakePrefab()
    {
        newMaterialOnObject = newMaterial;
        ChangeMaterialsRecursively(transform, newMaterialOnObject);
    }

    void ChangeMaterialsRecursively(Transform parent, Material mat)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = mat;
            }
            

            ChangeMaterialsRecursively(child, mat);
        }
    }

    public void CanBuild()
    {
        newMaterialOnObject.color = colorCanBuild;
        ChangeMaterialsRecursively(transform, newMaterialOnObject);

    }
    public void CantBuild()
    {
        newMaterialOnObject.color = colorCantBuild;
        ChangeMaterialsRecursively(transform, newMaterialOnObject);

    }
}

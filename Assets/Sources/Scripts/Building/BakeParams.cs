using UnityEngine;


[CreateAssetMenu(fileName = "BakeParams", menuName = "Build")]
public class BakeParams : ScriptableObject
{
    public GameObject[] prefabs;
    public Material material;
    public Color colorCanBuild;
    public Color colorCantBuild;
    public LayerMask layerMaskBake;
    public LayerMask layerMaskFloor;
}

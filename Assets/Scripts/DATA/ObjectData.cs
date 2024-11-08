using UnityEngine;

public class ObjectData : MonoBehaviour
{
    #region LOD
    public bool haveLOD;


    public GameObject Model_LOD1;
    public GameObject Model_LOD2;
    public GameObject Model_LOD3;
    private GameObject[] Models_LOD;


    private GameObject ObjectSelf;
    private GameObject Object_LOD1;
    private GameObject Object_LOD2;
    private GameObject Object_LOD3;
    private GameObject[] Objects_LOD;

    private GameObject Current_LOD;


    private void Awake()
    {

        if(!haveLOD) return;

        Models_LOD = new GameObject[] { Model_LOD1, Model_LOD2, Model_LOD3 };
        Objects_LOD = new GameObject[] { Object_LOD1, Object_LOD2, Object_LOD3 };

        ObjectSelf = gameObject;
        Current_LOD = ObjectSelf;

        for(int i = 0; i < Models_LOD.Length; i++)
        {
            if(Models_LOD[i] == null) break;
            Objects_LOD[i] = Instantiate(Models_LOD[i], transform.position, Quaternion.identity);
            Objects_LOD[i].SetActive(false);
        }

        LODManager.Instance.AddObject(ObjectSelf);

    }

    private void OnDestroy()
    {

        if(!haveLOD) return;

        foreach(GameObject obj in Objects_LOD)
        {
            if(obj == null) break;
            Destroy(obj);
        }
    }


    public void SetLOD(int lod)
    {
        Current_LOD.SetActive(false);
        

        if(lod == 0)
        {
            ObjectSelf.SetActive(true);
            Current_LOD = ObjectSelf;
        }
        else
        {
            if(Objects_LOD[lod - 1] == null) return;
            Objects_LOD[lod - 1].SetActive(true);
            Current_LOD = Objects_LOD[lod - 1];
        }
    }



    


    #endregion

    #region UI

    public Sprite previewSprite;


    #endregion

}

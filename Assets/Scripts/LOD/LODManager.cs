using System.Collections.Generic;
using UnityEngine;

public class LODManager : Singleton<LODManager>
{
    public float moveThreeshold;

    public float Distance_LOD1;
    public float Distance_LOD2;
    public float Distance_LOD3;


    private Vector3 lastPos;
    private List<GameObject> obj_pool = new List<GameObject>();

    public void AddObject(GameObject obj)
    {
        obj_pool.Add(obj);
    }

    private void OnEnable()
    {
        lastPos = transform.position;
    }
    private void Update()
    {
        if(Vector3.Distance(transform.position, lastPos) < moveThreeshold) return;

        lastPos = transform.position;

        SetLODs();


    }

    private void SetLODs()
    {
        for(int i = 0; i < obj_pool.Count; i++)
        {
            if(obj_pool[i] == null)
            {
                obj_pool.RemoveAt(i);
                continue;
            }
            float distance = Vector3.Distance(transform.position, obj_pool[i].transform.position);
            if(distance < Distance_LOD1)
            {
                obj_pool[i].GetComponent<ObjectData>().SetLOD(0);
            }
            else if(distance < Distance_LOD2)
            {
                obj_pool[i].GetComponent<ObjectData>().SetLOD(1);
            }
            else if(distance < Distance_LOD3)
            {
                obj_pool[i].GetComponent<ObjectData>().SetLOD(2);
            }
            else
            {
                obj_pool[i].GetComponent<ObjectData>().SetLOD(3);
            }
        }
    }
}

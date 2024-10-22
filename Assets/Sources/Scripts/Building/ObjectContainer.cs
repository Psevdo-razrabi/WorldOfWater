using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectContainer : MonoBehaviour
{
    [SerializeField] public List<GameObject> objects = new List<GameObject>();
    public void AddObject(GameObject obj)
    {
        objects.Add(obj);
    }

    public void RemoveObject(GameObject obj)
    {
        objects.Remove(obj);
        CleanupList(objects);
    }

    void CleanupList<T>(List<T> list)
    {
        list.RemoveAll(x => x == null);
    }

}

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsController : MonoBehaviour
{
    [SerializeField] GameObject platformsHolder;
    [SerializeField] public List<GameObject> platforms = new List<GameObject>();
    [NonSerialized] public Action OnPlatformsCountUpdate;



    void Start()
    {
        for(int i = 0 ; i < platformsHolder.transform.childCount; i++)
        {
            platforms.Add(platformsHolder.transform.GetChild(i).gameObject);
        }
        OnPlatformsCountUpdate?.Invoke();
    }

    public void UpdatePlatformCount()
    {
        if(platformsHolder.transform.childCount == platforms.Count) return;
        for(int i = 0 ; i < platformsHolder.transform.childCount; i++)
        {
            GameObject platform = platformsHolder.transform.GetChild(i).gameObject;
            if(!platforms.Contains(platform))
            {
                platforms.Add(platform);
            }

        }

        while(platformsHolder.transform.childCount < platforms.Count)
        {
            platforms.Remove(FindMissingPlatform());
            CleanupList(platforms);
            
        }

        OnPlatformsCountUpdate?.Invoke();
    }

    public void UpdatePlatformCount(GameObject gameObject)
    {
        platforms.Remove(gameObject);
        CleanupList(platforms);
        OnPlatformsCountUpdate?.Invoke();
    }

    GameObject FindMissingPlatform()
    {
        for(int i = 0; i < platforms.Count; i++)
        {
            bool found = false;
            for(int j = 0; j < platformsHolder.transform.childCount; j++)
            {
                if(platforms[i].gameObject == platformsHolder.transform.GetChild(j).gameObject)
                {
                    found = true;
                    break;
                }
            }

            if(!found)
            {
                return platforms[i];
            }
        }

        return null;
    }

    void CleanupList<T>(List<T> list)
    {
        list.RemoveAll(x => x == null);
    }

    public CreateGrid FindClosestPlatform(Vector3 origin, float maxDistance)
    {
        CreateGrid closestPlatform = null;
        float minimalDistance = maxDistance;
        for(int i = 0; i < platforms.Count; i++)
        {
            float distance = Vector3.Distance(origin, platforms[i].transform.position);
            if(distance < minimalDistance)
            {
                minimalDistance = distance;
                closestPlatform = platforms[i].GetComponent<CreateGrid>();
            }
        }

        if(closestPlatform == null)
        {
            return null;
        }
        else
        {
            return closestPlatform;
        }

    }

    public List<CreateGrid> FindClosestPlatforms(Vector3 origin, float maxDistance)
    {
        List<CreateGrid> closestPlatform = new List<CreateGrid>();
        float minimalDistance = maxDistance;
        for(int i = 0; i < platforms.Count; i++)
        {
            float distance = Vector3.Distance(origin, platforms[i].transform.position);
            if(distance < minimalDistance)
            {
                closestPlatform.Add(platforms[i].GetComponent<CreateGrid>());
            }
        }

        if(closestPlatform.Count == 0)
        {
            return null;
        }
        else
        {
            return closestPlatform;
        }

    }


}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    [SerializeField] bool changeBuildMode;
    [SerializeField] bool isBuildMode;
    [SerializeField] GameObject platformsHolder;
    [SerializeField] public float maxBuildDistance;
    [SerializeField] float moveThresholdToUpdateBuildingPlatform;
    [SerializeField] public List<GameObject> platforms = new List<GameObject>();

    public Action OnPlatformsCountUpdate;
    public Action<bool> OnBuildModeChange;
    Vector3 lastPosition;
    CreateGrid currentPlatform, newPlatform;


    void Start()
    {
        for(int i = 0 ; i < platformsHolder.transform.childCount; i++)
        {
            platforms.Add(platformsHolder.transform.GetChild(i).gameObject);
        }
        OnPlatformsCountUpdate?.Invoke();


    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            isBuildMode = !isBuildMode;
            if(isBuildMode)
            {
                Build();
            }
            else
            {
                if(currentPlatform != null)
                    currentPlatform.ExitBuildMode();
                currentPlatform = null;
            }

            OnBuildModeChange?.Invoke(isBuildMode);

            changeBuildMode = false;
        }

        if (isBuildMode)
        {
            Vector3 newPosition = transform.position;
            if(Vector3.Distance(lastPosition, newPosition) > moveThresholdToUpdateBuildingPlatform)
            {
                lastPosition = newPosition;
            }
            else
            {
                return;
            }

            Build();

        }
    }



    void Build()
    {
        UpdatePlatformCount();
        newPlatform = FindClosestPlatform(gameObject);
        if(newPlatform != currentPlatform)
        {
            if(currentPlatform != null)
            {
                currentPlatform.ExitBuildMode();

            }
            currentPlatform = newPlatform;

            if(currentPlatform != null)
            {
                currentPlatform.EnterBuildMode();
            }

        }
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

    public CreateGrid FindClosestPlatform(GameObject origin)
    {
        CreateGrid closestPlatform = null;
        float minimalDistance = maxBuildDistance;
        for(int i = 0; i < platforms.Count; i++)
        {
            float distance = Vector3.Distance(origin.transform.position, platforms[i].transform.position);
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
}

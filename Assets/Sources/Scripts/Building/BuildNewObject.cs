using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildNewObject : MonoBehaviour
{
    [SerializeField] PlatformsController platformsController;
    [SerializeField] BuildNewPlatform buildNewPlatform;
    [SerializeField] BuildNewWall buildNewWall;
    [SerializeField] BuildNewBuilding buildNewBuilding;
    [SerializeField] DestroyObject destroyObject;
    [SerializeField] SelectedObjectToBuild selectedObjectToBuild;
    [SerializeField] bool isBuilding;
    [SerializeField] float maxDistanceFromPlatformToPlayer;    

    [Header("Raycast set")]
    [SerializeField] float rayDistance;
    [SerializeField] LayerMask layerMaskForRaycast;

    public CreateGrid closestPlatformToPlayer;
    public CreateGrid closestPlatformToCursor;

    private bool previousIsBuilding;
    private SelectedObjectToBuild previousObjectToBuild;


    private enum SelectedObjectToBuild { Destroy, Platform, Wall, Building }



    void Start()
    {

    }



    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            isBuilding = !isBuilding;
        }

        if(previousIsBuilding != isBuilding)
        {
            if(previousIsBuilding)
            {
                CancelBuildOfLastSelectedObjectType();
            }
            previousIsBuilding = isBuilding;
        }

        if(isBuilding)
        {
            InputController();
            closestPlatformToPlayer = platformsController.FindClosestPlatform(gameObject.transform.position, maxDistanceFromPlatformToPlayer);
            if(selectedObjectToBuild == SelectedObjectToBuild.Building || selectedObjectToBuild == SelectedObjectToBuild.Platform)
            {
                RaycastHit hit;
                if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, layerMaskForRaycast))
                {
                    closestPlatformToCursor = platformsController.FindClosestPlatform(hit.point, maxDistanceFromPlatformToPlayer);
                }
            }

            if(selectedObjectToBuild != previousObjectToBuild)
            {
                CancelBuildOfLastSelectedObjectType();
                previousObjectToBuild = selectedObjectToBuild;
            }

            BuildSelectedObjectType();
        }
    }

    void InputController()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedObjectToBuild = SelectedObjectToBuild.Platform;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedObjectToBuild = SelectedObjectToBuild.Wall;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedObjectToBuild = SelectedObjectToBuild.Building;
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedObjectToBuild = SelectedObjectToBuild.Destroy;
        }
    }


    void BuildSelectedObjectType()
    {
        if(selectedObjectToBuild == SelectedObjectToBuild.Platform)
        {
            buildNewPlatform.Build(closestPlatformToPlayer, closestPlatformToCursor);
        }

        if(selectedObjectToBuild == SelectedObjectToBuild.Wall)
        {
            buildNewWall.Build(platformsController.FindClosestPlatforms(transform.position, maxDistanceFromPlatformToPlayer));
        }

        if(selectedObjectToBuild == SelectedObjectToBuild.Building)
        {
            buildNewBuilding.Build(closestPlatformToCursor);
        }

        if(selectedObjectToBuild == SelectedObjectToBuild.Destroy)
        {
            destroyObject.Destroy();
        }

    }

    void CancelBuildOfLastSelectedObjectType()
    {
        if(previousObjectToBuild == SelectedObjectToBuild.Platform)
        {
            buildNewPlatform.CancelBuild();
        }

        if(previousObjectToBuild == SelectedObjectToBuild.Wall)
        {
            buildNewWall.CancelBuild();
        }

        if(previousObjectToBuild == SelectedObjectToBuild.Building)
        {
            buildNewBuilding.CancelBuild();
        }
    }

}


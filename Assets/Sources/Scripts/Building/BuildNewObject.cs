using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildNewObject : MonoBehaviour
{
    public PlatformsController platformsController;
    public BuildNewPlatform buildNewPlatform;
    public BuildNewWall buildNewWall;
    public BuildNewBuilding buildNewBuilding;
    public DestroyObject destroyObject;
    
    public IBuild currentBuild;
    public Action OnBuildSet;
    public SelectedObjectToBuild selectedObjectToBuild, selectedObjectToBuildPrev;
    public bool isBuilding;
    public float maxDistanceFromPlatformToPlayer;

    [Header("Raycast set")]
    public float rayDistance;
    public LayerMask layerMaskForRaycast;

    private BuildParams buildParams;

    public enum SelectedObjectToBuild { Platform, Wall, Building, Destroy }

    private Dictionary<SelectedObjectToBuild, IBuild> builds;

    private void Start()
    {
        InitializeBuildDictionary();
    }

    private void InitializeBuildDictionary()
    {
        builds = new Dictionary<SelectedObjectToBuild, IBuild>
        {
            { SelectedObjectToBuild.Platform, buildNewPlatform },
            { SelectedObjectToBuild.Wall, buildNewWall },
            { SelectedObjectToBuild.Building, buildNewBuilding },
            { SelectedObjectToBuild.Destroy, destroyObject }
        };
    }

    void Update()
    {
        if (isBuilding)
        {
            BuildSelectedObjectType();
        }
    }

    public void EnterBuild()
    {
        isBuilding = true;
    }

    public void ExitBuild()
    {
        CancelBuildSelectedObjectType();
        isBuilding = false;
    }

    private void BuildSelectedObjectType()
    {
        if (TryGetCurrentBuild())
        {
            InitBuildParams();
            currentBuild.Build(buildParams);
        }
    }

    private bool TryGetCurrentBuild()
    {
        IBuild build;

        if (builds.TryGetValue(selectedObjectToBuild, out build))
        {
            currentBuild = build;

            if (selectedObjectToBuildPrev != selectedObjectToBuild)
            {
                selectedObjectToBuildPrev = selectedObjectToBuild;
                CancelBuildSelectedObjectType();
                OnBuildSet?.Invoke();

            }


            return true;
        }
        else
        {
            Debug.LogWarning($"Can't find {selectedObjectToBuild} in {nameof(builds)} dictionary");
            build = null;
            return false;
        }
    }

    private void InitBuildParams()
    {
        buildParams.ClosestPlatformsToPlayer = platformsController.FindClosestPlatforms(transform.position, maxDistanceFromPlatformToPlayer);
        buildParams.ClosestPlatformToCursor = FindClosestPlatformToCursor();
        buildParams.ClosestPlatformToPlayer = platformsController.FindClosestPlatform(transform.position, maxDistanceFromPlatformToPlayer);
    }

    private CreateGrid FindClosestPlatformToCursor()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, rayDistance, layerMaskForRaycast))
        {
            return platformsController.FindClosestPlatform(hit.point, maxDistanceFromPlatformToPlayer);
        }

        return null;
    }

    private void CancelBuildSelectedObjectType()
    {
        currentBuild?.CancelBuild();
    }
}

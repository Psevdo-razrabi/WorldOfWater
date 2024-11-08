using System.Collections.Generic;
using UnityEngine;

public interface IBuild
{
    public void Build(BuildParams buildParams);
    public void CancelBuild();
    public int SelectNextBuild();
    public int SelectPrevBuild();
    public Sprite GetPreviewSprite(int index);
    public int PreviewsCount();

}


public struct BuildParams
{   
    public List<CreateGrid> ClosestPlatformsToCursor;
    public List<CreateGrid> ClosestPlatformsToPlayer;
    public CreateGrid ClosestPlatformToCursor;
    public CreateGrid ClosestPlatformToPlayer;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int currentWorld;
    [SerializeField] private int currentRegion; // 1-1, 1-2, 1-3 ---- 1,1,1 - world || 1,2,3 - regions

    [SerializeField] private List<WorldDeta> worldData;

    public void LoadRegion(int currentWorld, int currentRegion)
    {
        foreach (var world in worldData)
        {
            if(world.worldNumber == currentWorld)
            {
                foreach (var region in world.regionData)
                {
                    if(region.RegionNumber == currentRegion)
                    {
                        region.Region.SetActive(true);
                        return;
                    }
                }

                Debug.LogError("Current Region Doesn't exist");
            }
        }

        Debug.LogError("Current World Doesn't exist");
    }
}

public struct WorldDeta
{
    public int worldNumber;
    public List<RegionData> regionData;
}

public struct RegionData
{
    public int RegionNumber;
    public GameObject Region;
}
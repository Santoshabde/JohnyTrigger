using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.JonnyTriggerProto
{
    public class LevelManager : SerializeSingleton<LevelManager>
    {
        [SerializeField] private int currentWorld;
        [SerializeField] private int currentRegion; // 1-1, 1-2, 1-3 ---- 1,1,1 - world || 1,2,3 - regions

        [SerializeField] private WorldData worldData;

        public void LoadRegion(int currentWorld, int currentRegion)
        {
            if (worldData.worldNumber == currentWorld)
            {
                foreach (var region in worldData.regionData)
                {
                    if (region.RegionNumber == currentRegion)
                    {
                        region.Region.SetActive(true);
                        return;
                    }
                }

                Debug.LogError("Current Region Doesn't exist");
            }

            Debug.LogError("Current World Doesn't exist");
        }

        public bool IsRegionCompleted(int currentRegion)
        {
            bool isRegionCompleted = true;

            RegionData? currentRegionData = GetCurrentRegionData(currentRegion);
            if (currentRegionData == null)
                return false;

            foreach (var zone in currentRegionData.Value.zonesInRegion)
            {
                if (!zone.isCompleted)
                {
                    isRegionCompleted = false;
                    break;
                }
            }

            return isRegionCompleted;
        }

        public RegionData? GetCurrentRegionData(int currentRegion)
        {
            foreach (var item in worldData.regionData)
            {
                if (item.RegionNumber == currentRegion)
                    return item;
            }

            return null;
        }
    }

    [System.Serializable]
    public struct WorldData
    {
        public int worldNumber;
        public List<RegionData> regionData;
    }

    [System.Serializable]
    public struct RegionData
    {
        public int RegionNumber;
        public List<ZoneManager> zonesInRegion;
        public GameObject Region;
    }
}
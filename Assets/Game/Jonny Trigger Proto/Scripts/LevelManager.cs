using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;
using Cinemachine;

namespace SNGames.JonnyTriggerProto
{
    public class LevelManager : SerializeSingleton<LevelManager>
    {
        [SerializeField] private int currentWorld;
        [SerializeField] private int currentRegion; // 1-1, 1-2, 1-3 ---- 1,1,1 - world || 1,2,3 - regions

        [SerializeField] private CharacterStateController characterInGame;
        [SerializeField] private Cinemachine.CinemachineVirtualCamera virtualCamera;

        [SerializeField] private WorldData worldData;

        [SerializeField, EditorButton("LoadLevelEditor", "LoadLevel", ButtonActivityType.Everything)] private bool isEditorBool;

        private void Awake()
        {
            SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.ON_REGION_COMPLETED, OnRegionCompleted);
        }

        private void Start()
        {
            LoadRegion(currentWorld, currentRegion);
        }

        private void OnDestroy()
        {
            SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.ON_REGION_COMPLETED, OnRegionCompleted);
        }

        public void LoadRegion(int currentWorld, int currentRegion)
        {
            //Deactivate all regions
            worldData.regionData.ForEach(region => region.Region.SetActive(false));

            //Activate current region
            if (worldData.worldNumber == currentWorld)
            {
                foreach (var region in worldData.regionData)
                {
                    if (region.RegionNumber == currentRegion)
                    {
                        //Activate Region
                        region.Region.SetActive(true);

                        //Set character and camera positions
                        characterInGame.transform.position = region.characterPosition;
                        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = region.cameraPosition;
                        return;
                    }
                }

                Debug.LogError("[Level Region and World Loading] Current Region Doesn't exist");
            }

            Debug.LogError("[Level Region and World Loading] Current World Doesn't exist");
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

        public bool IsCurrentRegionCompleted()
        {
            return IsRegionCompleted(currentRegion);
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

        private void OnRegionCompleted()
        {
            //Spawn Confitte
            Vector3 confitteSpawnPoint = GetCurrentRegionData(currentRegion).Value.confettiLocation.position;
            ParticleEffectsController.Instance.SpawnParticleEffect("WinConfetti", confitteSpawnPoint, Quaternion.Euler(-90, 0, 0));
        }

        private void LoadLevelEditor()
        {
            LoadRegion(currentWorld, currentRegion);
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
        public Transform confettiLocation;
        public Vector3 characterPosition;
        public Vector3 cameraPosition;
    }

    public enum InGameEvents
    {
        ON_REGION_COMPLETED,
        ON_WORLD_COMPLETED,
        ON_REGION_COMPLETION_FAILED,
        On_ZONE_COMPLETION_FAILED,
        ON_ZONE_COMPLETION_SUCCESS
    }
}
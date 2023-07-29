using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.JonnyTriggerProto
{
    public class CharacterPhysicsController : MonoBehaviour
    {
        [SerializeField] private CharacterStateController characterStateController;

        public static Action<ZoneManager> OnEnterZone;
        public static Action<ZoneManager> OnExitZone;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("[Physics] Collided with: " + other.name);
            ZoneEnterArea zoneEnterArea = other.GetComponent<ZoneEnterArea>();
            if (zoneEnterArea != null)
            {
                ZoneManager currentZoneManager = zoneEnterArea.GetCurrentZonesManagerAndSetData();
                currentZoneManager.OnCharacterEnterInZone(characterStateController);

                OnEnterZone?.Invoke(currentZoneManager);
            }

            ZoneExitArea zoneExitArea = other.GetComponent<ZoneExitArea>();
            if (zoneExitArea != null)
            {
                ZoneManager currentZoneManager = zoneExitArea.GetCurrentZonesManagerAndSetData();
                currentZoneManager.OnCharacterExitInZone(characterStateController);

                OnExitZone?.Invoke(currentZoneManager);
            }
        }
    }
}
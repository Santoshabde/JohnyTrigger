using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPhysicsController : MonoBehaviour
{
    public static Action<ZoneManager> OnEnterZone;
    public static Action<ZoneManager> OnExitZone;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        ZoneEnterArea zoneEnterArea = other.GetComponent<ZoneEnterArea>();
        if (zoneEnterArea != null)
        {
            ZoneManager currentZoneManager = zoneEnterArea.GetCurrentZonesManager();

            OnEnterZone?.Invoke(currentZoneManager);
        }

        ZoneExitArea zoneExitArea = other.GetComponent<ZoneExitArea>();
        if (zoneExitArea != null)
        {
            ZoneManager currentZoneManager = zoneExitArea.GetCurrentZonesManager();
            OnExitZone?.Invoke(currentZoneManager);
        }
    }
}

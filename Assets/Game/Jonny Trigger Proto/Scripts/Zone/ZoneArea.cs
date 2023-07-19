using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneArea : MonoBehaviour
{
    [SerializeField] protected ZoneManager zoneManager;

    public ZoneManager GetCurrentZonesManager() => zoneManager;
}

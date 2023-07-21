using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.JonnyTriggerProto
{
    public class ZoneArea : MonoBehaviour
    {
        [SerializeField] protected ZoneManager zoneManager;

        public ZoneManager GetCurrentZonesManagerAndSetData() => zoneManager;
    }
}
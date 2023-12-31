using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;

namespace SNGames.JonnyTriggerProto
{
    public class LevelEndDetector : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<CharacterStateController>())
            {
                if(LevelManager.Instance.IsCurrentRegionCompleted())
                {
                    SNEventsController<InGameEvents>.TriggerEvent(InGameEvents.ON_REGION_COMPLETED);
                }
                else
                {
                    SNEventsController<InGameEvents>.TriggerEvent(InGameEvents.ON_REGION_COMPLETION_FAILED);
                }    
            }
        }
    }
}
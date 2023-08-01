using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.JonnyTriggerProto
{
    public class LevelEndDetector : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<CharacterStateController>())
            {
                if(LevelManager.Instance.IsRegionCompleted(1))
                {
                    Time.timeScale = 0;
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;

namespace SNGames.JonnyTriggerProto
{
    public class InputController : Subject<InputData>
    {
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Mouse0)
                || Input.GetMouseButtonDown(0))
            {
                NotifyAllObservers(new InputData() { inputID = "Shoot" });
            }
        }
    }

    public struct InputData
    {
        public string inputID;
    }
}

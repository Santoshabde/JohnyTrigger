using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.JonnyTriggerProto
{
    public class InGameServiceBinder : ServiceBinder
    {
        protected override void BindAllServicesInGame()
        {
            BindService(new LocalStorage());
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ServiceBinder : MonoBehaviour
{
    private void Awake()
    {
        BindAllServicesInGame();
    }

    /// <summary>
    /// Binds All Services at the start of the game
    /// </summary>
    protected virtual void BindAllServicesInGame()
    {

    }
}

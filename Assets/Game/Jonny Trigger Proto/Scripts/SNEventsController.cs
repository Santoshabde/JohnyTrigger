using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SNEventsController
{
    public static Dictionary<InGameEvents, Action> eventsInGame = new Dictionary<InGameEvents, Action>();

    public static void RegisterEvent(InGameEvents eventName, Action actionCallBack)
    {
        if(eventsInGame == null)
            eventsInGame = new Dictionary<InGameEvents, Action>();

        if (!eventsInGame.ContainsKey(eventName))
            eventsInGame.Add(eventName, actionCallBack);
        else
            eventsInGame[eventName] += actionCallBack;
    }

    public static void TriggerEvent(InGameEvents eventName)
    {
        eventsInGame[eventName]?.Invoke();
    }

    public static void DeregisterEvent(InGameEvents eventName, Action listerner)
    {
        eventsInGame[eventName] -= listerner;
    }
}

public enum InGameEvents
{
    ON_REGION_COMPLETED,
    ON_WORLD_COMPLETED
}

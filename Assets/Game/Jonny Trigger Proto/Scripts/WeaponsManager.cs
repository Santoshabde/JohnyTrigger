using SNGames.JonnyTriggerProto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    [SerializeField] private LineRenderer aimLineRenderer;

    private void Start()
    {
        CharacterState_BattleMode.OnDecideCharacterShootFunctionality += OnDecideCharacterShootFunctionality;
    }

    private void OnDecideCharacterShootFunctionality(bool shoudEnable, Vector3 startingPosition, Vector3 finalPosition)
    {
        if (shoudEnable)
        {
            if (!aimLineRenderer.enabled)
                aimLineRenderer.enabled = true;

            aimLineRenderer.SetPosition(0, startingPosition);
            aimLineRenderer.SetPosition(1, finalPosition);
        }else
        {
            if (aimLineRenderer.enabled)
                aimLineRenderer.enabled = false;
        }
    }

    private void OnDestroy()
    {
        CharacterState_BattleMode.OnDecideCharacterShootFunctionality -= OnDecideCharacterShootFunctionality;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;
using System;
using DG.Tweening;

namespace SNGames.JonnyTriggerProto
{
    public class CharacterState_BattleMode_WithoutJumpCurve : State
    {
        private CharacterStateController characterStateController;
        private ZoneManager currentZone;
        private ZoneDataOutput zoneDataOutput;
        private Coroutine jumpCurveCoroutine;
        private bool inBattleZone = false;

        public static Action<bool, Vector3, Vector3> OnDecideCharacterShootFunctionality;

        public CharacterState_BattleMode_WithoutJumpCurve(CharacterStateController characterStateController, ZoneManager currentZone)
        {
            this.characterStateController = characterStateController;
            this.currentZone = currentZone;
            zoneDataOutput = currentZone.GetCurrentZoneData();
        }

        public override void Enter()
        {
            if (jumpCurveCoroutine != null) characterStateController.StopCoroutine(jumpCurveCoroutine);
            jumpCurveCoroutine = characterStateController.StartCoroutine(PlayJumpCurveAnimation());
        }

        public override void Exit()
        {
            
        }

        public override void Tick(float deltaTime)
        {
            characterStateController.CharacterController.Move(new Vector3(0, -deltaTime * characterStateController.CharacterGravityForceActing,  deltaTime * (inBattleZone? 18f : characterStateController.CharacterRunSpeed)));
        }

        private IEnumerator PlayJumpCurveAnimation()
        {
            //Wait for some initial buffer time
            yield return new WaitForSeconds(zoneDataOutput.initialWaitTimeBeforeSlowing);

            //characterStateController.lefthandChainIkContraint.weight = 1;
            characterStateController.righthandChainIkContraint.weight = 1;
            characterStateController.headIKConstaint.weight = 1;

            //characterStateController.leftHandChainIkTarget.position = currentZone.GetCurrentAimAtPointOnAimAtCurve();
            characterStateController.rightHandChainIkTarget.position = currentZone.GetCurrentAimAtPointOnAimAtCurve();
            characterStateController.headIkTarget.position = currentZone.GetCurrentAimAtPointOnAimAtCurve();

            //Tween the time scale from 1 to desired value
            Time.timeScale = zoneDataOutput.jumpSlowMotionTimeScale;
            Time.fixedDeltaTime = 0.0009f;
            Physics.gravity = Physics.gravity * 40f;

            //Activating zone - moving toAim curve here
            currentZone.ActivateZone();
            inBattleZone = true;

            while (currentZone.GetCurrentAimCurveEvalutationTime() < 0.80f)
            {
                //characterStateController.leftHandChainIkTarget.position = currentZone.GetCurrentAimAtPointOnAimAtCurve();
                characterStateController.rightHandChainIkTarget.position = currentZone.GetCurrentAimAtPointOnAimAtCurve();
                characterStateController.headIkTarget.position = currentZone.GetCurrentAimAtPointOnAimAtCurve();

                if (currentZone.GetCurrentAimCurveEvalutationTime() > 0.10f)
                    OnDecideCharacterShootFunctionality?.Invoke(true, characterStateController.leftHandTransform.position, currentZone.GetCurrentAimAtPointOnAimAtCurve());

                yield return null;
            }

            inBattleZone = false;

            //characterStateController.lefthandChainIkContraint.weight = 0;
            characterStateController.righthandChainIkContraint.weight = 0;
            characterStateController.headIKConstaint.weight = 0;

            //Tween the time scale from 1 to desired value
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            Physics.gravity = new Vector3(0, -9.8f, 0);

            OnDecideCharacterShootFunctionality?.Invoke(false, characterStateController.leftHandTransform.position, currentZone.GetCurrentAimAtPointOnAimAtCurve());
        }
    }
}
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

        public static Action<bool, Vector3, ZoneManager> OnDecideCharacterShootFunctionality;

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
            characterStateController.CharacterController.Move(new Vector3(0, -deltaTime * characterStateController.CharacterGravityForceActing,
                deltaTime * (inBattleZone ? zoneDataOutput.characterMovementSpeedInNoJumpCurveZoneInSlowMo : characterStateController.CharacterRunSpeed)));
        }

        private IEnumerator PlayJumpCurveAnimation()
        {
            //Wait for some initial buffer time
            yield return new WaitForSeconds(zoneDataOutput.initialWaitTimeBeforeSlowing);

            //Activating zone - moving toAim curve here
            currentZone.ActivateZone();

            float value = 0;
            DOTween.To(() => value, x => value = x, 1, 0.1f)
                .OnUpdate(() =>
                {
                    if (zoneDataOutput.enableHeadIKToAimCurveTarget)
                    {
                        characterStateController.headIKConstaint.weight = value;
                        characterStateController.headIkTarget.position = currentZone.GetCurrentAimAtPointOnAimAtCurve();
                    }
                });

            if (zoneDataOutput.enableLeftHandIKToAimCurveTarget)
            {
                characterStateController.lefthandChainIkContraint.weight = 1;

                characterStateController.lefthandChainIkContraint.data.tipRotationWeight = zoneDataOutput.onZoneEnterIkTipRotationWeight;
                characterStateController.lefthandChainIkContraint.data.chainRotationWeight = zoneDataOutput.onZoneEnterIkChainRotationWeight;

                characterStateController.leftHandChainIkTarget.position = currentZone.GetCurrentAimAtPointOnAimAtCurve();
            }

            if (zoneDataOutput.enableRightHandIKToAimCurveTarget)
            {
                characterStateController.righthandChainIkContraint.weight = 1;

                characterStateController.righthandChainIkContraint.data.tipRotationWeight = zoneDataOutput.onZoneEnterIkTipRotationWeight;
                characterStateController.righthandChainIkContraint.data.chainRotationWeight = zoneDataOutput.onZoneEnterIkChainRotationWeight;

                characterStateController.rightHandChainIkTarget.position = currentZone.GetCurrentAimAtPointOnAimAtCurve();
            }

            //Tween the time scale from 1 to desired value
            Time.timeScale = zoneDataOutput.jumpSlowMotionTimeScale;
            Time.fixedDeltaTime = zoneDataOutput.fixedDeltaTimeScale;
            Physics.gravity = Physics.gravity * 40f;

            inBattleZone = true;

            while (currentZone.GetCurrentAimCurveEvalutationTime() < 0.98f)
            {
                characterStateController.leftHandChainIkTarget.position = currentZone.GetCurrentAimAtPointOnAimAtCurve();
                characterStateController.rightHandChainIkTarget.position = currentZone.GetCurrentAimAtPointOnAimAtCurve();
                characterStateController.headIkTarget.position = currentZone.GetCurrentAimAtPointOnAimAtCurve();

                if (currentZone.GetCurrentAimCurveEvalutationTime() > zoneDataOutput.aimCurveEvaluationTimeToEnableAim)
                    OnDecideCharacterShootFunctionality?.Invoke(true, characterStateController.leftHandTransform.position, currentZone);

                yield return null;
            }

            inBattleZone = false;

            characterStateController.lefthandChainIkContraint.weight = 0;
            characterStateController.righthandChainIkContraint.weight = 0;
            characterStateController.headIKConstaint.weight = 0;

            //Tween the time scale from 1 to desired value
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            Physics.gravity = new Vector3(0, -9.8f, 0);

            OnDecideCharacterShootFunctionality?.Invoke(false, characterStateController.leftHandTransform.position, currentZone);

            characterStateController.SwitchState(new CharacterState_Run(characterStateController, false));
        }
    }
}
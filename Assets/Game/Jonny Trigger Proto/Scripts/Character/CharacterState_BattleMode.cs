using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;
using System;
using DG.Tweening;

namespace SNGames.JonnyTriggerProto
{
    public class CharacterState_BattleMode : State
    {
        private CharacterStateController characterStateController;
        private ZoneManager currentZone;
        private ZoneDataOutput zoneDataOutput;
        private Coroutine jumpCurveCoroutine;

        public static Action<bool, Vector3, Vector3> OnDecideCharacterShootFunctionality;

        public CharacterState_BattleMode(CharacterStateController characterStateController, ZoneManager currentZone)
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
            if (jumpCurveCoroutine != null)
                characterStateController.StopCoroutine(jumpCurveCoroutine);

            characterStateController.lefthandChainIkContraint.weight = 0;
            characterStateController.righthandChainIkContraint.weight = 0;
        }

        public override void Tick(float deltaTime)
        {

        }

        float jumpCurveEvalutationTime = 0;
        private IEnumerator PlayJumpCurveAnimation()
        {
            jumpCurveEvalutationTime = 0;
            Vector3 finalPosition = zoneDataOutput.zoneJumpPathSpline.EvaluatePosition(0);

            characterStateController.CharacterAnimator.CrossFade("StartingFlip", 0.1f);

            //Wait for character to reach eval(0) jump curve position
            while ((characterStateController.transform.position - finalPosition).magnitude > 0.1f)
            {
                characterStateController.transform.position = Vector3.MoveTowards(characterStateController.transform.position, finalPosition, Time.deltaTime);
                yield return null;
            }

            characterStateController.CharacterAnimator.CrossFade("Flip", 0.3f);

            //Wait for some initial buffer time
            yield return new WaitForSeconds(zoneDataOutput.initialWaitTimeBeforeSlowing);
   
            //Tween the time scale from 1 to desired value
            Time.timeScale = zoneDataOutput.jumpSlowMotionTimeScale;
            Debug.Log("B4: " + Time.fixedDeltaTime);
            Time.fixedDeltaTime = 0.003f;
            Physics.gravity = Physics.gravity * 10f;
            Debug.Log("After: " + Time.fixedDeltaTime);

            //Activating zone - moving toAim curve here
            currentZone.ActivateZone();

            characterStateController.lefthandChainIkContraint.weight = 1;
            characterStateController.righthandChainIkContraint.weight = 1;

            while (true)
            {
                finalPosition = zoneDataOutput.zoneJumpPathSpline.EvaluatePosition(jumpCurveEvalutationTime);

                if (currentZone.GetCurrentAimCurveEvalutationTime() < 0.98f)
                {
                    characterStateController.leftHandChainIkTarget.position = currentZone.GetCurrentAimAtPointOnAimAtCurve();
                    characterStateController.rightHandChainIkTarget.position = currentZone.GetCurrentAimAtPointOnAimAtCurve();

                    OnDecideCharacterShootFunctionality?.Invoke(true, characterStateController.leftHandTransform.position, currentZone.GetCurrentAimAtPointOnAimAtCurve());
                }
                else
                {
                    characterStateController.lefthandChainIkContraint.weight = Mathf.Lerp(characterStateController.lefthandChainIkContraint.weight, 0, Time.deltaTime * 15f);
                    characterStateController.righthandChainIkContraint.weight = Mathf.Lerp(characterStateController.righthandChainIkContraint.weight, 0, Time.deltaTime * 15f);

                    OnDecideCharacterShootFunctionality?.Invoke(false, characterStateController.leftHandTransform.position, currentZone.GetCurrentAimAtPointOnAimAtCurve());
                }

                if ((characterStateController.transform.position - finalPosition).magnitude < 0.1f)
                {
                    jumpCurveEvalutationTime += Time.deltaTime * zoneDataOutput.jumpCurveSpeed;
                }

                characterStateController.transform.position = Vector3.MoveTowards(characterStateController.transform.position, finalPosition, Time.deltaTime * zoneDataOutput.jumpCurveSpeed);

                if (jumpCurveEvalutationTime >= 0.98f)
                {
                    Time.timeScale = 1;
                    Time.fixedDeltaTime = 0.02f;
                    Physics.gravity = new Vector3(0,-9.8f, 0);
                    characterStateController.SwitchState(new CharacterState_Run(characterStateController));
                }

                yield return null;
            }
        }

        private float GetCurretCharacterMoveSpeed(Vector3 finalposition)
        {
            if (finalposition == (Vector3)zoneDataOutput.zoneJumpPathSpline.EvaluatePosition(0))
                return 15f;

            return zoneDataOutput.jumpCurveSpeed;
        }
    }
}

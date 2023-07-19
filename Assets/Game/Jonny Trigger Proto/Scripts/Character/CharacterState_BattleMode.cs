using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;
using System;

namespace SNGames.JonnyTriggerProto
{
    public class CharacterState_BattleMode : State
    {
        private CharacterStateController characterStateController;
        private ZoneManager currentZone;
        private ZoneDataOutput zoneDataOutput;
        private Coroutine jumpCurveCoroutine;

        public static Action<bool, Vector3, Vector3> OnDecideCharacterShootFunctionality;

        public CharacterState_BattleMode(CharacterStateController characterStateController, ZoneManager currentZone, ZoneDataOutput zoneDataOutput)
        {
            this.characterStateController = characterStateController;
            this.zoneDataOutput = zoneDataOutput;
            this.currentZone = currentZone;
        }

        public override void Enter()
        {
            characterStateController.CharacterAnimator.CrossFade("Flip", 0.1f);

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
            yield return new WaitForSeconds(zoneDataOutput.initialWaitTimeBeforeSlowing);

            jumpCurveEvalutationTime = 0;
            Vector3 finalPosition = zoneDataOutput.zoneJumpPathSpline.EvaluatePosition(0);

            Time.timeScale = zoneDataOutput.jumpSlowMotionTimeScale;

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

                if(jumpCurveEvalutationTime >= 0.98f)
                {
                    Time.timeScale = 1;
                    characterStateController.SwitchState(new CharacterState_Run(characterStateController));
                }

                yield return null;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;

namespace SNGames.JonnyTriggerProto
{
    public class CharacterState_Run : State
    {
        private CharacterStateController characterStateController;
        private bool shouldRollBeforeRun;
        private bool startedRoll;

        public CharacterState_Run(CharacterStateController characterStateController, bool shouldRollBeforeRun)
        {
            this.characterStateController = characterStateController;
            this.shouldRollBeforeRun = shouldRollBeforeRun;
        }

        public override void Enter()
        {
            if (shouldRollBeforeRun)
            {
                characterStateController.StartCoroutine(RollAndRun());

                IEnumerator RollAndRun()
                {
                    startedRoll = true;
                    characterStateController.CharacterAnimator.CrossFade("Landing", 0.4f);

                    yield return new WaitForSeconds(2f);

                    startedRoll = false;
                    characterStateController.CharacterAnimator.CrossFade("Run", 0.1f);
                }
            }
            else
            {
                characterStateController.CharacterAnimator.CrossFade("Run", 0.1f);
            }

            CharacterPhysicsController.OnEnterZone += OnZoneEnter;
        }

        public override void Exit()
        {
            CharacterPhysicsController.OnEnterZone -= OnZoneEnter;
        }

        public override void Tick(float deltaTime)
        {
            characterStateController.CharacterController.Move(new Vector3(0, -deltaTime * characterStateController.CharacterGravityForceActing, deltaTime * GetCurrentCharacterSpeed()));
        }

        private void OnZoneEnter(ZoneManager currentZone)
        {
            characterStateController.SwitchState(new CharacterState_BattleMode(characterStateController, currentZone));
        }

        private float GetCurrentCharacterSpeed() => startedRoll ? 0f : characterStateController.CharacterRunSpeed;
    }
}
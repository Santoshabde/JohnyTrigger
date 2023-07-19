using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;

namespace SNGames.JonnyTriggerProto
{
    public class CharacterState_Run : State
    {
        private CharacterStateController characterStateController;
        public CharacterState_Run(CharacterStateController characterStateController)
        {
            this.characterStateController = characterStateController;
        }

        public override void Enter()
        {
            characterStateController.CharacterAnimator.CrossFade("Run", 0.1f);

            CharacterPhysicsController.OnEnterZone += OnZoneEnter;
        }

        public override void Exit()
        {
            CharacterPhysicsController.OnEnterZone -= OnZoneEnter;
        }

        public override void Tick(float deltaTime)
        {
            characterStateController.CharacterController.Move(new Vector3(0, -deltaTime * characterStateController.CharacterGravityForceActing, deltaTime * characterStateController.CharacterRunSpeed));
        }

        private void OnZoneEnter(ZoneManager currentZone, ZoneDataOutput zoneDataOutput)
        {
            characterStateController.SwitchState(new CharacterState_BattleMode(characterStateController, currentZone, zoneDataOutput));
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;

namespace SNGames.JonnyTriggerProto
{
    public class CharacterState_Idle : State
    {
        private CharacterStateController characterStateController;

        public CharacterState_Idle(CharacterStateController characterStateController)
        {
            this.characterStateController = characterStateController;
        }

        public override void Enter()
        {
            characterStateController.CharacterAnimator.CrossFade("Idle", 0.4f);
        }

        public override void Exit()
        {

        }

        public override void Tick(float deltaTime)
        {

        }
    }
}
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
            characterStateController.CharacterAnimator.CrossFade("Run", 0.02f);
        }

        public override void Exit()
        {

        }

        public override void Tick(float deltaTime)
        {
            characterStateController.CharacterController.Move(new Vector3(0, 0, deltaTime * 3f));
        }
    }
}
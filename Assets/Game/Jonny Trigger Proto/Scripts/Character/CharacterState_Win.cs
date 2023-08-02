using SNGames.CommonModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.JonnyTriggerProto
{
    public class CharacterState_Win : State
    {
        private CharacterStateController characterStateController;

        public CharacterState_Win(CharacterStateController characterStateController)
        {
            this.characterStateController = characterStateController;
        }

        public override void Enter()
        {
            characterStateController.CharacterAnimator.CrossFade("Ninja Idle", 0.2f);
        }

        public override void Exit()
        {

        }

        public override void Tick(float deltaTime)
        {

        }
    }
}

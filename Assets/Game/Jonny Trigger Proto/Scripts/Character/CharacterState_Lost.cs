using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;

namespace SNGames.JonnyTriggerProto
{
    public class CharacterState_Lost : State
    {
        private CharacterStateController characterStateController;
        public CharacterState_Lost(CharacterStateController characterStateController)
        {
            this.characterStateController = characterStateController;
        }

        public override void Enter()
        {
            characterStateController.CharacterAnimator.CrossFade("Defeated", 0.3f);
        }

        public override void Exit()
        {
           
        }

        public override void Tick(float deltaTime)
        {
            
        }
    }
}

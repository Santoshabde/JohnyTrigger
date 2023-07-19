using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;

namespace SNGames.JonnyTriggerProto
{
    public class CharacterStateController : StateMachine
    {
        [SerializeField] private Animator characterAnimator;
        [SerializeField] private CharacterController characterController;

        public Animator CharacterAnimator => characterAnimator;
        public CharacterController CharacterController => characterController;

        private void Start()
        {
            SwitchState(new CharacterState_Run(this));
        }
    }
}
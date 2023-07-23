using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;
using UnityEngine.Animations.Rigging;

namespace SNGames.JonnyTriggerProto
{
    public class CharacterStateController : StateMachine
    {
        [SerializeField] private Animator characterAnimator;
        [SerializeField] private CharacterController characterController;
        [SerializeField] public ChainIKConstraint lefthandChainIkContraint;
        [SerializeField] public Transform leftHandChainIkTarget;
        [SerializeField] public ChainIKConstraint righthandChainIkContraint;
        [SerializeField] public Transform rightHandChainIkTarget;
        [SerializeField] public MultiAimConstraint headIKConstaint;
        [SerializeField] public Transform headIkTarget;
        [SerializeField] public Transform leftHandTransform;

        //Need to segreagate with configs
        [SerializeField] private float characterRunSpeed;
        [SerializeField] private float characterGravityForceActing;

        public Animator CharacterAnimator => characterAnimator;
        public CharacterController CharacterController => characterController;
        public float CharacterRunSpeed => characterRunSpeed;
        public float CharacterGravityForceActing => characterGravityForceActing;

        private void Start()
        {
            SwitchState(new CharacterState_Run(this, false));
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.Animations.Rigging;

namespace SNGames.JonnyTriggerProto
{
    public enum EnemyState
    {
        Idle,
        Combat,
        Death
    }

    public class BaseEnemy : MonoBehaviour, IDamagable
    {
        [SerializeField] protected MultiAimConstraint headMultiAimConstraint;
        [SerializeField] protected Transform headIKTarget;
        [SerializeField] protected Transform rightHand;
        [SerializeField] protected ChainIKConstraint righthandChainIkConstaint;
        [SerializeField] protected Transform rightHandIKTarget;
        [SerializeField] protected Transform leftHand;
        [SerializeField] protected ChainIKConstraint lefthandChainIkConstaint;
        [SerializeField] protected Transform leftHandIKTarget;

        private void Awake()
        {
            DisableRagDoll();
        }

        public virtual void OnDamage(float damageAmount, Vector3 damageDirection)
        {
            
        }

        public virtual void OnCharacterEnteredTheZone(CharacterStateController characterInZone)
        {
            
        }

        public virtual void OnCharacterExitTheZone(CharacterStateController characterInZone)
        {

        }

        protected void DisableRagDoll()
        {
            foreach (var item in transform.GetComponentsInChildren<Rigidbody>())
            {
                item.isKinematic = true;
            }
        }

        protected void EnableRagDoll()
        {
            foreach (var item in transform.GetComponentsInChildren<Rigidbody>())
            {
                item.isKinematic = true;
            }
        }
    }
}

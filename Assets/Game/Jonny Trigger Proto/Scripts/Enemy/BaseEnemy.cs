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
        [SerializeField, Disable] private EnemyState enemyState;
        [SerializeField] protected Animator animtor;

        protected virtual void Awake()
        {
            enemyState = EnemyState.Idle;
            DisableRagDoll();
        }

        public virtual void OnDamage(Rigidbody rigidBodyHit, float damageAmount, Vector3 damageDirection)
        {
            ParticleEffectsController.Instance.SpawnParticleEffect("Hit_Blood", rigidBodyHit.position, Quaternion.identity);
            enemyState = EnemyState.Death;
            animtor.enabled = false;
            EnableRagDoll();
            rigidBodyHit.AddForce(damageDirection * damageAmount, ForceMode.Impulse);
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
                item.isKinematic = false;
            }
        }

        public EnemyState GetEnemyState() => enemyState;
    }
}

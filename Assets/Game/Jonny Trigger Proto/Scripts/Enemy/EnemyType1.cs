using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace SNGames.JonnyTriggerProto
{
    public class EnemyType1 : BaseEnemy
    {
        [SerializeField] protected MultiAimConstraint headMultiAimConstraint;
        [SerializeField] protected Transform headIKTarget;
        [SerializeField] protected Transform rightHand;
        [SerializeField] protected ChainIKConstraint righthandChainIkConstaint;
        [SerializeField] protected Transform rightHandIKTarget;
        [SerializeField] protected Transform leftHand;
        [SerializeField] protected ChainIKConstraint lefthandChainIkConstaint;
        [SerializeField] protected Transform leftHandIKTarget;

        private Coroutine characterEnterCoroutine;

        public override void OnCharacterEnteredTheZone(CharacterStateController characterInZone)
        {
            float headWeightValue = 0f;
            DOTween.To(() => headWeightValue, x => headWeightValue = x, 1f, 2f)
                .OnUpdate(() => {
                    headMultiAimConstraint.weight = 0;
                });

            float distanceFromRightHandToCharacter = (characterInZone.transform.position - rightHand.transform.position).magnitude;
            float distanceFromLeftHandToCharacter = (characterInZone.transform.position - leftHand.transform.position).magnitude;

            bool useRightHand;
            if(distanceFromLeftHandToCharacter > distanceFromRightHandToCharacter)
            {
                useRightHand = true;

                float handIKValue = 0f;
                DOTween.To(() => handIKValue, x => handIKValue = x, 1f, 1f)
                    .OnUpdate(() => {
                        righthandChainIkConstaint.weight = handIKValue;
                    });
            }
            else
            {
                useRightHand = false;

                float handIKValue = 0f;
                DOTween.To(() => handIKValue, x => handIKValue = x, 1f, 1f)
                    .OnUpdate(() => {
                        lefthandChainIkConstaint.weight = handIKValue;
                    });
            }

            if (characterEnterCoroutine != null)
                StopCoroutine(characterEnterCoroutine);

            characterEnterCoroutine = StartCoroutine(OnCharacterEntered());

            IEnumerator OnCharacterEntered()
            {
                while (true)
                {
                    headIKTarget.position = characterInZone.transform.position;

                    if (useRightHand)
                        rightHandIKTarget.position = characterInZone.transform.position;
                    else
                        leftHandIKTarget.position = characterInZone.transform.position;

                    yield return null;
                }
            }
        }

        public override void OnCharacterExitTheZone(CharacterStateController characterInZone)
        {
            if (characterEnterCoroutine != null)
                StopCoroutine(characterEnterCoroutine);

            lefthandChainIkConstaint.weight = 0;
            righthandChainIkConstaint.weight = 0;
            headMultiAimConstraint.weight = 0;
        }
    }
}
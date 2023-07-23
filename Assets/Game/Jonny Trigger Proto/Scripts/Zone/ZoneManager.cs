using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace SNGames.JonnyTriggerProto
{
    public class ZoneManager : MonoBehaviour
    {
        [Help("Zone jump settings are mandotory, only enable if you have jump curve for character", UnityMessageType.Info, Order = -1)]
        [Label("Zone Jump Settings", skinStyle: SkinStyle.Round, Alignment = TextAnchor.MiddleCenter)]
        [SerializeField] 
        private bool hasJumpCurve;
        [SerializeField, DisableIf(nameof(hasJumpCurve), false, Comparison = UnityComparisonMethod.Equal)]
        private SplineContainer zoneJumpPathSpline;
        [SerializeField, DisableIf(nameof(hasJumpCurve), false, Comparison = UnityComparisonMethod.Equal)] 
        private string animationToPlay;
        [SerializeField, DisableIf(nameof(hasJumpCurve), false, Comparison = UnityComparisonMethod.Equal)] 
        private float jumpCurveSpeed;

        [Help("Aim at curve is mandatory - for character aim to follow in the zone", UnityMessageType.Info, Order = -1)]
        [Label("Aim At Curve Settings", skinStyle: SkinStyle.Round, Alignment = TextAnchor.MiddleCenter)]
        [SerializeField, NotNull] private SplineContainer zoneAimPathSpline;
        [SerializeField] private float aimAtTransformSpeedOnCurve;

        [Label("Zone Movement Settings", skinStyle: SkinStyle.Round, Alignment = TextAnchor.MiddleCenter)]
        [SerializeField, DisableIf(nameof(hasJumpCurve), true, Comparison = UnityComparisonMethod.Equal)] private float characterMovementSpeedInNoJumpCurveZoneInSlowMo;
        [SerializeField, Range(0, 1)] private float jumpSlowMotionTimeScale;
        [SerializeField, Tooltip("By default - 1/60 = 0.02")] private float fixedDeltaTimeScale;
        [SerializeField] private float initialWaitTimeBeforeSlowing;
        [SerializeField, Range(0,1)] private float aimCurveEvaluationTimeToEnableAim;
        [SerializeField, ReorderableList] private List<BaseEnemy> enemiesInZone;

        [Label("Zone Enter character IK settings", skinStyle: SkinStyle.Round, Alignment = TextAnchor.MiddleCenter)]
        [SerializeField] private bool enableLeftHandIKToAimCurveTarget;
        [SerializeField] private bool enableRightHandIKToAimCurveTarget;
        [SerializeField] private bool enableHeadIKToAimCurveTarget;

        private Vector3 aimAtPoint;
        private Coroutine aimAtCurveCoroutine;
        private float aimCurveEvalutationTime = 0;
        private CharacterStateController currentCharacterInTheZone;

        public ZoneDataOutput GetCurrentZoneData() => new ZoneDataOutput()
        {
            animationToPlay = animationToPlay,
            initialWaitTimeBeforeSlowing = initialWaitTimeBeforeSlowing,
            jumpCurveSpeed = jumpCurveSpeed,
            jumpSlowMotionTimeScale = jumpSlowMotionTimeScale,
            zoneJumpPathSpline = zoneJumpPathSpline,
            aimCurveEvaluationTimeToEnableAim = aimCurveEvaluationTimeToEnableAim,
            enemiesInZone = enemiesInZone,
            fixedDeltaTimeScale = fixedDeltaTimeScale,
            enableHeadIKToAimCurveTarget = enableHeadIKToAimCurveTarget,
            enableLeftHandIKToAimCurveTarget = enableLeftHandIKToAimCurveTarget,
            enableRightHandIKToAimCurveTarget = enableRightHandIKToAimCurveTarget,
            characterMovementSpeedInNoJumpCurveZoneInSlowMo = characterMovementSpeedInNoJumpCurveZoneInSlowMo
        };

        public Vector3 GetCurrentAimAtPointOnAimAtCurve() => aimAtPoint;

        public float GetCurrentAimCurveEvalutationTime() => aimCurveEvalutationTime;

        public void ActivateZone()
        {
            aimAtPoint = zoneAimPathSpline.EvaluatePosition(0);

            if (aimAtCurveCoroutine != null) StopCoroutine(aimAtCurveCoroutine);
            aimAtCurveCoroutine = StartCoroutine(StartAimAtCurveCoroutine());

            IEnumerator StartAimAtCurveCoroutine()
            {
                aimCurveEvalutationTime = 0;
                while (true)
                {
                    aimAtPoint = zoneAimPathSpline.EvaluatePosition(aimCurveEvalutationTime);
                    aimCurveEvalutationTime += Time.deltaTime * aimAtTransformSpeedOnCurve;
                    yield return null;
                }
            }
        }

        public void OnCharacterEnterInZone(CharacterStateController characterStateController)
        {
            currentCharacterInTheZone = characterStateController;
            enemiesInZone.ForEach(enemy => enemy.OnCharacterEnteredTheZone(currentCharacterInTheZone));
        }

        public void OnCharacterExitInZone(CharacterStateController characterStateController)
        {
            currentCharacterInTheZone = null;
            enemiesInZone.ForEach(enemy => enemy.OnCharacterExitTheZone(characterStateController));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(aimAtPoint, 0.4f);
        }
    }

    public struct ZoneDataOutput
    {
        public SplineContainer zoneJumpPathSpline;
        public float initialWaitTimeBeforeSlowing;
        public string animationToPlay;
        public float jumpSlowMotionTimeScale;
        public float jumpCurveSpeed;
        public float fixedDeltaTimeScale;
        public float aimCurveEvaluationTimeToEnableAim;
        public List<BaseEnemy> enemiesInZone;
        public bool enableLeftHandIKToAimCurveTarget;
        public bool enableRightHandIKToAimCurveTarget;
        public bool enableHeadIKToAimCurveTarget;
        public float characterMovementSpeedInNoJumpCurveZoneInSlowMo;
    }
}
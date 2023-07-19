using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class ZoneManager : MonoBehaviour
{
    [SerializeField] private SplineContainer zoneJumpPathSpline;
    [SerializeField] private SplineContainer zoneAimPathSpline;
    [SerializeField] private string animationToPlay;
    [SerializeField, Range(0,1)] private float jumpSlowMotionTimeScale;
    [SerializeField] private float jumpCurveSpeed;
    [SerializeField] private float initialWaitTimeBeforeSlowing;
    [SerializeField] private float aimAtTransformSpeedOnCurve;

    private Vector3 aimAtPoint;
    private Coroutine aimAtCurveCoroutine;
    private float aimCurveEvalutationTime = 0;

    public ZoneDataOutput GetCurrentZoneData() => new ZoneDataOutput() 
    { animationToPlay = animationToPlay,
        initialWaitTimeBeforeSlowing = initialWaitTimeBeforeSlowing,
        jumpCurveSpeed = jumpCurveSpeed,
        jumpSlowMotionTimeScale = jumpSlowMotionTimeScale ,
        zoneJumpPathSpline = zoneJumpPathSpline
    };

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

    public Vector3 GetCurrentAimAtPointOnAimAtCurve() => aimAtPoint;
    public float GetCurrentAimCurveEvalutationTime() => aimCurveEvalutationTime;

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
}

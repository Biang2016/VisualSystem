using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CameraFollowPath : MonoBehaviour
{
    public BezierPath BezierPath;
    public float Speed = 5f;
    private Vector3[] pathPoints;
    public Transform LookAtTarget;

    void Start()
    {
        pathPoints = BezierPath.GetPathPoints(BezierPath.MarkerNumber).ToArray();
        StartCoroutine(Co_Move());
    }

    IEnumerator Co_Move()
    {
        transform.position = pathPoints[0];
        for (int i = 0; i < pathPoints.Length; i++)
        {
            float duration = 1f;
            if (i < pathPoints.Length - 1)
            {
                Vector3 direction = pathPoints[i + 1] - pathPoints[i];
                duration = (direction).magnitude / Speed;
                transform.DOMove(pathPoints[i + 1], duration).SetEase(Ease.Linear);
            }
            else if (i == pathPoints.Length - 1)
            {
                Vector3 direction = pathPoints[0] - pathPoints[i];
                duration = (direction).magnitude / Speed;
                transform.DOMove(pathPoints[0], duration).SetEase(Ease.Linear);
            }

            yield return new WaitForSeconds(duration);
            if (i == pathPoints.Length - 1)
            {
                i = 0;
            }
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(LookAtTarget);
    }
}
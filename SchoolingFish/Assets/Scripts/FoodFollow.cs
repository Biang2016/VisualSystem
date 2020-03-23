using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class FoodFollow : MonoBehaviour
{
    public BezierPath BezierPath;
    public float Speed = 5f;
    private Vector3[] pathPoints;

    void Start()
    {
        pathPoints = BezierPath.GetPathPoints(BezierPath.MarkerNumber).ToArray();
        StartCoroutine(Co_Move());
    }

    IEnumerator Co_Move()
    {
        float finalSpeed = Speed * SchoolingFishManager.Instance.SchoolingUpdateInterval;
        transform.position = pathPoints[0];
        for (int i = 0; i < pathPoints.Length; i++)
        {
            float duration = 1f;
            if (i < pathPoints.Length - 1)
            {
                Vector3 direction = pathPoints[i + 1] - pathPoints[i];
                duration = (direction).magnitude / finalSpeed;
                transform.DOMove(pathPoints[i + 1], duration).SetEase(Ease.Linear);
                transform.DOLookAt(pathPoints[i + 1], 2f);
            }
            else if (i == pathPoints.Length - 1)
            {
                Vector3 direction = pathPoints[0] - pathPoints[i];
                duration = (direction).magnitude / finalSpeed;
                transform.DOMove(pathPoints[0], duration).SetEase(Ease.Linear);
                transform.DOLookAt(pathPoints[0],2f);
                i = 0;
            }

            yield return new WaitForSeconds(duration);
        }
    }
}
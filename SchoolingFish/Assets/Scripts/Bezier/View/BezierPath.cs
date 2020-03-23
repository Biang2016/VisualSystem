using System.Collections.Generic;
using UnityEngine;

public class BezierPath : MonoBehaviour
{
    public float DefaultCurveLength = 10f;

    public float CurveWidth = 4f;
    public bool ShowHandles = true;
    public bool ShowWhiteLines = true;

    public bool ShowMarkers = true;
    public int MarkerNumber = 5;
    public float MarkerSize = 0.3f;

    public List<BezierCurve> curveList = new List<BezierCurve>();

    public bool LoopClosed
    {
        get
        {
            if (curveList.Count == 0) return false;
            if (curveList[0].BezierInfo.LastBezier) return true;
            return false;
        }
    }

    void Awake()
    {
        foreach (BezierCurve bc in GetComponents<BezierCurve>())
        {
            curveList.Add(bc);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (ShowMarkers)
        {
            foreach (BezierCurve bc in curveList)
            {
                for (int i = 0; i <= MarkerNumber; i++)
                {
                    float t = (float) i / MarkerNumber;
                    Vector3 pointOnCurve = CalculateBezier(bc.BezierInfo, t);
                    if (i == 0 || i == MarkerNumber)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawSphere(pointOnCurve, MarkerSize * 2);
                    }
                    else
                    {
                        Gizmos.color = Color.white;
                        Gizmos.DrawSphere(pointOnCurve, MarkerSize);
                    }
                }
            }
        }
    }

    public List<Vector3> GetPathPoints(int markerCountPerCurve)
    {
        List<Vector3> res = new List<Vector3>();
        foreach (BezierCurve bc in curveList)
        {
            for (int i = 0; i < markerCountPerCurve; i++)
            {
                float t = (float) i / markerCountPerCurve;
                Vector3 pointOnCurve = CalculateBezier(bc.BezierInfo, t);
                res.Add(pointOnCurve);
            }
        }

        return res;
    }

    private Vector3 CalculateBezier(Bezier curveData, float t)
    {
        Vector3 a = curveData.StartPoint;
        Vector3 b = curveData.StartTangent + curveData.StartPoint;
        Vector3 c = curveData.EndTangent + curveData.EndPoint;
        Vector3 d = curveData.EndPoint;

        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);
        Vector3 cd = Vector3.Lerp(c, d, t);

        Vector3 abc = Vector3.Lerp(ab, bc, t);
        Vector3 bcd = Vector3.Lerp(bc, cd, t);

        Vector3 final = Vector3.Lerp(abc, bcd, t);

        return final;
    }
}
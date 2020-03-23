using System;
using UnityEngine;

[Serializable]
public class Bezier
{
    public BezierCurve LastBezier;
    public BezierCurve NextBezier;
    [SerializeField] protected Vector3 startPoint;
    [SerializeField] protected Vector3 endPoint;
    [SerializeField] protected Vector3 startTangent;
    [SerializeField] protected Vector3 endTangent;

    public Bezier(BezierCurve lastBezier, BezierCurve nextBezier, Vector3 startPoint, Vector3 endPoint, Vector3 startTangent, Vector3 endTangent)
    {
        LastBezier = lastBezier;
        NextBezier = nextBezier;
        if (lastBezier)
        {
            this.startPoint = lastBezier.BezierInfo.endPoint;
            this.startTangent = lastBezier.BezierInfo.EndTangent * -1;
        }
        else
        {
            StartPoint = startPoint;
            StartTangent = startTangent;
        }

        if (nextBezier)
        {
            this.endPoint = nextBezier.BezierInfo.StartPoint;
            this.endTangent = nextBezier.BezierInfo.StartTangent;
        }
        else
        {
            EndPoint = endPoint;
            EndTangent = endTangent;
        }
    }

    public Vector3 StartPoint
    {
        get
        {
            if (LastBezier)
            {
                startPoint = LastBezier.BezierInfo.EndPoint;
            }

            return startPoint;
        }
        set
        {
            if (LastBezier)
            {
                LastBezier.BezierInfo.endPoint = value;
            }

            startPoint = value;
        }
    }

    public Vector3 EndPoint
    {
        get { return endPoint; }
        set
        {
            if (NextBezier)
            {
                NextBezier.BezierInfo.startPoint = value;
            }

            endPoint = value;
        }
    }

    public Vector3 StartTangent
    {
        get { return startTangent; }
        set
        {
            if (LastBezier)
            {
                LastBezier.BezierInfo.endTangent = -LastBezier.BezierInfo.EndTangent.magnitude * value.normalized;
            }

            startTangent = value;
        }
    }

    public Vector3 EndTangent
    {
        get { return endTangent; }
        set
        {
            if (NextBezier)
            {
                NextBezier.BezierInfo.startTangent = -NextBezier.BezierInfo.StartTangent.magnitude * value.normalized;
            }

            endTangent = value;
        }
    }
}
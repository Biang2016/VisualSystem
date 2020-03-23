using UnityEngine;

[ExecuteInEditMode]
public class BezierCurve : MonoBehaviour
{
    public BezierPath BezierPath;
    public Bezier BezierInfo;
    public Color color;

    void OnEnable()
    {
        color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
    }
}
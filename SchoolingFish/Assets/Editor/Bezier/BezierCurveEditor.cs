using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveEditor : Editor
{
    public void OnSceneGUI()
    {
        BezierCurve bc = target as BezierCurve;

        if (bc.BezierPath.ShowHandles)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 startPoint = Handles.PositionHandle(bc.BezierInfo.StartPoint, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RegisterCompleteObjectUndo(target, "Move Bezier Point");
                if (bc.BezierInfo.LastBezier)
                {
                    Undo.RegisterCompleteObjectUndo(bc.BezierInfo.LastBezier, "Move Bezier Point");
                }

                Undo.FlushUndoRecordObjects();
                bc.BezierInfo.StartPoint = startPoint;
                // EditorUtility.SetDirty(bc.gameObject);
            }

            EditorGUI.BeginChangeCheck();
            Vector3 endPoint = Handles.PositionHandle(bc.BezierInfo.EndPoint, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RegisterCompleteObjectUndo(target, "Move Bezier Point");
                Undo.FlushUndoRecordObjects();
                bc.BezierInfo.EndPoint = endPoint;
                // EditorUtility.SetDirty(bc.gameObject);
            }

            EditorGUI.BeginChangeCheck();
            Vector3 startTangent = Handles.PositionHandle(bc.BezierInfo.StartTangent + bc.BezierInfo.StartPoint, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RegisterCompleteObjectUndo(target, "Change Bezier Tangent");
                if (bc.BezierInfo.LastBezier)
                {
                    Undo.RegisterCompleteObjectUndo(bc.BezierInfo.LastBezier, "Change Bezier Tangent");
                }

                Undo.FlushUndoRecordObjects();
                bc.BezierInfo.StartTangent = startTangent - bc.BezierInfo.StartPoint;
                // EditorUtility.SetDirty(bc.gameObject);
            }

            EditorGUI.BeginChangeCheck();
            Vector3 endTangent = Handles.PositionHandle(bc.BezierInfo.EndTangent + bc.BezierInfo.EndPoint, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RegisterCompleteObjectUndo(target, "Change Bezier Tangent");
                Undo.FlushUndoRecordObjects();
                bc.BezierInfo.EndTangent = endTangent - bc.BezierInfo.EndPoint;
                // EditorUtility.SetDirty(bc.gameObject);
            }
        }

        Handles.DrawBezier(bc.BezierInfo.StartPoint, bc.BezierInfo.EndPoint, bc.BezierInfo.StartPoint + bc.BezierInfo.StartTangent, bc.BezierInfo.EndPoint + bc.BezierInfo.EndTangent, bc.color, null, bc.BezierPath.CurveWidth);

        if (bc.BezierPath.ShowWhiteLines)
        {
            Handles.DrawLines(
                new[]
                {
                    bc.BezierInfo.StartPoint,
                    bc.BezierInfo.StartPoint + bc.BezierInfo.StartTangent,
                    bc.BezierInfo.StartPoint + bc.BezierInfo.StartTangent,
                    bc.BezierInfo.EndPoint + bc.BezierInfo.EndTangent,
                    bc.BezierInfo.EndPoint + bc.BezierInfo.EndTangent,
                    bc.BezierInfo.EndPoint,
                }
            );
        }
    }
}
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierPath))]
public class BezierPathEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BezierPath path = (BezierPath) target;

        if (GUILayout.Button("Clear path"))
        {
            foreach (BezierCurve bc in path.curveList)
            {
                DestroyImmediate(bc);
            }

            path.curveList.Clear();

            BezierCurve[] bcs = path.GetComponentsInChildren<BezierCurve>();
            foreach (BezierCurve bc in bcs)
            {
                DestroyImmediate(bc);
            }

            EditorUtility.SetDirty(path);
        }

        DrawDefaultInspector();

        if (GUILayout.Button("Undo"))
        {
            Undo.PerformUndo();
        }

        if (GUILayout.Button("Redo"))
        {
            Undo.PerformRedo();
        }

        if (!path.LoopClosed)
        {
            if (GUILayout.Button("Create next curve"))
            {
                BezierCurve newBC = AddNewBezierCurve();
                if (path.curveList.Count == 0)
                {
                    newBC.BezierInfo = new Bezier(null, null, Vector3.zero, Vector3.forward * path.DefaultCurveLength, Vector3.forward * path.DefaultCurveLength / 3, -Vector3.forward * path.DefaultCurveLength / 3);
                }
                else if (path.curveList.Count > 0)
                {
                    BezierCurve lastBE = path.curveList[path.curveList.Count - 1];
                    newBC.BezierInfo = new Bezier(lastBE, null, Vector3.zero, lastBE.BezierInfo.EndPoint + Vector3.forward * path.DefaultCurveLength, Vector3.one, -Vector3.forward * path.DefaultCurveLength / 3);
                    lastBE.BezierInfo.NextBezier = newBC;
                }

                path.curveList.Add(newBC);
                EditorUtility.SetDirty(path);
            }

            if (GUILayout.Button("Enclose curve"))
            {
                BezierCurve newBC = AddNewBezierCurve();
                if (path.curveList.Count == 0)
                {
                    Debug.Log("Path should have at least 1 curve!");
                }
                else
                {
                    BezierCurve nextBE = path.curveList[0];
                    BezierCurve lastBE = path.curveList[path.curveList.Count - 1];
                    newBC.BezierInfo = new Bezier(lastBE, nextBE, Vector3.one, nextBE.BezierInfo.StartPoint, Vector3.one, -nextBE.BezierInfo.StartTangent);

                    nextBE.BezierInfo.LastBezier = newBC;
                    lastBE.BezierInfo.NextBezier = newBC;

                    path.curveList.Add(newBC);
                    EditorUtility.SetDirty(path);
                }
            }
        }
    }

    private void OnEnable()
    {
        BezierPath path = (BezierPath) target;
        path.curveList.RemoveAll(item => item == null);
        //
        // for (int i = 1; i < path.curveList.Count; i++)
        // {
        //     path.curveList[i].BezierInfo.LastBezier = path.curveList[i - 1];
        // }
    }

    private BezierCurve AddNewBezierCurve()
    {
        BezierPath path = (BezierPath) target;
        BezierCurve bc = Undo.AddComponent<BezierCurve>(path.gameObject);
        bc.BezierPath = path;
        return bc;
    }
}
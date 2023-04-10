using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI(){
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.forward, Vector3.up, 360, fov.viewRadius);
        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - fov.transform.position;
        float globalAngle = Utilities.GetGlobalRotation(cursorPosition).eulerAngles.z;
        float startAngle = globalAngle - fov.viewAngle / 2;
        float endAngle = globalAngle + fov.viewAngle / 2;
        Vector2 viewAngleStart = fov.DirFromAngle(startAngle - 90, true);
        Vector2 viewAngleEnd = fov.DirFromAngle(endAngle - 90, true);

        Debug.DrawRay(fov.transform.position, viewAngleStart * fov.viewRadius);
        Debug.DrawRay(fov.transform.position, viewAngleEnd * fov.viewRadius);
        Handles.color = Color.red;
        foreach(Transform visibleTarget in fov.visibleTargets){
            Handles.DrawLine(fov.transform.position, visibleTarget.position);
        }

    }
}

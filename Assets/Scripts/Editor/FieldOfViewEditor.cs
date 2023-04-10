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
        Vector2 viewAngleStart = fov.DirFromAngle(-fov.viewAngle/2);
        Vector2 viewAngleEnd = fov.DirFromAngle(fov.viewAngle/2);

        Debug.DrawRay(fov.transform.position, viewAngleStart * fov.viewRadius);
        Debug.DrawRay(fov.transform.position, viewAngleEnd * fov.viewRadius);
        Handles.color = Color.red;
        foreach(Transform visibleTarget in fov.visibleTargets){
            Debug.Log("RUNNING");
            Handles.DrawLine(fov.transform.position, visibleTarget.position);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI(){
        FieldOfView fov = (FieldOfView)this.target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.forward, Vector3.up, 360, fov.view_radius);
        Vector2 cursor_position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - fov.transform.position;
        float global_angle = Utilities.GetGlobalRotation(cursor_position).eulerAngles.z;
        float start_angle = global_angle - fov.view_angle / 2;
        float end_angle = global_angle + fov.view_angle / 2;
        Vector2 view_angle_start = fov.DirFromAngle(start_angle - 90, true);
        Vector2 view_angle_end = fov.DirFromAngle(end_angle - 90, true);

        Debug.DrawRay(fov.transform.position, view_angle_start * fov.view_radius);
        Debug.DrawRay(fov.transform.position, view_angle_end * fov.view_radius);
        Handles.color = Color.red;
        foreach(Transform visibleTarget in fov.visibleTargets){
            Handles.DrawLine(fov.transform.position, visibleTarget.position);
        }
    }
}

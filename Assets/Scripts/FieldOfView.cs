using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {
    public float view_radius = 10;
    [Range(0, 360)]
    public float view_angle = 90;
    public LayerMask targetMask;
    public LayerMask wallMask;
    public List<Transform> visibleTargets = new List<Transform>();

    protected void Start() {
        StartCoroutine(this._FindTargetsWithDelay(.1f));
    }
    private IEnumerator _FindTargetsWithDelay(float delay) {
        while(true) {
            yield return new WaitForSeconds(delay);
            this._FindVisibleTargets();
        }
    }
    private void _FindVisibleTargets() {
        this.visibleTargets.Clear();
        Vector3 cursor_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(this.transform.position, this.view_radius, this.targetMask);
        foreach(Collider2D targetCollider in targetsInViewRadius) {
            Transform target = targetCollider.transform;
            Vector2 dir_to_target = (target.position - this.transform.position).normalized;

            // TODO: Generalize gaze direction to be on mouse cursor
            float smallest_angle = Vector2.Angle(cursor_position - this.transform.position, dir_to_target);
            if(smallest_angle < this.view_angle / 2) {
                float dist_to_target = Vector2.Distance(this.transform.position, target.position);
                RaycastHit2D hit = Physics2D.Raycast(this.transform.position, dir_to_target, dist_to_target, this.wallMask);
                if(hit.collider == null) {
                    this.visibleTargets.Add(target);
                }
            }
        }
    }


    public Vector2 DirFromAngle(float angle_degrees, bool angle_is_global = false) {
        if(!angle_is_global) {
            angle_degrees += this.transform.eulerAngles.z;
        }
        // Note we swap Cos <--> Sin since unity angles are compass angles,
        // whereas we want to use the usual math standard angles. The
        // relationship is as follows: standard_ang = 90 - unity_ang.
        // Equivalently, we can interchange Cos <--> Sin
        return new Vector2(Mathf.Cos(angle_degrees * Mathf.Deg2Rad), Mathf.Sin(angle_degrees * Mathf.Deg2Rad));
    }
}

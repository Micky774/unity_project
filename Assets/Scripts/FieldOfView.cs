using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controllable player vision cone
/// </summary>
public class FieldOfView : MonoBehaviour {
    /// <summary>
    /// Max view distance from observer
    /// </summary>
    public float view_radius = 10;
    /// <summary>
    /// Vision cone angle
    /// </summary>
    [Range(0, 360)]
    public float view_angle = 90;
    /// <summary>
    /// Layer of targets to be detected
    /// </summary>
    public LayerMask targetMask;
    /// <summary>
    /// Layer of walls that block vision
    /// </summary>
    public LayerMask wallMask;
    /// <summary>
    /// List of target positions
    /// </summary>
    public List<Transform> visibleTargets = new List<Transform>();

    /// <summary>
    /// Begins _FindTargetsWithDelay coroutine
    /// </summary>
    protected void Start() {
        StartCoroutine(this._FindTargetsWithDelay(.1f));
    }

    /// <summary>
    /// Loop checks for visible targets within vision cone periodically
    /// </summary>
    /// <param name="delay"> Time to wait between checks for targets </param>
    /// <returns> Coroutine reference </returns>
    private IEnumerator _FindTargetsWithDelay(float delay) {
        while(true) {
            yield return new WaitForSeconds(delay);
            this._FindVisibleTargets();
        }
    }

    /// <summary>
    /// Checks for visible targets within vision cone
    /// </summary>
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

    // TODO: Document DirFromAngle
    /// <summary>
    /// Scary angle conversion formula
    /// </summary>
    /// <param name="angle_degrees"></param>
    /// <param name="angle_is_global"></param>
    /// <returns></returns>
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius = 10;
    [Range(0, 360)]
    public float viewAngle = 90;
    public LayerMask targetMask;
    public LayerMask wallMask;
    public List<Transform> visibleTargets = new List<Transform>();

    void Start(){
        StartCoroutine("FindTargetsWithDelay", 1f);
    }
    private IEnumerator FindTargetsWithDelay(float delay){
        while(true){
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }
    void FindVisibleTargets(){
        visibleTargets.Clear();
        Debug.DrawRay(transform.position, Vector2.up * (viewRadius + 5));
        Debug.DrawRay(transform.position, Vector2.down * (viewRadius + 5));
        Debug.DrawRay(transform.position, Vector2.left * (viewRadius + 5));
        Debug.DrawRay(transform.position, Vector2.right * (viewRadius + 5));
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius);
        Debug.Log($"Finding targets in layermask {targetMask.ToString()}; {targetsInViewRadius.Length} targets within radius");

        foreach(Collider targetCollider in targetsInViewRadius){
            Transform target = targetCollider.transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;
            Debug.DrawLine(transform.position, target.position);
            float smallestAngle = Vector2.Angle((Vector2)transform.position, dirToTarget);
            if(smallestAngle < viewAngle/2){
                float distToTarget = Vector2.Distance(transform.position, target.position);
                Debug.DrawRay(transform.position, dirToTarget * distToTarget);
                if(!Physics.Raycast(transform.position, dirToTarget, distToTarget, wallMask)){
                    visibleTargets.Add(target);
                }
            }
        }
    }
    

    public Vector2 DirFromAngle(float angleDegrees, bool angleIsGlobal = false){
        if(!angleIsGlobal){
            angleDegrees += transform.eulerAngles.z;
        }
        // Note we swap Cos <--> Sin since unity angles are compass angles,
        // whereas we want to use the usual math standard angles. The
        // relationship is as follows: standard_ang = 90 - unity_ang.
        // Equivalently, we can interchange Cos <--> Sin
        return new Vector2(Mathf.Cos(angleDegrees*Mathf.Deg2Rad), Mathf.Sin(angleDegrees*Mathf.Deg2Rad));
    }
}

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
    public float meshPerDegree = .05f;
    private Robo _robo;
    private Vector3 _cursor;
    private float _orientation;

    public float GetOrientation(){
        return _orientation;
    }
    void Start(){
        StartCoroutine("FindTargetsWithDelay", .1f);
        _robo = GetComponentInParent<Robo>();

    }
    private IEnumerator FindTargetsWithDelay(float delay){
        while(true){
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }
    private void Update(){
        _cursor = _robo.GetCursor();
        _orientation = _robo.GetOrientation();
        DrawFieldOfView();
    }
    void FindVisibleTargets(){
        visibleTargets.Clear();

        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);
        foreach(Collider2D targetCollider in targetsInViewRadius){
            Transform target = targetCollider.transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;

            // TODO: Generalize gaze direction to be on mouse cursor
            float smallestAngle = Vector2.Angle(_cursor - transform.position, dirToTarget);
            if(smallestAngle < viewAngle / 2){
                float distToTarget = Vector2.Distance(transform.position, target.position);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToTarget, distToTarget, wallMask);
                if(hit.collider == null){
                    visibleTargets.Add(target);
                }
            }
        }
    }
    void DrawFieldOfView(){
        int stepCount = Mathf.RoundToInt(viewAngle * meshPerDegree);
        float stepAngleSize = viewAngle / stepCount;
        for(int i = 0; i <= stepCount; i++){
            float angle = Utilities.GetGlobalRotationAngle(_cursor - transform.position) - viewAngle / 2 + stepAngleSize * i;
            Debug.DrawRay(transform.position, Utilities.DirFromAngle(angle) * viewRadius, Color.black);
        }
    }
    void ViewCast(float globalAngle) {
        return;
    }
    public struct ViewCastInfo {
        public bool hit;
        public Vector3 point;
        public float dist;
        public float angle;
        public ViewCastInfo(bool hit, Vector3 point, float dist, float angle){
            this.hit = hit;
            this.point = point;
            this.dist = dist;
            this.angle = angle;
        }
    }
}

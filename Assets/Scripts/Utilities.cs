using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
    public static Quaternion GetGlobalRotationQuaternion(Vector2 displacement){
        float angle = (Vector2.SignedAngle(Vector2.up, displacement) + 360) % 360;
        return Quaternion.Euler(new Vector3(0, 0, angle));       
    }
    public static float GetGlobalRotationAngle(Vector2 displacement){
        return (Vector2.SignedAngle(Vector2.up, displacement) + 360) % 360;
    }
    public static Vector2 DirFromAngle(float angleDegrees){
        // Note we enforce the use of "rotation" angles,
        // i.e. N = 0, W = 90, S = 180, E = 270.
        return new Vector2(Mathf.Cos((angleDegrees + 90)*Mathf.Deg2Rad), Mathf.Sin((angleDegrees + 90)*Mathf.Deg2Rad));
    }
}

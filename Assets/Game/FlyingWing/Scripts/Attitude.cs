// https://answers.unity.com/questions/1366142/get-pitch-and-roll-values-from-object.html

using UnityEngine;

public class Attitude : MonoBehaviour
{
    [SerializeField]
    Transform targetTransform = null;


    public float Roll => roll;

    public float Pitch => pitch;

    public void UpdateState()
    {
        var worldUp = Vector3.up;
        var transformUp = targetTransform.up;
        var transformForward = targetTransform.forward;
        var transformRight = targetTransform.right;

        roll = targetTransform.eulerAngles.z;
        
        var dot = Vector3.Dot( worldUp, transformUp );
        if( dot < 0f )
        {
            dot = -1f;
        }
        else if( dot > 0f )
        {
            dot = 1f;
        }
        transformRight.y = 0f;
        transformRight *= Mathf.Sign( transformUp.y );
        var forward = Vector3.Cross( transformRight.normalized, worldUp ).normalized;
        pitch = Vector3.Angle( forward, transformForward ) * Mathf.Sign( transformForward.y ) * dot;
    }
    

    float roll;
    float pitch;
}
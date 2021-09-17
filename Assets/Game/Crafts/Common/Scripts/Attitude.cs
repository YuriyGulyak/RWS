// https://answers.unity.com/questions/1366142/get-pitch-and-roll-values-from-object.html

using UnityEngine;

namespace RWS
{
    public class Attitude : MonoBehaviour
    {
        public Transform targetTransform;


        public float RollDeg => rollDeg;
        public float PitchDeg => pitchDeg;
        

        float rollDeg;
        float pitchDeg;
        
        
        void OnValidate()
        {
            if( !targetTransform )
            {
                targetTransform = GetComponent<Transform>();
            }
        }


        public void UpdateStaet()
        {
            var worldUp = Vector3.up;
            var transformUp = targetTransform.up;
            var transformForward = targetTransform.forward;
            var transformRight = targetTransform.right;

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
            
            rollDeg = targetTransform.eulerAngles.z;
            pitchDeg = Vector3.Angle( forward, transformForward ) * Mathf.Sign( transformForward.y ) * dot;
        }

        public void Reset()
        {
            rollDeg = 0f;
            pitchDeg = 0f;
        }
    }
}
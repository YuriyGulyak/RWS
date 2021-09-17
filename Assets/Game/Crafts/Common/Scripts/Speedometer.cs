using UnityEngine;

namespace RWS
{
    public class Speedometer : MonoBehaviour
    {
        public Rigidbody targetRigidbody;


        public float SpeedMs => speedMs;
        public float TopSpeedMs => topSpeedMs;
        public float ForwardSpeedMs => forwardSpeedMs;


        Transform rigidbodyTransform;
        float speedMs;
        float topSpeedMs;
        float forwardSpeedMs;


        void OnValidate()
        {
            if( !targetRigidbody )
            {
                targetRigidbody = GetComponent<Rigidbody>();
            }
        }

        void Awake()
        {
            rigidbodyTransform = targetRigidbody.transform;
        }


        public void UpdateStaet()
        {
            var velocityLocal = rigidbodyTransform.InverseTransformDirection( targetRigidbody.velocity );
            
            speedMs = velocityLocal.magnitude;
            topSpeedMs = Mathf.Max( topSpeedMs, speedMs );
            forwardSpeedMs = velocityLocal.z;
        }

        public void Reset()
        {
            speedMs = 0f;
            topSpeedMs = 0f;
            forwardSpeedMs = 0f;
        }
    }
}
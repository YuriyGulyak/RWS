using UnityEngine;

namespace RWS
{
    public class OrbitCamera : MonoBehaviour
    {
        [SerializeField]
        Transform pivotTransform = null;

        [SerializeField]
        Camera targetCamera = null;

        [SerializeField]
        Vector2 rotationSpeed = new Vector2( 1f, 3f );

        [SerializeField]
        Vector2 rotationSpeedSmooth = new Vector2( 2f, 2f );

        [SerializeField]
        Vector2 xAngleMinMax = new Vector2( -10f, 35f );

        [SerializeField]
        float fovSpeed = 1f;

        [SerializeField]
        float fovSmoothTime = 1f;

        [SerializeField]
        Vector2 fovMinMax = new Vector2( 20f, 60f );

        //--------------------------------------------------------------------------------------------------------------

        Vector2 smoothedSpeed;
        float xDirection;
        float xVelocity;
        float yVelocity;

        float fovDirection;
        float smoothedFovSpeed;
        float fovVelocity;


        void Start()
        {
            xDirection = 1f;
            fovDirection = 1f;
        }

        void LateUpdate()
        {
            smoothedSpeed.x = Mathf.SmoothDamp( smoothedSpeed.x, rotationSpeed.x * xDirection, ref xVelocity,
                rotationSpeedSmooth.x );
            smoothedSpeed.y =
                Mathf.SmoothDamp( smoothedSpeed.y, rotationSpeed.y, ref yVelocity, rotationSpeedSmooth.y );

            var angles = new Vector2
            {
                x = Wrap180( pivotTransform.localEulerAngles.x ),
                y = Wrap180( pivotTransform.localEulerAngles.y )
            };

            if( xDirection > 0f && angles.x > xAngleMinMax.y )
            {
                xDirection = -1f;
            }

            if( xDirection < 0f && angles.x < xAngleMinMax.x )
            {
                xDirection = 1f;
            }

            var deltaTime = Time.deltaTime;

            angles.x += smoothedSpeed.x * deltaTime;
            angles.y += smoothedSpeed.y * deltaTime;
            angles.y = Wrap360( angles.y );

            pivotTransform.localRotation = Quaternion.Euler( angles );


            var cameraFov = targetCamera.fieldOfView;

            if( fovDirection > 0f && cameraFov > fovMinMax.y )
            {
                fovDirection = -1f;
            }

            if( fovDirection < 0f && cameraFov < fovMinMax.x )
            {
                fovDirection = 1f;
            }

            smoothedFovSpeed =
                Mathf.SmoothDamp( smoothedFovSpeed, fovSpeed * fovDirection, ref fovVelocity, fovSmoothTime );
            targetCamera.fieldOfView += smoothedFovSpeed * deltaTime;
        }


        static float Wrap360( float angle )
        {
            return ( ( angle % 360f ) + 360f ) % 360f;
        }

        static float Wrap180( float angle )
        {
            if( angle > 180f )
            {
                angle = angle - 360f;
            }

            if( angle < -180f )
            {
                angle = 360f - Mathf.Abs( angle );
            }

            return angle;
        }
    }
}
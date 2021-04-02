using UnityEngine;

namespace RWS
{
    public class PilotLook : MonoBehaviour
    {
        [SerializeField]
        Transform target = null;

        [SerializeField]
        Transform pilotBase = null;

        [SerializeField]
        Transform pilotEyes = null;

        [SerializeField]
        float smoothTime = 0.1f;

        [SerializeField]
        float trackingBias = 0f;

        [SerializeField]
        Vector3 targetOffset = Vector3.zero;

        //--------------------------------------------------------------------------------------------------------------

        public void SetTarget( Transform target )
        {
            this.target = target;

            var baseAngles = pilotBase.eulerAngles;
            baseAngles.y = CalcBaseAngle();
            pilotBase.eulerAngles = baseAngles;

            var eyesAngles = pilotEyes.eulerAngles;
            eyesAngles.x = CalcEyesAngle();
            pilotEyes.eulerAngles = eyesAngles;

            horizontalAngleVelocity = 0f;
            verticalAngleVelocity = 0f;
        }

        public void LookAtTarget()
        {
            SetTarget( target );
        }

        //--------------------------------------------------------------------------------------------------------------

        float horizontalAngleVelocity;
        float verticalAngleVelocity;


        void OnEnable()
        {
            if( !target )
            {
                return;
            }

            var baseAngles = pilotBase.eulerAngles;
            baseAngles.y = CalcBaseAngle();
            pilotBase.eulerAngles = baseAngles;

            var eyesAngles = pilotEyes.eulerAngles;
            eyesAngles.x = CalcEyesAngle();
            pilotEyes.eulerAngles = eyesAngles;
        }

        void FixedUpdate()
        {
            if( !target )
            {
                return;
            }


            // Update base (horizontal) angle

            var baseAngles = pilotBase.eulerAngles;
            baseAngles.y =
                Mathf.SmoothDampAngle( baseAngles.y, CalcBaseAngle(), ref horizontalAngleVelocity, smoothTime );
            pilotBase.eulerAngles = baseAngles;


            // Update eyes (vertical) angle

            var eyesAngles = pilotEyes.eulerAngles;
            eyesAngles.x = Mathf.SmoothDampAngle( MathUtils.WrapAngle180( eyesAngles.x ), CalcEyesAngle(),
                ref verticalAngleVelocity, smoothTime );
            pilotEyes.eulerAngles = eyesAngles;
        }


        float CalcBaseAngle()
        {
            var targetPos = target.position + ( target.right * targetOffset.x ) + ( target.up * targetOffset.y ) +
                            ( target.forward * targetOffset.z );
            var dirToTarget = ( targetPos - pilotBase.position ).normalized;
            return Mathf.Atan2( dirToTarget.x, dirToTarget.z ) * Mathf.Rad2Deg;
        }

        float CalcEyesAngle()
        {
            var targetPos = target.position + ( target.right * targetOffset.x ) + ( target.up * targetOffset.y ) +
                            ( target.forward * targetOffset.z );
            var dirToTarget = ( targetPos + Vector3.up * trackingBias ) - pilotEyes.position;
            return Quaternion.LookRotation( dirToTarget, Vector3.up ).eulerAngles.x;
        }
    }
}
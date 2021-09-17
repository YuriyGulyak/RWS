using UnityEngine;

namespace RWS
{
    public class ElevonLinkageKinematic : MonoBehaviour
    {
        [SerializeField]
        Transform servoArm = default;

        [SerializeField]
        Transform rod = default;
        
        [SerializeField]
        Transform rodTip = default;
        
        [SerializeField, Tooltip( "Elevon horn tip pivot" )]
        Transform pivotA = default;

        [SerializeField, Tooltip( "Servo arm tip pivot" )]
        Transform pivotB = default;

        [SerializeField, Tooltip( "Servo arm base pivot" )]
        Transform pivotC = default;

        //----------------------------------------------------------------------------------------------------

        Transform linkageParent;
        float rodLength;
        float servoArmLength;
        float elevonAngle;

        //----------------------------------------------------------------------------------------------------

        void Awake()
        {
            linkageParent = transform;
            rodLength = Vector3.Distance( pivotA.position, pivotB.position );
            servoArmLength = Vector3.Distance( pivotB.position, pivotC.position );
        }

        void Update()
        {
            // Servo arm angle
            var servoArmAngle = MathUtils.LawOfCos( rodLength, servoArmLength, Vector3.Distance( pivotA.position, pivotC.position ) ) - 90f;
            var servoArmAngles = servoArm.localEulerAngles;
            servoArmAngles.x = -servoArmAngle;
            servoArm.localEulerAngles = servoArmAngles;

            // Servo rod orientation
            rod.LookAt( pivotA, linkageParent.up );

            // Servo rod tip orientation
            rodTip.LookAt( pivotB, linkageParent.up );
        }

        void OnDrawGizmos()
        {
            if( !pivotA || !pivotB || !pivotC )
            {
                return;
            }

            var gizmosColorTemp = Gizmos.color;

            Gizmos.color = new Color( 1f, 1f, 1f, 0.5f );

            Gizmos.DrawWireSphere( pivotA.position, 0.001f );
            Gizmos.DrawWireSphere( pivotB.position, 0.001f );
            Gizmos.DrawWireSphere( pivotC.position, 0.001f );

            Gizmos.DrawLine( pivotA.position, pivotB.position );
            Gizmos.DrawLine( pivotB.position, pivotC.position );

            Gizmos.color = new Color( 1f, 1f, 0f, 0.5f );
            Gizmos.DrawLine( pivotC.position, pivotA.position );

            Gizmos.color = gizmosColorTemp;
        }
    }
}
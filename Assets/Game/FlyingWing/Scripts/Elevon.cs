using UnityEngine;

public class Elevon : MonoBehaviour
{
    [SerializeField] 
    Transform elevonTransform = null;

    [SerializeField]
    Transform servoArmTransform = null;
    
    [SerializeField]
    Transform servoRodTransform = null;
    
    [SerializeField]
    Transform servoRodTipTransform = null;
    
    [SerializeField]
    Transform pivotATransform = null;
    
    [SerializeField]
    Transform pivotBTransform = null;
    
    [SerializeField]
    Transform pivotCTransform = null;
    
    //----------------------------------------------------------------------------------------------------

    public float Angle
    {
        get => elevonAngle;
        set
        {
            elevonAngle = value;
            UpdateElevonKinematic();
        }
    }

    //----------------------------------------------------------------------------------------------------

    Transform elevonTransformParent;
    float rodLength;
    float armLength;
    float elevonAngle;
    
    //----------------------------------------------------------------------------------------------------

    void Awake()
    {
        elevonTransformParent = elevonTransform.parent;
        rodLength = Vector3.Distance( pivotATransform.position, pivotBTransform.position );
        armLength = Vector3.Distance( pivotBTransform.position, pivotCTransform.position );
    }
    
    void OnDrawGizmos()
    {
        if( !pivotATransform || !pivotBTransform || !pivotCTransform )
        {
            return;
        }

        var gizmosColorTemp = Gizmos.color;
        
        Gizmos.color = new Color( 1f, 1f, 1f, 0.5f );
        
        Gizmos.DrawWireSphere( pivotATransform.position, 0.001f );
        Gizmos.DrawWireSphere( pivotBTransform.position, 0.001f );
        Gizmos.DrawWireSphere( pivotCTransform.position, 0.001f );
        
        Gizmos.DrawLine( pivotATransform.position, pivotBTransform.position );
        Gizmos.DrawLine( pivotBTransform.position, pivotCTransform.position );
        
        Gizmos.color = new Color( 1f, 1f, 0f, 0.5f );
        Gizmos.DrawLine( pivotCTransform.position, pivotATransform.position );
        
        Gizmos.color = gizmosColorTemp;
    }
    
    //----------------------------------------------------------------------------------------------------

    void UpdateElevonKinematic()
    {
        // Elevon angle
        var elevonAngles = elevonTransform.localEulerAngles;
        elevonAngles.x = elevonAngle;
        elevonTransform.localEulerAngles = elevonAngles;

        // Servo arm angle
        var servoArmAngle = MathUtils.LawOfCos( rodLength, armLength, Vector3.Distance( pivotATransform.position, pivotCTransform.position ) ) - 90f;
        var servoArmAngles = servoArmTransform.localEulerAngles;
        servoArmAngles.x = -servoArmAngle;
        servoArmTransform.localEulerAngles = servoArmAngles;
        
        // Servo rod orientation
        servoRodTransform.LookAt( pivotATransform, elevonTransformParent.up );

        // Servo rod tip orientation
        servoRodTipTransform.LookAt( pivotBTransform, elevonTransformParent.up );
    }
}
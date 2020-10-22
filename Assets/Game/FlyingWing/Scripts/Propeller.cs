// https://www.youtube.com/watch?v=0bP2MH3LqvI
// https://www.youtube.com/watch?v=FmF0o7OC6dw

using UnityEngine;

public class Propeller : MonoBehaviour
{
    [Header( "Settings" )]
    
    [SerializeField] 
    Transform propellerTransform = null;

    [SerializeField] 
    Color gizmosColor = Color.white;

    [SerializeField]
    float diameter = 5f;

    [SerializeField] 
    float pitch = 5f;

    [SerializeField, Range( 0f, 1f )] 
    float pitchSlip = 0.15f;
    
    [SerializeField]
    Vector2 bladeScale = new Vector2( 0.01f, 0.05f );

    [SerializeField]
    int bladeCount = 2;

    [SerializeField]
    AnimationCurve cxVsAlpha = null;

    [SerializeField]
    AnimationCurve cyVsAlpha = null;
    
    
    [Header( "Debug" )]
    
    [SerializeField]
    float rpm = 0f;
    
    //----------------------------------------------------------------------------------------------------

    public float lift; // N
    public float drag; // N
    public float torque; // Nm
    public bool isBlocked;
    
    
    public void UpdateState( float upstreamSpeed, float rpm )
    {
        this.upstreamSpeed = upstreamSpeed;
        this.rpm = rpm;
        
        upstreamSpeed_Kmh = upstreamSpeed * MathUtils.Ms2Kmh;

        tangentialSpeed = ( rpm / 60f ) * MathUtils.Circumference( radiusInMeteers );
        tangentialSpeed_Kmh = tangentialSpeed * MathUtils.Ms2Kmh;
        
        forwardSpeed = ( rpm / 60f ) * pitchInMeteers - upstreamSpeed;

        angleOfAttack = Mathf.Atan2( forwardSpeed, tangentialSpeed ) * Mathf.Rad2Deg;

        liftCoef = cyVsAlpha.Evaluate( angleOfAttack );
        lift = AerodynamicsFormulas.L( tangentialSpeed, surfaceArea, liftCoef );
        liftKg = lift / 9.8f;

        dragCoef = cxVsAlpha.Evaluate( angleOfAttack );
        drag = AerodynamicsFormulas.D( tangentialSpeed, surfaceArea, dragCoef );
        dragKg = drag / 9.8f;
        
        torque = ( lift + drag ) * ( radiusInMeteers * 0.5f );
    }

    //----------------------------------------------------------------------------------------------------

    void Awake()
    {
        Init();
    }

    void OnValidate()
    {
        Init();
        UpdateState( upstreamSpeed, rpm );
    }

    void OnDrawGizmos()
    {
        var gizmosMatrixTemp = Gizmos.matrix;
        var gizmosColorTemp = Gizmos.color;


        Gizmos.matrix = propellerTransform.localToWorldMatrix;
        Gizmos.color = gizmosColor;

        var circleResolution = 60;
        var angleStep = 360f / circleResolution * -1f;


        // Propeller radius

        for( var i = 0; i < circleResolution; i++ )
        {
            var indexA = i;
            var indexB = ( i + 1 ) % circleResolution;

            var angleA = indexA * angleStep * Mathf.Deg2Rad;
            var angleB = indexB * angleStep * Mathf.Deg2Rad;
            
            var pointA = new Vector3
            {
                x = Mathf.Cos( angleA ) * radiusInMeteers,
                y = Mathf.Sin( angleA ) * radiusInMeteers,
            };
            var pointB = new Vector3
            {
                x = Mathf.Cos( angleB ) * radiusInMeteers, 
                y = Mathf.Sin( angleB ) * radiusInMeteers,
            };

            Gizmos.DrawLine( pointA, pointB );
        }


        // Propeller pitch

        for( var i = 0; i < circleResolution; i++ )
        {
            var indexA = i;
            var indexB = i + 1;

            var angleA = indexA * angleStep * Mathf.Deg2Rad;
            var angleB = indexB * angleStep * Mathf.Deg2Rad;
            
            var pointA = new Vector3
            {
                x = Mathf.Cos( angleA ) * radiusInMeteers,
                y = Mathf.Sin( angleA ) * radiusInMeteers, 
                z = pitchInMeteers * indexA / circleResolution
            };
            var pointB = new Vector3
            {
                x = Mathf.Cos( angleB ) * radiusInMeteers,
                y = Mathf.Sin( angleB ) * radiusInMeteers, 
                z = pitchInMeteers * indexB / circleResolution
            };

            Gizmos.DrawLine( pointA, pointB );
        }


        // Propeller center

        Gizmos.DrawWireSphere( Vector3.zero, 0.005f );

        
        // Propeller blades

        for( var i = 0; i < bladeCount; i++ )
        {
            Gizmos.matrix *= Matrix4x4.Rotate( Quaternion.Euler( 0f, 0f, 360f / bladeCount ) );
            //Gizmos.DrawWireCube( Vector3.up * ( radiusInMeteers - bladeScale.y / 2f ), new Vector3( bladeScale.x, bladeScale.y, 0f ) );
            Gizmos.DrawWireCube( Vector3.right * ( radiusInMeteers - bladeScale.y / 2f ), new Vector3( bladeScale.y, bladeScale.x, 0f ) );
        }

        

        Gizmos.matrix = gizmosMatrixTemp;
        Gizmos.color = gizmosColorTemp;
    }
    
    void OnTriggerEnter( Collider other )
    {
        if( other.isTrigger )
        {
            return;
        }
        isBlocked = true;
    }

    void OnTriggerExit( Collider other )
    {
        if( other.isTrigger )
        {
            return;
        }
        isBlocked = false;
    }
    
    //----------------------------------------------------------------------------------------------------

    void Init()
    {
        radiusInMeteers = diameter * MathUtils.Inch2Meter * 0.5f;
        
        effectivePitch = pitch * ( 1f - pitchSlip );
        pitchInMeteers = effectivePitch * MathUtils.Inch2Meter;
        
        surfaceArea = ( bladeScale.x * bladeScale.y ) * bladeCount;
    }
    
    float radiusInMeteers;
    float effectivePitch;
    float pitchInMeteers;
    float surfaceArea;
    float upstreamSpeed;
    float upstreamSpeed_Kmh;
    float tangentialSpeed;
    float tangentialSpeed_Kmh;
    float forwardSpeed;
    //float forwardSpeed_Kmh;
    float angleOfAttack;
    float liftCoef;
    float liftKg;
    float dragCoef;
    float dragKg;
}
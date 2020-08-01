using UnityEngine;

public class FlyingWing : MonoBehaviour
{
    [SerializeField]
    Rigidbody wingRigidbody = null;

    [SerializeField] 
    Transform wingTransform = null;

    [SerializeField]
    AirfoilSection[] airfoilSections = null;

    [SerializeField]
    float wingspan = 0f;

    [SerializeField]
    float aspectRatio = 0f;
    
    [SerializeField]
    Elevon leftElevon = null;
    
    [SerializeField]
    Elevon rightElevon = null;

    [SerializeField]
    Vector2 elevonRange = new Vector2( -20f, 20f );

    [SerializeField]
    float elevonSmoothTime = 0.02f;
    
    [SerializeField]
    float elevonMaxSpeed = 600f;
    
    [SerializeField]
    Motor motor = null;

    [SerializeField]
    Battery battery = null;

    [SerializeField]
    Altimeter altimeter = null;
    
    [SerializeField]
    Vector3 GC = Vector3.zero;
    
    [SerializeField]
    Vector3 drag = Vector3.zero;

    [SerializeField]
    Vector3 agularDrag = Vector3.zero;
    
    [SerializeField]
    float maxAngularVelocity = Mathf.Infinity;

    //----------------------------------------------------------------------------------------------------
    
    public float RollSetpoint
    {
        get => rollSetpoint;
        set => rollSetpoint = Mathf.Clamp( value, -1f, 1f );
    }
    
    public float PitchSetpoint
    {
        get => pitchSetpoint;
        set => pitchSetpoint = Mathf.Clamp( value, -1f, 1f );
    }
    
    public float Throttle
    {
        get => throttle;
        set => throttle = Mathf.Clamp( value, 0f, 1f );
    }
    
    public float Altitude => altitude;

    public float TAS => speed; // True airspeed, TAS 
    
    public float RollSpeed => rollSpeed;
    
    public float PitchSpeed => pitchSpeed;
    
    public float AngleOfAttack => angleOfAttack;

    public float SideslipAngle => sideslipAngle;
    
    //----------------------------------------------------------------------------------------------------

    float wingSurface;
    float rollSetpoint;
    float pitchSetpoint;
    float throttle;
    Vector3 velocity;
    Vector3 velocityLocal;
    float liftSum;
    float dragSum;
    float angleOfAttack;
    float sideslipAngle;
    float speed;
    float altitude;
    float rollSpeed;
    float pitchSpeed;
    float leftElevonAngleVelocity;
    float rightElevonAngleVelocity;
    Vector2 perlinOffset;
    
    
    void OnValidate()
    {
        airfoilSections = GetComponentsInChildren<AirfoilSection>( false );

        // Total surface area
        wingSurface = 0f;
        for( var i = 0; i < airfoilSections.Length; i++ )
        {
            wingSurface += airfoilSections[ i ].surfaceArea;
        }

        aspectRatio = AerodynamicsFormulas.AR( wingspan, wingSurface );
        foreach( var airfoilSection in airfoilSections )
        {
            airfoilSection.aspectRatio = aspectRatio;
        }
        
        //var CDi = AerodynamicsFormulas.CDi( 1f, aspectRatio, 0.85f );
        //var Di = AerodynamicsFormulas.L( 1.225f, 40f, wingSurface, CDi );
    }

    void Awake()
    {
        //wingRigidbody.ResetInertiaTensor();
        //wingRigidbody.ResetCenterOfMass();
        wingRigidbody.centerOfMass = GC;
        wingRigidbody.maxAngularVelocity = maxAngularVelocity * Mathf.Deg2Rad;
    }

    void FixedUpdate()
    {
        UpdateElevons();
        UpdateState();
    }

    void OnDrawGizmos()
    {
        var gizmosMatrixTemp = Gizmos.matrix;
        var gizmosColorTemp = Gizmos.color;
        
        
        Gizmos.color = Color.red;
        Gizmos.matrix = wingTransform.localToWorldMatrix;
        
        // GC point
        Gizmos.DrawWireSphere( GC, 0.0025f );
        
       // Gravity vector
       var downLocal = wingTransform.InverseTransformVector( Vector3.down );
       Gizmos.DrawRay( GC, downLocal * 0.1f );
        
        // Velocity vector
        Gizmos.DrawRay( GC, velocityLocal * ( wingRigidbody.velocity.magnitude * 0.01f ) );

        // Wingspan
        Gizmos.color = new Color( 0f, 0f, 0f, 0.25f );
        var leftWingTip = Vector3.left * ( wingspan * 0.5f );
        var rightWingTip = Vector3.right * ( wingspan * 0.5f );
        Gizmos.DrawLine( leftWingTip, rightWingTip );
        Gizmos.DrawRay( leftWingTip, Vector3.back * 0.05f );
        Gizmos.DrawRay( rightWingTip, Vector3.back * 0.05f );

        
        Gizmos.matrix = gizmosMatrixTemp;
        Gizmos.color = gizmosColorTemp;
    }


    void UpdateState()
    {
        var deltaTime = Time.fixedDeltaTime;
        
        velocity = wingRigidbody.velocity;
        velocityLocal = wingTransform.InverseTransformDirection( velocity );
        speed = velocity.magnitude;
        altitude = altimeter.Altitude;
        
        angleOfAttack = Vector3.SignedAngle( Vector3.forward, new Vector3( 0f, velocityLocal.y, velocityLocal.z ).normalized, Vector3.right );
        angleOfAttack = MathUtils.WrapAngle90( angleOfAttack );

        sideslipAngle = Vector3.SignedAngle( Vector3.forward, new Vector3( velocityLocal.x, 0f, velocityLocal.z ).normalized, Vector3.up );
        sideslipAngle = MathUtils.WrapAngle90( sideslipAngle );

        
        // Turbulence imitation
        
        var speedNorm = ( speed * 3.6f ) / 200f;

        perlinOffset.x += deltaTime * 3f * speedNorm;
        perlinOffset.y += deltaTime * 1f * speedNorm;
        
        if( perlinOffset.x > 1f )
        {
            perlinOffset.x -= 1f;
        }
        if( perlinOffset.y > 1f )
        {
            perlinOffset.y -= 1f;
        }

        var perlinNoiseValue = Mathf.PerlinNoise( perlinOffset.x, perlinOffset.y );
        var turbulenceAmplitude = new Vector2( 0.6f, 3f );

        var turbulenceRotation =
            Quaternion.AngleAxis( Mathf.Lerp( -turbulenceAmplitude.x, turbulenceAmplitude.x, perlinNoiseValue ) * speedNorm, wingTransform.right ) * 
            Quaternion.AngleAxis( Mathf.Lerp( -turbulenceAmplitude.y, turbulenceAmplitude.y, perlinNoiseValue ) * speedNorm, wingTransform.up );

        
        // . . .
        
        foreach( var section in airfoilSections )
        {
            var pointVelocity = wingRigidbody.GetPointVelocity( section.cp );
            pointVelocity = turbulenceRotation * pointVelocity;

            section.UpdateState( pointVelocity );
            wingRigidbody.AddForceAtPosition( section.liftForce + section.dragForce, section.cp, ForceMode.Force );
        }


        // . . .
        
        motor.UpdateState( velocityLocal.z, battery.Voltage, throttle );
        wingRigidbody.AddForce( wingTransform.forward * motor.thrust, ForceMode.Force );
        wingRigidbody.AddRelativeTorque( 0f, 0f, -motor.torque, ForceMode.Force );

        
        // . . .
        
        battery.UpdateState( motor.current, deltaTime );
        

        // Linear drag
        
        velocityLocal = new Vector3
        {
            x = velocityLocal.x * Mathf.Clamp01( 1f - drag.x * deltaTime ),
            y = velocityLocal.y * Mathf.Clamp01( 1f - drag.y * deltaTime ),
            z = velocityLocal.z * Mathf.Clamp01( 1f - drag.z * deltaTime )
        };
        wingRigidbody.velocity = wingTransform.TransformDirection( velocityLocal );

        
        // Angular drag
        
        var localAngularVelocity = wingTransform.InverseTransformDirection( wingRigidbody.angularVelocity );
        localAngularVelocity = Vector3.Scale( localAngularVelocity, new Vector3
        {
            x = Mathf.Clamp01( 1f - agularDrag.x * deltaTime ),
            y = Mathf.Clamp01( 1f - agularDrag.y * deltaTime ),
            z = Mathf.Clamp01( 1f - agularDrag.z * deltaTime )
        } );
        wingRigidbody.angularVelocity = wingTransform.TransformDirection( localAngularVelocity );
        
        
        // Angular speed
        
        rollSpeed = -localAngularVelocity.z * Mathf.Rad2Deg;
        pitchSpeed = -localAngularVelocity.x * Mathf.Rad2Deg;
    }
    
    void UpdateElevons()
    {
        var leftElevonValue = pitchSetpoint - rollSetpoint;
        var rightElevonValue = pitchSetpoint + rollSetpoint;
        
        var leftElevonAngleTarget = Mathf.Lerp( elevonRange.x, elevonRange.y, Mathf.InverseLerp( -1f, 1f, leftElevonValue ) );
        var rightElevonAngleTarget = Mathf.Lerp( elevonRange.x, elevonRange.y, Mathf.InverseLerp( -1f, 1f, rightElevonValue ) );
        
        leftElevon.Angle = Mathf.SmoothDampAngle( leftElevon.Angle, leftElevonAngleTarget, ref leftElevonAngleVelocity, elevonSmoothTime, elevonMaxSpeed );
        rightElevon.Angle = Mathf.SmoothDampAngle( rightElevon.Angle, rightElevonAngleTarget, ref rightElevonAngleVelocity, elevonSmoothTime, elevonMaxSpeed );
    }
}
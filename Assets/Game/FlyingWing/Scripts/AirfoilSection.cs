using UnityEngine;

public class AirfoilSection : MonoBehaviour
{
    [SerializeField] 
    Transform sectionTransform = null;

    [SerializeField] 
    Transform leadingEdgePoint = null;
    
    [SerializeField] 
    Transform trailingEdgePoint = null;

    [SerializeField] 
    Color boundsColor = Color.green;

    [SerializeField]
    Color chordColor = Color.yellow;
    
    [SerializeField]
    float forceDebugScale = 0.1f;

    [SerializeField]
    float velocityDebugScale = 0.01f;

    //----------------------------------------------------------------------------------------------------
    
    public Vector3 scale = new Vector3( 1f, 1f, 1f );
    public Vector3 center = new Vector3( 0f, 0f, 0f );
    
    public float surfaceArea;
    
    public float sweepAngle;

    public float aspectRatio;
    
    public AirfoilConfig airfoilConfig;
    
    public float liftScale = 1f;
    public float dragScale = 1f;

    [Range( 0f, 1f )]
    public float centerOfPressurePosition;

    public Vector3 cp; // Center Of Pressure
    
    public Vector3 liftForce;
    public Vector3 dragForce;
    
    
    public void UpdateState( Vector3 pointVelocity )
    {
        velocity = pointVelocity;
        //velocity = Vector3.forward * 25f;

        var velocityLocal = sectionTransform.InverseTransformVector( velocity );
        var velocityNorm = velocity.normalized;
        
        trailingEdgePointRelative = leadingEdgePoint.InverseTransformPoint( trailingEdgePoint.position );
        chordRelativeAngle = Mathf.Atan2( -trailingEdgePointRelative.y, -trailingEdgePointRelative.z ) * Mathf.Rad2Deg;
        
        aoa = Vector3.SignedAngle( Vector3.forward, new Vector3( 0f, velocityLocal.y, velocityLocal.z ).normalized, Vector3.right ) + chordRelativeAngle;
        aoa = MathUtils.WrapAngle90( aoa );

        sideslipAngle = Vector3.SignedAngle( Vector3.forward, new Vector3( velocityLocal.x, 0f, velocityLocal.z ).normalized, Vector3.up );
        sideslipAngle = MathUtils.WrapAngle90( sideslipAngle );
        sideslipCoef = Mathf.Cos( ( sideslipAngle - sweepAngle ) * Mathf.Deg2Rad );
        //sideslipCoef = 1f;
        
        centerOfPressurePosition = airfoilConfig.CpVsAlpha.Evaluate( aoa );
        cp = Vector3.Lerp( leadingEdgePoint.position, trailingEdgePoint.position, centerOfPressurePosition );
        
        var speedMs = velocity.magnitude;

        liftCoef = airfoilConfig.CyVsAlpha.Evaluate( aoa );
        lift = AerodynamicsFormulas.L( speedMs, surfaceArea, liftCoef ) * sideslipCoef * liftScale;
        liftForce = Vector3.Cross( velocityNorm, sectionTransform.right ) * lift;

        dragCoef = airfoilConfig.CxVsAlpha.Evaluate( aoa );
        drag = AerodynamicsFormulas.D( speedMs, surfaceArea, dragCoef ) * sideslipCoef * dragScale;
        dragForce = -velocityNorm * drag;
    }

    //----------------------------------------------------------------------------------------------------

    void Awake()
    {
        Init();
    }

    void OnValidate()
    {
        Init();
    }

    void OnDrawGizmos()
    {
        var gizmosMatrixTemp = Gizmos.matrix;
        var gizmosColorTemp = Gizmos.color;
        
        
        Gizmos.matrix = sectionTransform.localToWorldMatrix;

        // Center point
        Gizmos.color = Color.white;
        centerOfPressurePosition = airfoilConfig.CpVsAlpha.Evaluate( aoa );
        cp = Vector3.Lerp( leadingEdgePoint.position, trailingEdgePoint.position, centerOfPressurePosition );
        Gizmos.DrawWireSphere( sectionTransform.InverseTransformPoint( cp ), 0.002f );

        // Wing surface
        Gizmos.color = boundsColor;
        DrawWireWingSurface( scale );

        // Chord
        if( leadingEdgePoint && trailingEdgePoint )
        {
            var leadingPointLocal = sectionTransform.InverseTransformPoint( leadingEdgePoint.position );
            var trailingPointLocal = sectionTransform.InverseTransformPoint( trailingEdgePoint.position );
            
            Gizmos.color = chordColor;
            Gizmos.DrawLine( leadingPointLocal, trailingPointLocal );
            Gizmos.DrawWireSphere( leadingPointLocal, 0.002f );
            Gizmos.DrawWireSphere( trailingPointLocal, 0.002f );
        }
        
        Gizmos.matrix = gizmosMatrixTemp;
       

        // Velocity vector
        Gizmos.color = new Color( 0f, 0.5f, 1f, 0.5f );
        Gizmos.DrawRay( sectionTransform.position, velocity * velocityDebugScale );
        
        // Lift force
        Gizmos.color = new Color( 0f, 1f, 0f, 0.5f );
        Gizmos.DrawRay( sectionTransform.position, liftForce * forceDebugScale );

        // Drag force
        Gizmos.color = new Color( 1f, 0f, 0f, 0.5f );
        Gizmos.DrawRay( sectionTransform.position + liftForce * forceDebugScale, dragForce * forceDebugScale );

        // Resultant force
        Gizmos.color = new Color( 1f, 1f, 1f, 0.25f );
        Gizmos.DrawRay( sectionTransform.position, ( liftForce + dragForce ) * forceDebugScale );

        
        Gizmos.color = gizmosColorTemp;
    }
    
    //---------------------------------------------------------------------------------------------------

    Vector3 velocity;
    float aoa;
    float sideslipAngle;
    float sideslipCoef;
    Vector3 trailingEdgePointRelative;
    float chordRelativeAngle;
    float liftCoef;
    float lift;
    float dragCoef;
    float drag;

    void Init()
    {
        surfaceArea = scale.x * scale.z;
        centerOfPressurePosition = airfoilConfig.CpVsAlpha.Evaluate( aoa );
        cp = Vector3.Lerp( leadingEdgePoint.position, trailingEdgePoint.position, centerOfPressurePosition );
    }
    
    void DrawWireWingSurface( Vector3 scale )
    {
        var zOffset = Mathf.Tan( sweepAngle * Mathf.Deg2Rad ) * scale.x * 0.5f;

        var leftOffset = new Vector3( -scale.x * 0.5f, 0f, zOffset ) + center;
        var rightOffset = new Vector3( scale.x * 0.5f, 0f, -zOffset ) + center;
        
        var p1 = Vector3.zero;
        var p2 = Vector3.Scale( Vector3.back, new Vector3( 0f, scale.y, scale.z ) );

        var p1_left = p1 + leftOffset;
        var p2_left = p2 + leftOffset;
            
        var p1_right = p1 + rightOffset;
        var p2_right = p2 + rightOffset;
            
        Gizmos.DrawLine( p1_left, p2_left );
        Gizmos.DrawLine( p1_right, p2_right );
            
        Gizmos.DrawLine( p1_left, p1_right );
        Gizmos.DrawLine( p2_left, p2_right );
    }
}
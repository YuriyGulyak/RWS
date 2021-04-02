using UnityEngine;

namespace RWS
{
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

        [SerializeField] 
        Vector3 scale = new Vector3( 1f, 1f, 1f );

        [SerializeField] 
        Vector3 center = new Vector3( 0f, 0f, 0f );

        [SerializeField] 
        float sweepAngle = 0f;

        [SerializeField] 
        AirfoilConfig airfoilConfig;

        [SerializeField] 
        float liftScale = 1f;

        [SerializeField] 
        float dragScale = 1f;

        //----------------------------------------------------------------------------------------------------

        public Vector3 Position => sectionTransform.position;

        public Vector3 Scale => scale;

        public float SweepAngle => sweepAngle;

        public float SurfaceArea => surfaceArea;

        public float LiftScale
        {
            get => liftScale;
            set => liftScale = value;
        }

        public float DragScale
        {
            get => dragScale;
            set => dragScale = value;
        }


        public void UpdateState( Vector3 pointVelocity )
        {
            velocity = pointVelocity;
            //velocity = Vector3.forward * 25f;

            var velocityLocal = sectionTransform.InverseTransformVector( velocity );
            var velocityNorm = velocity.normalized;

            trailingEdgePointRelative = leadingEdgePoint.InverseTransformPoint( trailingEdgePoint.position );
            chordRelativeAngle = Mathf.Atan2( -trailingEdgePointRelative.y, -trailingEdgePointRelative.z ) *
                                 Mathf.Rad2Deg;

            aoa = Vector3.SignedAngle( Vector3.forward, new Vector3( 0f, velocityLocal.y, velocityLocal.z ).normalized,
                      Vector3.right ) + chordRelativeAngle;
            aoa = MathUtils.WrapAngle90( aoa );

            sideslipAngle = Vector3.SignedAngle( Vector3.forward,
                new Vector3( velocityLocal.x, 0f, velocityLocal.z ).normalized, Vector3.up );
            sideslipAngle = MathUtils.WrapAngle90( sideslipAngle );
            sideslipCoef = Mathf.Cos( ( sideslipAngle - sweepAngle ) * Mathf.Deg2Rad );

            centerOfPressurePosition = airfoilConfig.CpVsAlpha.Evaluate( aoa );
            cp = Vector3.Lerp( leadingEdgePoint.position, trailingEdgePoint.position, centerOfPressurePosition );

            var speedMs = velocity.magnitude;
            var liftNorm = Vector3.Cross( velocityNorm, sectionTransform.right );

            // Lift
            liftCoef = airfoilConfig.CyVsAlpha.Evaluate( aoa );
            lift = Aerodynamics.L( speedMs, surfaceArea, liftCoef ) * sideslipCoef * liftScale;
            liftForce = liftNorm * lift;

            // Reflex
            reflexCp = trailingEdgePoint.position;
            var reflex = airfoilConfig.ReflexVsAlpha.Evaluate( aoa ) *
                         ( 0.5f * Aerodynamics.p * ( speedMs * speedMs ) * surfaceArea ) * sideslipCoef * liftScale;
            reflexForce = -liftNorm * reflex;

            // Drag
            dragCoef = airfoilConfig.CxVsAlpha.Evaluate( aoa );
            drag = Aerodynamics.D( speedMs, surfaceArea, dragCoef ) * sideslipCoef * dragScale;
            dragForce = -velocityNorm * drag;
        }

        public void ApplyForces( Rigidbody rigidbody )
        {
            rigidbody.AddForceAtPosition( liftForce + dragForce, cp, ForceMode.Force );
            rigidbody.AddForceAtPosition( reflexForce, reflexCp, ForceMode.Force );
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
        Vector3 cp; // Center Of Pressure
        Vector3 liftForce;
        Vector3 dragForce;
        Vector3 reflexCp;
        Vector3 reflexForce;
        float surfaceArea;
        float centerOfPressurePosition;


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
}
using UnityEngine;

namespace RWS
{
    public class FlyingWing : MonoBehaviour
    {
        [SerializeField]
        Rigidbody wingRigidbody = default;

        [SerializeField]
        Transform wingTransform = default;

        [SerializeField]
        AirfoilSection[] airfoilSections = default;

        [SerializeField]
        CenterOfMass centerOfMass = default;

        [SerializeField]
        Elevon leftElevon = default;

        [SerializeField]
        Elevon rightElevon = default;

        [SerializeField]
        Vector2 elevonRange = new Vector2( -20f, 20f );

        [SerializeField]
        float elevonSmoothTime = 0.02f;

        [SerializeField]
        float elevonMaxSpeed = 600f;

        [SerializeField]
        Speedometer speedometer = default;
        
        [SerializeField]
        Altimeter altimeter = default;

        [SerializeField]
        Attitude attitude = default;

        [SerializeField]
        Home home = default;
        
        [SerializeField]
        Motor motor = default;

        [SerializeField]
        Battery battery = default;

        [SerializeField]
        Transceiver transceiver = default;

        [SerializeField]
        CrashDetector crashDetector = default;

        //[SerializeField]
        //CraftTelemetryData telemetryData = default;
        
        [SerializeField]
        float maxAngularVelocity = 900f;

        //----------------------------------------------------------------------------------------------------

        public Transform Transform => wingTransform;

        public Rigidbody Rigidbody => wingRigidbody;

        public Battery Battery => battery;

        public Motor Motor => motor;

        public Transceiver Transceiver => transceiver;

        public Speedometer Speedometer => speedometer;
        
        public Elevon LeftElevon => leftElevon;
        public Elevon RightElevon => rightElevon;

        public CrashDetector CrashDetector => crashDetector;
        
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

        //public float SpeedMs => speedometer.SpeedMs;

        //public float TopSpeedMs => speedometer.TopSpeedMs;

        public float Altitude => altimeter.Altitude;
        
        public float RollDeg => attitude.RollDeg;

        public float PitchDeg => attitude.PitchDeg;

        public float SignalStrength => transceiver.SignalStrength;
        
        public float RollSpeed => rollSpeed;

        public float PitchSpeed => pitchSpeed;

        public float AngleOfAttack => angleOfAttack;

        public float SideslipAngle => sideslipAngle;

        public float Flytime => flytime;

        public float Voltage => battery.Voltage;

        public float CellVoltage => battery.CellVoltage;

        public float CapacityDrawn => battery.CapacityDrawn * 1000f; // mAh

        public float CurrentDraw => currentDraw;

        public float RPM => motor.rpm;

        public float FlightDistance => flightDistance;

        public float HomeDirection => home.HomeDirection;
        public float HomeDistance => home.HomeDistance;
        
        public void Reset( Vector3 position, Quaternion rotation )
        {
            wingRigidbody.isKinematic = true;
            wingRigidbody.position = position;
            wingRigidbody.rotation = rotation;
            
            wingTransform.position = position;
            wingTransform.rotation = rotation;
            
            ResetState();
        }

        //----------------------------------------------------------------------------------------------------
        
        float rollSetpoint;
        float pitchSetpoint;
        float throttle;
        Vector3 velocityLocal;
        float liftSum;
        float dragSum;
        float angleOfAttack;
        float sideslipAngle;
        float rollSpeed;
        float pitchSpeed;
        float leftElevonAngleVelocity;
        float rightElevonAngleVelocity;
        Vector2 perlinOffset;
        float flytime;
        float currentDraw;
        float flightDistance;


        void OnValidate()
        {
            airfoilSections = GetComponentsInChildren<AirfoilSection>( false );

            if( !wingRigidbody )
            {
                wingRigidbody = GetComponent<Rigidbody>();
            }
            if( !wingTransform )
            {
                wingTransform = transform;
            }
            if( !centerOfMass )
            {
                centerOfMass = GetComponent<CenterOfMass>();
            }
            if( !speedometer )
            {
                speedometer = GetComponent<Speedometer>();
            }
            if( !altimeter )
            {
                altimeter = GetComponent<Altimeter>();
            }
            if( !attitude )
            {
                attitude = GetComponent<Attitude>();
            }
            if( !home )
            {
                home = GetComponent<Home>();
            }
        }

        void Awake()
        {
            wingRigidbody.maxAngularVelocity = maxAngularVelocity * Mathf.Deg2Rad;
        }

        void Start()
        {
            altimeter.CalcTerrainHeight();
            home.Init();
            
            ResetState();
        }

        void OnDisable()
        {
            ResetState();
        }

        void FixedUpdate()
        {
            var deltaTime = Time.fixedDeltaTime;

            speedometer.UpdateStaet();
            altimeter.UpdateStaet();
            attitude.UpdateStaet();

            velocityLocal = wingTransform.InverseTransformDirection( wingRigidbody.velocity );

            angleOfAttack = Vector3.SignedAngle( Vector3.forward, new Vector3( 0f, velocityLocal.y, velocityLocal.z ).normalized, Vector3.right );
            angleOfAttack = MathUtils.WrapAngle90( angleOfAttack );

            sideslipAngle = Vector3.SignedAngle( Vector3.forward, new Vector3( velocityLocal.x, 0f, velocityLocal.z ).normalized, Vector3.up );
            sideslipAngle = MathUtils.WrapAngle90( sideslipAngle );


            if( speedometer.SpeedMs >= 0.1f )
            {
                flytime += deltaTime;
            }

            flightDistance += speedometer.SpeedMs * deltaTime;


            // Turbulence imitation

            var speedNorm = ( speedometer.SpeedMs * MathUtils.Ms2Kmh ) / 200f;

            perlinOffset.x += deltaTime * 5f * speedNorm;
            perlinOffset.y += deltaTime * 2f * speedNorm;

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

            var turbulenceRotation = Quaternion.AngleAxis( Mathf.Lerp( -turbulenceAmplitude.x, turbulenceAmplitude.x, perlinNoiseValue ) * speedNorm, wingTransform.right ) *
                                     Quaternion.AngleAxis( Mathf.Lerp( -turbulenceAmplitude.y, turbulenceAmplitude.y, perlinNoiseValue ) * speedNorm, wingTransform.up );


            // . . .

            UpdateElevons();

            foreach( var section in airfoilSections )
            {
                var pointVelocity = wingRigidbody.GetPointVelocity( section.Position );
                pointVelocity = turbulenceRotation * pointVelocity;

                section.UpdateState( pointVelocity );
                section.ApplyForces( wingRigidbody );
            }


            // . . .

            motor.UpdateState( speedometer.ForwardSpeedMs, battery.Voltage, throttle );
            currentDraw = motor.current;


            // Add motor forces

            wingRigidbody.AddForce( wingTransform.forward * motor.thrust, ForceMode.Force );
            wingRigidbody.AddRelativeTorque( 0f, 0f, -motor.torque, ForceMode.Force );


            // . . .

            battery.UpdateState( currentDraw, deltaTime );


            // Angular speed

            var localAngularVelocity = wingTransform.InverseTransformDirection( wingRigidbody.angularVelocity );
            rollSpeed = -localAngularVelocity.z * Mathf.Rad2Deg;
            pitchSpeed = -localAngularVelocity.x * Mathf.Rad2Deg;
            
            
            // Update craft variables
            
            //telemetryData.UpdateData( this );
        }

        void OnDrawGizmos()
        {
            var gizmosMatrixTemp = Gizmos.matrix;
            var gizmosColorTemp = Gizmos.color;


            Gizmos.color = Color.red;
            Gizmos.matrix = wingTransform.localToWorldMatrix;
            

            // Gravity vector
            var downLocal = wingTransform.InverseTransformVector( Vector3.down );
            Gizmos.DrawRay( centerOfMass.value, downLocal * 0.1f );

            // Velocity vector
            Gizmos.DrawRay( centerOfMass.value, velocityLocal * ( wingRigidbody.velocity.magnitude * 0.01f ) );


            Gizmos.matrix = gizmosMatrixTemp;
            Gizmos.color = gizmosColorTemp;
        }


        void ResetState()
        {
            velocityLocal = Vector3.zero;
            flytime = 0f;
            flightDistance = 0f;
            altimeter.Reset();
            speedometer.Reset();
            attitude.Reset();
            transceiver.Reset();
            battery.Reset();
            motor.Reset();
            crashDetector.Reset();
            home.Reset();
            
            //telemetryData.UpdateData( this );
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
}
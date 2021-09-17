using Unity.Mathematics;
using UnityEngine;

namespace RWS
{
    public class Transceiver : MonoBehaviour
    {
        [SerializeField]
        PixelateImageEffect pixelateEffect = default;

        [SerializeField]
        Transform craftAntennaTransform = default;

        [SerializeField]
        Transform groundAntennaTransform = default;

        [SerializeField]
        LayerMask obstacleMask = default;

        [SerializeField]
        float power = 100f;

        [SerializeField]
        float rssiMax = 0f;

        [SerializeField]
        float rssiMin = -66f;

        [SerializeField]
        float maxObstacleWidth = 1f; // 

        [SerializeField]
        float updateRate = 30f;

        const float warningSignal = 0.2f;
        const float criticalSignal = 0.1f;

        //----------------------------------------------------------------------------------------------------

        public float SignalStrength => signalStrength; // 0...1

        public Status SignalStatus => signalStatus;

        public bool InfiniteRange
        {
            get => infiniteRange;
            set => infiniteRange = value;
        }

        public void Init( Vector3 groundAntennaPosition )
        {
            this.groundAntennaPosition = groundAntennaPosition;
        }

        public void Reset()
        {
            targetPixelateIntensity = 0f;
            smoothedRssiValue = 1f;
            signalStatus = Status.Ok;
        }

        //----------------------------------------------------------------------------------------------------

        void Awake()
        {
            signalStrength = 1f;
            targetPixelateIntensity = 0f;
            smoothedRssiValue = 1f;
            signalStatus = Status.Ok;

            customUpdate = new CustomUpdate( updateRate );
            customUpdate.OnUpdate += OnUpdate;
        }

        void Update()
        {
            customUpdate.Update( Time.time );
        }

        //----------------------------------------------------------------------------------------------------

        Vector3 groundAntennaPosition;
        float distance;
        float rssiDecibels;
        float signalStrength;
        float smoothedRssiValue;
        float obstacleWidth;
        float obstacleFactor;
        float targetPixelateIntensity;
        Status signalStatus;
        CustomUpdate customUpdate;
        bool infiniteRange;


        void OnUpdate( float deltaTime )
        {
            if( infiniteRange )
            {
                return;
            }

            var positionA = craftAntennaTransform.position + new Vector3( 0f, 1f, 0f );
            var positionB = groundAntennaTransform ? groundAntennaTransform.position : groundAntennaPosition;

            distance = math.distance( positionA, positionB );
            distance = math.max( 1f, distance );

            if( Physics.Raycast( positionA, positionB - positionA, out var hit, distance, obstacleMask ) )
            {
                obstacleWidth = distance - hit.distance;

                if( Physics.Raycast( positionB, positionA - positionB, out hit, distance, obstacleMask ) )
                {
                    obstacleWidth -= hit.distance;
                }

                obstacleFactor = math.remap( 0f, maxObstacleWidth, 1f, 0f, obstacleWidth );
                obstacleFactor = math.saturate( obstacleFactor );
            }
            else
            {
                obstacleWidth = 0f;
                obstacleFactor = 1f;
            }

            rssiDecibels = Decibels( 1f, power / ( distance * distance ) );


            signalStrength = math.remap( rssiMax, rssiMin, 1f, 0f, rssiDecibels );
            signalStrength += Mathf.Lerp( -0.05f, 0.05f, Mathf.PerlinNoise( Time.fixedTime * 0.5f, Time.fixedTime ) );
            signalStrength = math.saturate( signalStrength );
            signalStrength *= obstacleFactor;


            targetPixelateIntensity = math.remap( 0.2f, 0f, 0f, 1f, signalStrength );

            var smoothedPixelateIntensity = Mathf.Lerp( pixelateEffect.Intensity, targetPixelateIntensity, math.saturate( deltaTime * 4f ) );
            if( smoothedPixelateIntensity > 0.99f )
            {
                smoothedPixelateIntensity = 1f;
            }

            pixelateEffect.Intensity = smoothedPixelateIntensity;


            smoothedRssiValue = Mathf.Lerp( smoothedRssiValue, signalStrength, deltaTime * 5f );
            if( smoothedRssiValue > warningSignal )
            {
                signalStatus = Status.Ok;
            }
            else if( smoothedRssiValue > criticalSignal )
            {
                signalStatus = Status.Warning;
            }
            else
            {
                signalStatus = Status.Critical;
            }
        }

        static float Decibels( float p1, float p2 )
        {
            return 10f * Mathf.Log10( p2 / p1 );
        }


        /*
        // Free space path loss
        static float FSPL( float d, float f )
        {
            // Speed of light
            var c = 299792458f;
            
            return Mathf.Pow( ( 4f * Mathf.PI * d * f ) / c, 2f );
        }
        */
    }
}
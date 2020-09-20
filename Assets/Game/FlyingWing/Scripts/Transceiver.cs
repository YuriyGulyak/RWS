using Unity.Mathematics;
using UnityEngine;

public class Transceiver : MonoBehaviour
{
    [SerializeField]
    PixelateImageEffect pixelateEffect = null;
    
    [SerializeField]
    Transform planeAntennaTransform = null;
    
    [SerializeField]
    Transform groundAntennaTransform = null;

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

    //----------------------------------------------------------------------------------------------------

    public float RSSI => rssiValue; // 0...1

    public void Init( Vector3 groundAntennaPosition )
    {
        this.groundAntennaPosition = groundAntennaPosition;
    }

    //----------------------------------------------------------------------------------------------------

    void OnEnable()
    {
        targetPixelateIntensity = 0f;
    }

    void Update()
    {
        var time = Time.time;
        if( time - lastUpdateTime > 1f / updateRate )
        {
            lastUpdateTime = time;
            UpdateState();
        }
        
        if( pixelateEffect )
        {
            var smoothedPixelateIntensity = Mathf.Lerp( pixelateEffect.Intensity, targetPixelateIntensity, math.saturate( Time.deltaTime * 4f ) );
            if( smoothedPixelateIntensity > 0.99f )
            {
                smoothedPixelateIntensity = 1f;
            }
        
            pixelateEffect.Intensity = smoothedPixelateIntensity;
        }
    }

    //----------------------------------------------------------------------------------------------------

    Vector3 groundAntennaPosition;
    float distance;
    float rssiDecibels;
    float rssiValue;
    float obstacleWidth;
    float obstacleFactor;
    float lastUpdateTime;
    float targetPixelateIntensity;


    void UpdateState()
    {
        if( !planeAntennaTransform )
        {
            return;
        }

        var positionA = planeAntennaTransform.position + new Vector3( 0f, 1f, 0f );
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

        rssiValue = math.remap( rssiMax, rssiMin, 1f, 0f, rssiDecibels );
        rssiValue += Mathf.Lerp( -0.05f, 0.05f, Mathf.PerlinNoise( Time.fixedTime * 0.5f, Time.fixedTime ) );
        rssiValue = math.saturate( rssiValue );
        rssiValue *= obstacleFactor;
        
        targetPixelateIntensity = math.remap( 0.2f, 0f, 0f, 1f, rssiValue );
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

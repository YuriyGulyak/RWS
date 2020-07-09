using UnityEngine;

[CreateAssetMenu]
public class RateProfile : ScriptableObject
{
    [Space]

    [SerializeField, Range( 0f, 1f )]
    float rollExpo = 0.2f;
    
    [SerializeField, Range( 0f, 1f )]
    float rollSuperRate = 0.75f;

    [Space]

    [SerializeField, Range( 0f, 1f )]
    float pitchExpo = 0.2f;
    
    [SerializeField, Range( 0f, 1f )]
    float pitchSuperRate = 0.75f;
    
    //--------------------------------------------------------------------------------------------------------------

    public float EvaluateRoll( float roll )
    {
        return BfCalc( roll, 1f, rollExpo, rollSuperRate ) / maxRollRate;
    }

    public float EvaluatePitch( float pitch )
    {
        return BfCalc( pitch, 1f, pitchExpo, pitchSuperRate ) / maxPitchRate;
    }
    
    //--------------------------------------------------------------------------------------------------------------
    
    [SerializeField, HideInInspector]
    float maxRollRate;
    
    [SerializeField, HideInInspector]
    float maxPitchRate;

    
    void OnValidate()
    {
        maxRollRate = BfCalc( 1f, 1f, rollExpo, rollSuperRate );
        maxPitchRate = BfCalc( 1f, 1f, pitchExpo, pitchSuperRate );
    }

    
    // BetaFlight Rates. Return angular velocity, deg/s
    // https://apocolipse.github.io/RotorPirates/ - from page js code
    static float BfCalc( float rcCommand, float rcRate, float expo, float superRate )
    {
        if( rcRate > 2f )
        {
            rcRate = rcRate + ( 14.54f * ( rcRate - 2f ) );
        }

        if( !expo.Equals( 0f ) )
        {
            rcCommand = rcCommand * Mathf.Pow( Mathf.Abs( rcCommand ), 3f ) * expo + rcCommand * ( 1f - expo );
        }

        var angleRate = 200f * rcRate * rcCommand;

        if( !superRate.Equals( 0f ) )
        {
            var rcSuperFactor = 1f / ( Mathf.Clamp( 1f - ( Mathf.Abs( rcCommand ) * superRate ), 0.01f, 1f ) );
            angleRate *= rcSuperFactor;
        }

        return angleRate;
    }
}

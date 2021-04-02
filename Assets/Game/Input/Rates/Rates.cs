using UnityEngine;

namespace RWS
{
    public static class Rates
    {
        // BetaFlight Rates. Return angular velocity, deg/s
        // https://apocolipse.github.io/RotorPirates/ - from page js code
        public static float BfCalc( float rcCommand, float rcRate, float expo, float superRate )
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
}

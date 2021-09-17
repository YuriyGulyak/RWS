using UnityEngine;

public static class Aerodynamics
{
    // b - Wing span
    // p - Air density. (kg/m3)
    // V - Speed of the airplane relative to the air. (m/s)
    // S - Wing surface. (m2)
    // CL - Lift coefficient. (no unit)
    // CD - Drag coefficient. (no unit)
    // CDi - Induced drag coefficient. (no unit)
    // AR - Aspect ratio. (no unit)
    // e - Oswald factor. (Usually has a value between 0.8 and 0.9) (no unit)

    public const float p = 1.225f;

    // Lift force. (N)
    public static float L( float V, float S, float CL )
    {
        // https://www.grc.nasa.gov/www/k-12/airplane/lifteq.html
        return CL * ( 0.5f * p * ( V * V ) * S );
    }

    // Drag force. (N)
    public static float D( float V, float S, float CD )
    {
        return CD * ( 0.5f * p * ( V * V ) * S );
    }

    // Wing aspect ratio
    public static float AR( float b, float S )
    {
        return ( b * b ) / S;
    }

    // Induced Drag
    public static float CDi( float CL, float AR, float e )
    {
        return ( CL * CL ) / ( Mathf.PI * AR * e );
    }

    // Propeller thrust force. (N)
    public static float T( float A, float V0, float Ve )
    {
        // https://www.grc.nasa.gov/www/k-12/airplane/propth.html
        return 0.5f * p * A * ( Ve * Ve - V0 * V0 );
    }
}
using UnityEngine;

public static class MathUtils
{
    // Km/h <--> M/s
    public const float Ms2Kmh = 3.6f;
    public const float Kmh2Ms = 1f / 3.6f;
    
    
    // Meters <--> Inches
    public const float Inch2Meter = 0.0254f;
    public const float Meter2Inch = 1f / 0.0254f;


    // -180...+180 to -90...+90
    public static float WrapAngle90( float angle )
    {
        if( angle > 90f )
        {
            angle = 180f - angle;
        }
        if( angle < -90f )
        {
            angle = -180f - angle;
        }
        
        return angle;
    }
    
    
    // 0...360 to -180...+180
    public static float WrapAngle180( float angle )
    {
        if( angle > 180f )
        {
            angle = angle - 360f;
        }
        if( angle < -180f )
        {
            angle = 360f - Mathf.Abs( angle );
        }
        return angle;
    }


    // Distance around circle
    public static float Circumference( float radius )
    {
        return radius * 2f * Mathf.PI;
    }
    
    
    // a, b, c - sides, c - opposite side
    public static float LawOfCos( float a, float b, float c )
    {
        var cos = ( a * a + b * b - c * c ) / ( 2f *  a * b );
        return Mathf.Acos( cos ) * Mathf.Rad2Deg;
    }

    
    // a, b - legs of the triangle
    //public static float Hypotenuse( float a, float b )
    //{
    //    return Mathf.Sqrt( a * a + b * b );
    //}
    
    
    //public static float Remap( float a1, float b1, float t1, float a2, float b2 )
    //{
    //    return ( t1 - a1 ) * ( b2 - a2 ) / ( b1 - a1 ) + a2;
    //}
}
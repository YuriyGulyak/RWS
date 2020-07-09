using UnityEngine;

public static class Bezier
{
    public static Vector3 EvaluateQuadratic( Vector3 a, Vector3 b, Vector3 c, float t )
    {
        var p0 = Vector3.Lerp( a, b, t );
        var p1 = Vector3.Lerp( b, c, t );
        return Vector3.Lerp( p0, p1, t );
    }

    public static Vector3 EvaluateCubic( Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t )
    {
        var p0 = EvaluateQuadratic( a, b, c, t );
        var p1 = EvaluateQuadratic( b, c, d, t );
        return Vector3.Lerp( p0, p1, t );
    }
}
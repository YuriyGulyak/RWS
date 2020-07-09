using UnityEngine;

public class PathPlacer : MonoBehaviour
{
    public float spacing = 0.1f;
    public float resolution = 1;


    void Start()
    {
        var points = FindObjectOfType<PathCreator>().path.CalculateEvenlySpacedPoints( spacing, resolution );

        foreach( var point in points )
        {
            var g = GameObject.CreatePrimitive( PrimitiveType.Sphere );
            g.transform.position = point;
            g.transform.localScale = Vector3.one * ( spacing * 0.5f );
        }
    }
}
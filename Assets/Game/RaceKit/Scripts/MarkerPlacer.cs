using UnityEngine;
using Random = UnityEngine.Random;

public class MarkerPlacer : MonoBehaviour
{
    [SerializeField] 
    GameObject pointPrefab = null;
    
    [SerializeField]
    GameObject arrowPrefab = null;
    
    [SerializeField]
    float spacing = 1f;
    
    [SerializeField]
    float resolution = 1f;

    [SerializeField]
    float random = 0.1f;

    [SerializeField]
    int arrowEvery = 5;
    
    [SerializeField]
    LayerMask terrainLayer = default;


    public void PlaceMarkers()
    {
        var parent = GetComponent<Transform>();

        while( parent.childCount > 0 )
        {
            DestroyImmediate( parent.GetChild( 0 ).gameObject );
        }
        
        var points = GetComponent<PathCreator>().path.CalculateEvenlySpacedPoints( spacing, resolution );
        
        for( var i = 0; i < points.Length; i++ )
        {
            var markerPosition = points[ i ];
            var markerRotation = Quaternion.identity;
            var ray = new Ray( markerPosition, Vector3.down );
            var isArrow = i > 0 && i < points.Length - 1 && i % arrowEvery == 0;
            
            if( Physics.Raycast( ray, out var hit, 100f, terrainLayer, QueryTriggerInteraction.Ignore ) )
            {
                markerPosition.y -= hit.distance;
                markerRotation = Quaternion.LookRotation( hit.normal ) * Quaternion.Euler( 90f, 0f, 0f );
            }

            if( isArrow )
            {
                var dirToNextPoint = ( points[ i + 1 ] - points[ i ] ).normalized;
                var angleToNextPoint = Vector3.SignedAngle( markerRotation * Vector3.forward, dirToNextPoint, markerRotation * Vector3.up );
                markerRotation *= Quaternion.Euler( 0f, angleToNextPoint, 0f );
            }

            markerPosition.x += Random.Range( -random, random );
            markerPosition.z += Random.Range( -random, random );

            var markerPrefab = isArrow ? arrowPrefab : pointPrefab;
            var markerGameObject = Instantiate( markerPrefab, parent );
            markerGameObject.name = $"{markerPrefab.name} ({i + 1})";
                
            var markerTransform = markerGameObject.transform;
            markerTransform.position = markerPosition;
            markerTransform.rotation = markerRotation;
        }
    }
}
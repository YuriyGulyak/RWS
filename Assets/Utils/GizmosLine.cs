//#if UNITY_EDITOR
using UnityEngine;

public class GizmosLine : MonoBehaviour
{
    [SerializeField]
    Transform[] points = null;
    
    [SerializeField]
    Color color = Color.white;

    [SerializeField]
    bool loop = false;

    
    void OnDrawGizmos()
    {
        if( points == null || points.Length < 2 )
        {
            return;
        }
        
        var gizmosColorTemp = Gizmos.color;
        
        Gizmos.color = color;
        for( var i = 0; i < points.Length - 1; i++ )
        {
            Gizmos.DrawLine( points[ i ].position, points[ i + 1 ].position );
        }
        if( loop )
        {
            Gizmos.DrawLine( points[ 0 ].position, points[ points.Length - 1 ].position );
        }

        Gizmos.color = gizmosColorTemp;
    }
}
//#endif

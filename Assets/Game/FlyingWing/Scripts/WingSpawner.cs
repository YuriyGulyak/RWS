using UnityEngine;

public class WingSpawner : MonoBehaviour
{
    [SerializeField]
    Transform[] spawnPoints = null;
    
    [SerializeField]
    GameObject localPlayerWingPrefab = null;
    
    [SerializeField]
    GameObject remotePlayerWingPrefab = null;

    [SerializeField]
    LayerMask terrainLayer = default;

    [SerializeField]
    float spawnHeight = 1.8f;

    [SerializeField]
    float spawnAngle = 20f;
    
    //----------------------------------------------------------------------------------------------------
    
    public GameObject SpawnLocalPlayerWing( int spawnPointIndex )
    {
        var position = GetSpawnPosition( spawnPointIndex );
        var rotation = GetSpawnRotation();
        
        return Instantiate( localPlayerWingPrefab, position, rotation );
    }
    
    public GameObject SpawnRemotePlayerWing( Vector3 position, Quaternion rotation )
    {
        return Instantiate( remotePlayerWingPrefab, position, rotation );
    }

    public Vector3 GetSpawnPosition( int spawnPointIndex )
    {
        var spawnPosition = spawnPoints[ spawnPointIndex ].position;
        
        Physics.Raycast( spawnPosition, Vector3.down, out var hit, 100f, terrainLayer );
        spawnPosition.y = hit.point.y + spawnHeight;
        
        return spawnPoints[ spawnPointIndex ].position;
    }

    public Quaternion GetSpawnRotation()
    {
        return Quaternion.Euler( -spawnAngle, 0f, 0f );
    }
    
    //----------------------------------------------------------------------------------------------------
    
    void OnDrawGizmos()
    {
        if( spawnPoints == null || spawnPoints.Length == 0 )
        {
            return;
        }

        var gizmosColorBackup = Gizmos.color;
        Gizmos.color = new Color32( 255, 255, 255, 100 );

        for( var i = 0; i < spawnPoints.Length; i++ )
        {
            var spawnPosition = spawnPoints[ i ].position;
            
            Gizmos.DrawWireSphere( spawnPosition, 0.05f );
            Gizmos.DrawRay( spawnPosition, GetSpawnRotation() * Vector3.forward );
        }

        Gizmos.color = gizmosColorBackup;
    }
}

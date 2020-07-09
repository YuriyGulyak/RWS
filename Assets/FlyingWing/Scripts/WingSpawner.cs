using UnityEngine;

public class WingSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject wingPrefab = null;

    [SerializeField] 
    WingTelemetry wingTelemetry = null;

    [SerializeField]
    BatteryTelemetry batteryTelemetry = null;
    
    [SerializeField]
    MotorTelemetry motorTelemetry = null;


    public void SpawnWing( Vector3 position, Quaternion rotation )
    {
        var wingGameObject = Instantiate( wingPrefab, position, rotation );
        wingGameObject.name = wingPrefab.name;
        
        wingTelemetry.Init( wingGameObject.GetComponentInChildren<FlyingWing>() );
        motorTelemetry.Init( wingGameObject.GetComponentInChildren<Motor>() );
        batteryTelemetry.Init( wingGameObject.GetComponentInChildren<Battery>() );
    }


    void Awake()
    {
        SpawnWing( transform.position, transform.rotation );
    }
}

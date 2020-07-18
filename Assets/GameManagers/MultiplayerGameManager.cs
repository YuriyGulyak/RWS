using System.Collections;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerGameManager : MonoBehaviour, IOnEventCallback
{
    [SerializeField] 
    GameObject pressSpacebarText = null;

    [SerializeField]
    GameObject orbitCamera = null;

    [SerializeField]
    GameObject inputInfo = null;

    [SerializeField]
    GameObject wingLocalPlayerPrefab = null;
    
    [SerializeField]
    GameObject wingRemotePlayerPrefab = null;

    [SerializeField]
    WingLauncher wingLauncher = null;

    [SerializeField]
    WingTelemetry wingTelemetry = null;

    [SerializeField]
    BatteryTelemetry batteryTelemetry = null;

    [SerializeField] 
    MotorTelemetry motorTelemetry = null;
    
    [SerializeField]
    RaceTrack raceTrack = null;
    
    [SerializeField]
    LapTimer lapTimer = null;
    
    [SerializeField]
    int photonSendRate = 60; // Default 20

    [SerializeField]
    int photonSerializationRate = 60; // Default 10

    //----------------------------------------------------------------------------------------------------

    public void OnEvent( EventData photonEvent )
    {
        var eventCode = (RaiseEventCodes)photonEvent.Code;
        
        if( eventCode == RaiseEventCodes.SpawnRemotePlayerWing )
        {
            var data = (object[])photonEvent.CustomData;
            
            var wingPosition = (Vector3)data[0];
            var wingRotation = (Quaternion)data[1];
            var viewID = (int)data[2];
            
            SpawnRemotePlayerWing( wingPosition, wingRotation ).GetComponent<PhotonView>().ViewID = viewID;
        }
    }

    //----------------------------------------------------------------------------------------------------

    readonly string bestLapKey = "BestLap";
    readonly string playerTag = "Player";
    
    enum RaiseEventCodes : byte
    {
        SpawnRemotePlayerWing = 1
    }

    GameObject localWingGameObject;
    Rigidbody localWingRigidbody;
    

    void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget( this );
    }

    void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget( this );
    }

    void Awake()
    {
        PhotonNetwork.SendRate = photonSendRate;
        PhotonNetwork.SerializationRate = photonSerializationRate;
        
        if( PlayerPrefs.HasKey( bestLapKey ) )
        {
            lapTimer.Init( PlayerPrefs.GetFloat( bestLapKey ) );
        }
        lapTimer.OnNewBestTime += newBestTime =>
        {
            PlayerPrefs.SetFloat( bestLapKey, newBestTime );
        };
        lapTimer.Hide();

        raceTrack.OnStart.AddListener( craft =>
        {
            if( craft.CompareTag( playerTag ) )
            {
                lapTimer.StartNewTime();
            }
        } );
        raceTrack.OnFinish.AddListener( craft =>
        {
            if( craft.CompareTag( playerTag ) )
            {
                lapTimer.CompareTime();
            }
        } );
    }

    IEnumerator Start()
    {
        if( !PhotonNetwork.IsConnected )
        {
            yield break;
        }

        
        var wingPosition = new Vector3( Random.Range( -50f, 50f ), 10f, -250f );
        var wingRotation = Quaternion.Euler( -10f, 0f, 0f );
        
        localWingGameObject = SpawnLocalPlayerWing( wingPosition, wingRotation );
        localWingRigidbody = localWingGameObject.GetComponent<Rigidbody>();
        
        var photonView = localWingGameObject.GetComponent<PhotonView>();
        PhotonNetwork.AllocateViewID( photonView );
        
        SendEventToSpawnRemotePlayerWing( wingPosition, wingRotation, photonView.ViewID );

        
        orbitCamera.transform.position = wingPosition - new Vector3( 0f, 0f, 0.1f );

        if( wingTelemetry )
        {
            wingTelemetry.Init( localWingGameObject.GetComponent<FlyingWing>() );
        }
        if( batteryTelemetry )
        {
            batteryTelemetry.Init( localWingGameObject.GetComponentInChildren<Battery>() );
        }
        if( motorTelemetry )
        {
            motorTelemetry.Init( localWingGameObject.GetComponentInChildren<Motor>() );
        }

        
        yield return new WaitUntil( () => Keyboard.current.spaceKey.wasPressedThisFrame );

        
        pressSpacebarText.SetActive( false );
        inputInfo.SetActive( false );
        orbitCamera.SetActive( false );

        var fpvCamera = localWingGameObject.GetComponentInChildren<Camera>( true );
        fpvCamera.gameObject.SetActive( true );

        var playerInput = PlayerInputWrapper.Instance;
        playerInput.Launch.AddListener( () => wingLauncher.Launch( localWingRigidbody ) );
        playerInput.Restart.AddListener( RespawnLocalPlayerWing );
    }


    GameObject SpawnLocalPlayerWing( Vector3 position, Quaternion rotation )
    {
        return Instantiate( wingLocalPlayerPrefab, position, rotation );
    }

    GameObject SpawnRemotePlayerWing( Vector3 position, Quaternion rotation )
    {
        return Instantiate( wingRemotePlayerPrefab, position, rotation );
    }

    void SendEventToSpawnRemotePlayerWing( Vector3 position, Quaternion rotation, int viewID )
    {
        var content = new object[]
        {
            position, rotation, viewID
        };
        var eventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.Others,
            CachingOption = EventCaching.AddToRoomCache
        };
        var eventCode = (byte)RaiseEventCodes.SpawnRemotePlayerWing;
        
        PhotonNetwork.RaiseEvent( eventCode, content, eventOptions, SendOptions.SendReliable );
    }

    void RespawnLocalPlayerWing()
    {
        localWingRigidbody.isKinematic = true;
        localWingRigidbody.position = new Vector3( Random.Range( -50f, 50f ), 10f, -250f );
        localWingRigidbody.rotation = Quaternion.Euler( -10f, 0f, 0f );

        wingLauncher.Reset();
        
        lapTimer.Reset();
        lapTimer.Hide();
    }
}
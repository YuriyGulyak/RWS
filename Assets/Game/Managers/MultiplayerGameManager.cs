using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace RWS
{
    public class MultiplayerGameManager : MonoBehaviour, IOnEventCallback
    {
        [SerializeField]
        GameObject pilotAvatar = null;

        [SerializeField]
        WingSpawner wingSpawner = null;

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
        LapTime lapTime = null;

        [SerializeField]
        PlayerOverviewPanel playerOverviewPanel = null;
        
        [SerializeField]
        OSDTelemetry osdTelemetry = null;

        [SerializeField]
        OSDHome osdHome = null;
        
        [SerializeField]
        AttitudeIndicator attitudeIndicator = null;
        
        [SerializeField]
        GameMenu gameMenu = null;

        [SerializeField]
        SettingsPanel settingsPanel = null;

        [SerializeField]
        RoomChat roomChat = null;
        
        [SerializeField]
        BlackScreen blackScreen = null;
        
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

                var wingPosition = (Vector3)data[ 0 ];
                var wingRotation = (Quaternion)data[ 1 ];
                var viewID = (int)data[ 2 ];

                var wingGameObject = wingSpawner.SpawnRemotePlayerWing( wingPosition, wingRotation );

                var photonView = wingGameObject.GetComponent<PhotonView>();
                photonView.ViewID = viewID;

                if( remoteWingDictionary == null )
                {
                    remoteWingDictionary = new Dictionary<int, GameObject>();
                }
                remoteWingDictionary.Add( viewID, wingGameObject );
            }
        }

        //----------------------------------------------------------------------------------------------------

        readonly string bestLapPropertyKey = "BestLapProperty";
        readonly string playerTag = "Player";

        enum RaiseEventCodes : byte
        {
            SpawnRemotePlayerWing = 1
        }

        FlyingWing localFlyingWing;
        GameObject localWingGameObject;
        Dictionary<int, GameObject> remoteWingDictionary;
        int spawnPointIndex;
        bool gameStarted;


        void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget( this );
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget( this );
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void Awake()
        {
            //PhotonNetwork.GetPing();
            
            PhotonNetwork.SendRate = photonSendRate;
            PhotonNetwork.SerializationRate = photonSerializationRate;

            osdTelemetry.Hide();
            osdHome.Hide();
            attitudeIndicator.Hide();
            
            lapTime.Init( 0f );
            lapTime.OnNewBestTime += newBestTime =>
            {
                // Receiving in PlayerOverviewPanel
                PhotonNetwork.LocalPlayer.SetCustomProperties( new Hashtable { { bestLapPropertyKey, newBestTime } } );
            };
            lapTime.Hide();

            raceTrack.OnStart.AddListener( craft =>
            {
                if( craft.CompareTag( playerTag ) )
                {
                    lapTime.StartNewTime();
                }
            } );
            raceTrack.OnFinish.AddListener( craft =>
            {
                if( craft.CompareTag( playerTag ) )
                {
                    lapTime.CompareTime();
                }
            } );

            playerOverviewPanel.Hide();
            settingsPanel.Hide();
            gameMenu.Show();

            gameMenu.OnStartButton += OnStartButton;
            gameMenu.OnResumeButton += OnResumeButton;
            gameMenu.OnSettingsButton += OnSettingsButton;
            gameMenu.OnExitButton += OnExitButton;

            InputManager.Instance.OnEscapeButton += OnEscapeButton;

            if( PhotonNetwork.IsConnected )
            {
                spawnPointIndex = PhotonNetwork.LocalPlayer.ActorNumber;
                localWingGameObject = wingSpawner.SpawnLocalPlayerWing( spawnPointIndex );
                localFlyingWing = localWingGameObject.GetComponent<FlyingWing>();

                var wingTransform = localWingGameObject.transform;
                var wingPosition = wingTransform.position;
                var wingRotation = wingTransform.rotation;

                var pilotPosition = wingPosition + new Vector3( -0.75f, 0f, 0.3f );
                Physics.Raycast( pilotPosition, Vector3.down, out var hit );
                pilotPosition.y = hit.point.y;
                
                pilotAvatar.transform.position = pilotPosition;
                pilotAvatar.GetComponent<PilotLook>().SetTarget( wingTransform );

                var motor = localWingGameObject.GetComponentInChildren<Motor>();
                var battery = localWingGameObject.GetComponentInChildren<Battery>();
                
                osdTelemetry.Init( localFlyingWing, motor, battery );
                osdHome.Init( localFlyingWing );
                attitudeIndicator.Init( localFlyingWing );
                
                if( wingTelemetry )
                {
                    wingTelemetry.Init( localFlyingWing );
                }
                if( batteryTelemetry )
                {
                    batteryTelemetry.Init( battery );
                }
                if( motorTelemetry )
                {
                    motorTelemetry.Init( motor );
                }

                var wingPhotonView = localWingGameObject.GetComponent<PhotonView>();
                PhotonNetwork.AllocateViewID( wingPhotonView );
                SendEventToSpawnRemotePlayerWing( wingPosition, wingRotation, wingPhotonView.ViewID );
            }
        }

        
        void OnSceneLoaded( Scene scene, LoadSceneMode mode )
        {
            BlackScreen.Instance.StartFromBlackScreenAnimation();
        }

        void OnStartButton()
        {
            if( !PhotonNetwork.IsConnected )
            {
                return;
            }

            gameMenu.Hide();

            blackScreen.StartToBlackScreenAnimation( () =>
            {
                var pilotCamera = pilotAvatar.GetComponentInChildren<Camera>( true );
                pilotCamera.gameObject.SetActive( false );
            
                var fpvCamera = localWingGameObject.GetComponentInChildren<Camera>( true );
                fpvCamera.gameObject.SetActive( true );

                osdTelemetry.Show();
                osdHome.Show();
                attitudeIndicator.Show();
                
                playerOverviewPanel.Show();

                roomChat.HideInput();
                
                Cursor.visible = false;

                blackScreen.StartFromBlackScreenAnimation( () =>
                {
                    var inputManager = InputManager.Instance;
                    inputManager.LaunchControl.Performed += OnLaunchButton;
                    inputManager.ResetControl.Performed += OnResetButton;
                    
                    gameStarted = true;
                    
                } );
                
            } );
        }

        void OnResumeButton()
        {
            ShowOSD();
            gameMenu.Hide();
            settingsPanel.Hide();
            playerOverviewPanel.Show();
            roomChat.HideInput();
            
            Cursor.visible = false;
        }

        void OnSettingsButton()
        {
            settingsPanel.Show();
        }

        void OnExitButton()
        {
            BlackScreen.Instance.StartToBlackScreenAnimation( () =>
            {
                StartCoroutine( ExitCoroutine() );
            } );
        }

        void OnLaunchButton()
        {
            wingLauncher.Launch( localFlyingWing.Rigidbody );
        }

        void OnResetButton()
        {
            blackScreen.StartToBlackScreenAnimation( () =>
            {
                var localWingRigidbody = localFlyingWing.Rigidbody;
                localWingRigidbody.isKinematic = true;
                localWingRigidbody.position = wingSpawner.GetSpawnPosition( spawnPointIndex );
                localWingRigidbody.rotation = wingSpawner.GetSpawnRotation();
                
                localFlyingWing.Reset();
                
                osdTelemetry.Reset();
                osdHome.Reset();
                attitudeIndicator.Reset();
                
                lapTime.Reset();
                lapTime.Hide();
                
                blackScreen.StartFromBlackScreenAnimation( wingLauncher.Reset );
                
            } );
        }

        void OnEscapeButton()
        {
            if( !gameStarted )
            {
                return;
            }

            if( !gameMenu.IsActive )
            {
                HideOSD();
                playerOverviewPanel.Hide();
                gameMenu.Show();
                roomChat.ShowInput();
                Cursor.visible = true;
            }
        }

        
        void ShowOSD()
        {
            osdTelemetry.Show();
            osdHome.Show();
            attitudeIndicator.Show();

            if( lapTime.Started )
            {
                lapTime.Show();
            }
        }

        void HideOSD()
        {
            osdTelemetry.Hide();
            osdHome.Hide();
            attitudeIndicator.Hide();
            lapTime.Hide();
        }

        IEnumerator ExitCoroutine()
        {
            Destroy( localWingGameObject );

            if( remoteWingDictionary != null && remoteWingDictionary.Count > 0 )
            {
                foreach( var entry in remoteWingDictionary )
                {
                    Destroy( entry.Value );
                }
            }

            PhotonNetwork.Disconnect();
            yield return new WaitWhile( () => PhotonNetwork.IsConnected );
            
            SceneManager.LoadSceneAsync( 0 );
        }

        void SendEventToSpawnRemotePlayerWing( Vector3 position, Quaternion rotation, int viewID )
        {
            var content = new object[] { position, rotation, viewID };
            var eventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others, CachingOption = EventCaching.AddToRoomCache };
            var eventCode = (byte) RaiseEventCodes.SpawnRemotePlayerWing;

            PhotonNetwork.RaiseEvent( eventCode, content, eventOptions, SendOptions.SendReliable );
        }
    }
}
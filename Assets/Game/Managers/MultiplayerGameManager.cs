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
        GameMenu gameMenu = null;

        [SerializeField]
        SettingsPanel settingsPanel = null;

        [SerializeField]
        RoomChat roomChat = null;

        [SerializeField]
        BlackScreen blackScreen = null;

        [SerializeField]
        BloorEffectController bloorEffect = null;

        [SerializeField]
        int photonSendRate = 60; // Default 20

        [SerializeField]
        int photonSerializationRate = 60; // Default 10

        //----------------------------------------------------------------------------------------------------

        public void OnEvent( EventData photonEvent )
        {
            var eventCode = (RaiseEventCodes) photonEvent.Code;

            if( eventCode == RaiseEventCodes.SpawnRemotePlayerWing )
            {
                var data = (object[]) photonEvent.CustomData;

                var wingPosition = (Vector3) data[ 0 ];
                var wingRotation = (Quaternion) data[ 1 ];
                var viewID = (int) data[ 2 ];

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
        readonly string localBestLapKey = "LocalBestLap";
        readonly string dreamloPrivateCode = "ZbIxEmdjiU6we6WbZHRGZQp8S0j4TytEuwOlNrARz_Aw";

        enum RaiseEventCodes : byte
        {
            SpawnRemotePlayerWing = 1
        }

        FlyingWing localWing;
        GameObject localWingGameObject;
        Dictionary<int, GameObject> remoteWingDictionary;
        int spawnPointIndex;
        PilotLook pilotLook;
        GameObject losCameraGameObject;
        GameObject fpvCameraGameObject;
        Vector3 spawnPosition;
        Quaternion spawnRotation;
        float lastLaunchTime;
        bool fpvMode;
        Leaderboard leaderboard;


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
            PhotonNetwork.SendRate = photonSendRate;
            PhotonNetwork.SerializationRate = photonSerializationRate;

            gameMenu.OnResumeButton += OnResumeButton;
            gameMenu.OnSettingsButton += OnSettingsButton;
            gameMenu.OnExitButton += OnExitButton;

            var inputManager = InputManager.Instance;
            inputManager.LaunchResetControl.Performed += OnLaunchResetButton;
            inputManager.ViewControl.Performed += OnViewButton;
            inputManager.OnEnterButton += OnEnterButton;
            inputManager.OnEscapeButton += OnEscapeButton;

            leaderboard = Leaderboard.Instance;
        }

        void Start()
        {
            gameMenu.Hide();
            osdTelemetry.Hide();
            playerOverviewPanel.Show();
            settingsPanel.Hide();
            roomChat.HideInput();

            losCameraGameObject = pilotAvatar.GetComponentInChildren<Camera>( true ).gameObject;
            losCameraGameObject.SetActive( true );

            lapTime.Init( PlayerPrefs.GetFloat( localBestLapKey, 0f ) );
            lapTime.OnNewBestTime += newBestTime =>
            {
                // TODO
                // Receiving in PlayerOverviewPanel
                PhotonNetwork.LocalPlayer.SetCustomProperties( new Hashtable { { bestLapPropertyKey, newBestTime } } );


                PlayerPrefs.SetFloat( localBestLapKey, newBestTime );

                var pilotName = PlayerPrefs.GetString( "Nickname", "" );
                if( !string.IsNullOrEmpty( pilotName ) )
                {
                    leaderboard.AddRecord( dreamloPrivateCode, pilotName, "Mini Race Wing", newBestTime, null );
                }
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

            Cursor.visible = false;


            if( PhotonNetwork.IsConnected )
            {
                spawnPointIndex = PhotonNetwork.LocalPlayer.ActorNumber;
                localWingGameObject = wingSpawner.SpawnLocalPlayerWing( spawnPointIndex );
                localWing = localWingGameObject.GetComponent<FlyingWing>();

                var wingTransform = localWing.Transform;
                spawnPosition = wingTransform.position;
                spawnRotation = wingTransform.rotation;

                var pilotPosition = spawnPosition + new Vector3( -0.75f, 0f, 0.3f );
                if( Physics.Raycast( pilotPosition, Vector3.down, out var hit ) )
                {
                    pilotPosition.y = hit.point.y;
                }

                pilotAvatar.transform.position = pilotPosition;

                pilotLook = pilotAvatar.GetComponentInChildren<PilotLook>( true );
                pilotLook.SetTarget( wingTransform );

                pilotAvatar.GetComponentInChildren<PilotZoom>().SetTarget( wingTransform );

                localWing.Transceiver.Init( pilotPosition + new Vector3( 0f, 2f, 0f ) );

                osdTelemetry.Init( localWing );
                wingTelemetry.Init( localWing );
                batteryTelemetry.Init( localWing.Battery );
                motorTelemetry.Init( localWing.Motor );

                var roomCustomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
                if( roomCustomProperties.TryGetValue( "InfiniteBattery", out var infiniteCapacityObj ) )
                {
                    localWing.Battery.InfiniteCapacity = (bool) infiniteCapacityObj;
                }

                if( roomCustomProperties.TryGetValue( "InfiniteRange", out var infiniteRangeObj ) )
                {
                    localWing.Transceiver.InfiniteRange = (bool) infiniteRangeObj;
                }

                fpvCameraGameObject = localWingGameObject.GetComponentInChildren<Camera>( true ).gameObject;
                fpvCameraGameObject.SetActive( false );

                var wingPhotonView = localWingGameObject.GetComponent<PhotonView>();
                PhotonNetwork.AllocateViewID( wingPhotonView );
                SendEventToSpawnRemotePlayerWing( spawnPosition, spawnRotation, wingPhotonView.ViewID );
            }
        }

        void OnSceneLoaded( Scene scene, LoadSceneMode mode )
        {
            BlackScreen.Instance.StartFromBlackScreenAnimation();
        }


        void OnResumeButton()
        {
            gameMenu.Hide();
            settingsPanel.Hide();
            playerOverviewPanel.Show();
            roomChat.HideInput();
            Cursor.visible = false;
            bloorEffect.BloorEffectEnabled = false;

            if( fpvMode )
            {
                osdTelemetry.Show();

                if( lapTime.Started )
                {
                    lapTime.Show();
                }
            }
        }

        void OnSettingsButton()
        {
            settingsPanel.Show();
        }

        void OnExitButton()
        {
            BlackScreen.Instance.StartToBlackScreenAnimation( () => { StartCoroutine( ExitCoroutine() ); } );
        }


        void OnViewButton()
        {
            if( gameMenu.IsActive )
            {
                return;
            }

            // FPV
            if( fpvMode )
            {
                blackScreen.StartToBlackScreenAnimation( () =>
                {
                    losCameraGameObject.SetActive( true );
                    fpvCameraGameObject.SetActive( false );

                    osdTelemetry.Hide();

                    blackScreen.StartFromBlackScreenAnimation( () => { fpvMode = false; } );
                } );
            }

            // LOS (line of sight)
            else
            {
                blackScreen.StartToBlackScreenAnimation( () =>
                {
                    losCameraGameObject.SetActive( false );
                    fpvCameraGameObject.SetActive( true );

                    osdTelemetry.Show();

                    blackScreen.StartFromBlackScreenAnimation( () => { fpvMode = true; } );
                } );
            }
        }

        void OnLaunchResetButton()
        {
            if( gameMenu.IsActive )
            {
                return;
            }

            if( Time.time - lastLaunchTime < 2f )
            {
                return;
            }

            // Launch
            if( wingLauncher.Ready )
            {
                wingLauncher.Launch( localWing.Rigidbody );
                lastLaunchTime = Time.time;
            }

            // Reset
            else
            {
                blackScreen.StartToBlackScreenAnimation( () =>
                {
                    localWing.Reset( spawnPosition, spawnRotation );

                    osdTelemetry.Reset();

                    lapTime.Reset();
                    lapTime.Hide();

                    pilotLook.LookAtTarget();

                    blackScreen.StartFromBlackScreenAnimation( wingLauncher.Reset );
                } );
            }
        }

        void OnEnterButton()
        {
            if( gameMenu.IsActive )
            {
                return;
            }

            if( roomChat.IsInputActive )
            {
                roomChat.SendAndHideInput();
            }
            else
            {
                roomChat.ShowInput();
            }
        }

        void OnEscapeButton()
        {
            if( !gameMenu.IsActive )
            {
                osdTelemetry.Hide();
                lapTime.Hide();
                playerOverviewPanel.Hide();
                gameMenu.Show();
                roomChat.ShowInput();
                Cursor.visible = true;
                bloorEffect.BloorEffectEnabled = true;
            }
            else if( !settingsPanel.IsOpen )
            {
                OnResumeButton();
            }
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
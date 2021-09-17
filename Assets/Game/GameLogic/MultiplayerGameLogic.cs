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
    public class MultiplayerGameLogic : MonoBehaviour, IOnEventCallback
    {
        [SerializeField]
        GameObject pilotAvatar = default;

        [SerializeField]
        WingSpawner wingSpawner = default;

        [SerializeField]
        WingLauncher wingLauncher = default;

        [SerializeField]
        RaceTrack raceTrack = default;

        [SerializeField]
        LapTime lapTime = default;

        [SerializeField]
        PlayerOverviewPanel playerOverviewPanel = default;

        [SerializeField]
        GameObject osdTelemetry = default;

        [SerializeField]
        GameMenu gameMenu = default;

        [SerializeField]
        SettingsPanel settingsPanel = default;

        [SerializeField]
        RoomChat roomChat = default;

        [SerializeField]
        BlackScreen blackScreen = default;

        [SerializeField]
        BloorEffectController bloorEffect = default;

        [SerializeField]
        int photonSendRate = 60; // Default 20

        [SerializeField]
        int photonSerializationRate = 60; // Default 10

        [SerializeField]
        Leaderboard leaderboard = default;
        
        [SerializeField]
        BestLapKeys bestLapKeys = default;

        [SerializeField]
        InputManager inputManager = default;
        
        //----------------------------------------------------------------------------------------------------

        public void OnEvent( EventData photonEvent )
        {
            var eventCode = (RaiseEventCodes)photonEvent.Code;

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
        float currentSessionLapStartTime;
        float currentSessionBestLap;
        PlayerProfile playerProfile;
        
        
        void OnValidate()
        {
            if( !leaderboard )
            {
                leaderboard = Leaderboard.Instance;
            }

            if( !inputManager )
            {
                inputManager = InputManager.Instance;
            }
        }
        
        void Awake()
        {
            PhotonNetwork.SendRate = photonSendRate;
            PhotonNetwork.SerializationRate = photonSerializationRate;

            gameMenu.OnResumeButton += OnResumeButton;
            gameMenu.OnSettingsButton += OnSettingsButton;
            gameMenu.OnExitButton += OnExitButton;
            
            inputManager.LaunchResetControl.Performed += OnLaunchResetButton;
            inputManager.ViewControl.Performed += OnViewButton;
            inputManager.OnEnterButton += OnEnterButton;
            inputManager.OnEscapeButton += OnEscapeButton;
        }

        void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget( this );
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget( this );
            SceneManager.sceneLoaded -= OnSceneLoaded;

            if( localWing )
            {
                playerProfile.totalFlightTime += localWing.Flytime;
                playerProfile.totalFlightDistance += localWing.FlightDistance;
                playerProfile.longestFlightTime = Mathf.Max( playerProfile.longestFlightTime, localWing.Flytime );
                playerProfile.topSpeed = Mathf.Max( playerProfile.topSpeed, localWing.Speedometer.SpeedMs );
            }
            PlayerProfileDatabase.SavePlayerProfile( playerProfile );
        }

        void Start()
        {
            playerProfile = PlayerProfileDatabase.LoadPlayerProfile();
            
            gameMenu.Hide();
            osdTelemetry.SetActive( false );
            playerOverviewPanel.Show();
            settingsPanel.Hide();
            roomChat.HideInput();

            losCameraGameObject = pilotAvatar.GetComponentInChildren<Camera>( true ).gameObject;
            losCameraGameObject.SetActive( true );
            
            
            var localBestLapKey = bestLapKeys.playerPrefsKey; 
            var dreamloPrivateCode = bestLapKeys.dreamloPrivateCode; 
            
            lapTime.Init( PlayerPrefs.GetFloat( localBestLapKey, -1f ) );
            lapTime.OnNewBestTime += newBestTime =>
            {
                PlayerPrefs.SetFloat( localBestLapKey, newBestTime );

                var pilotName = PlayerPrefs.GetString( "Nickname", "" );
                if( !string.IsNullOrEmpty( pilotName ) )
                {
                    leaderboard.AddRecord( dreamloPrivateCode, pilotName, "Mini Race Wing", newBestTime, null );
                }
            };
            lapTime.OnTimeOut += () =>
            {
                raceTrack.ResetProgressFor( localWingGameObject );
            };
            lapTime.Hide();

            raceTrack.OnStart.AddListener( craft =>
            {
                if( craft.CompareTag( playerTag ) )
                {
                    lapTime.StartNewTime();
                    
                    // Temp solution
                    currentSessionLapStartTime = Time.time;
                }
            } );
            raceTrack.OnFinish.AddListener( craft =>
            {
                if( craft.CompareTag( playerTag ) )
                {
                    lapTime.CompareTime();
                    
                    // Temp solution
                    var newLapTime = Time.time - currentSessionLapStartTime;
                    if( currentSessionBestLap <= 0f || newLapTime < currentSessionBestLap )
                    {
                        currentSessionBestLap = newLapTime;
                        
                        // Receiving in PlayerOverviewPanel. Need refactoring
                        PhotonNetwork.LocalPlayer.SetCustomProperties( new Hashtable { { bestLapPropertyKey, newLapTime } } );
                    }
                    
                    playerProfile.completedLaps++;
                }
            } );

            Cursor.visible = false;


            if( PhotonNetwork.IsConnected )
            {
                spawnPointIndex = PhotonNetwork.LocalPlayer.ActorNumber;
                localWingGameObject = wingSpawner.SpawnLocalPlayerWing( spawnPointIndex );
                localWing = localWingGameObject.GetComponent<FlyingWing>();
                localWing.Rigidbody.isKinematic = true;
                
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

                localWing.CrashDetector.OnCrashed.AddListener( OnCrash );
                
                var roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
                if( roomProperties.TryGetValue( "InfiniteBattery", out var infiniteCapacityObj ) )
                {
                    localWing.Battery.InfiniteCapacity = (bool) infiniteCapacityObj;
                }
                if( roomProperties.TryGetValue( "InfiniteRange", out var infiniteRangeObj ) )
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
                osdTelemetry.SetActive( true );

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

                    osdTelemetry.SetActive( false );

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

                    osdTelemetry.SetActive( true );

                    blackScreen.StartFromBlackScreenAnimation( () => { fpvMode = true; } );
                } );
            }
        }

        void OnLaunchResetButton()
        {
            if( !localWing )
            {
                return;
            }

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
                
                playerProfile.numberOfLaunches++;
            }

            // Reset
            else
            {
                playerProfile.totalFlightTime += localWing.Flytime;
                playerProfile.totalFlightDistance += localWing.FlightDistance;
                playerProfile.longestFlightTime = Mathf.Max( playerProfile.longestFlightTime, localWing.Flytime );
                playerProfile.topSpeed = Mathf.Max( playerProfile.topSpeed, localWing.Speedometer.TopSpeedMs );
                
                blackScreen.StartToBlackScreenAnimation( () =>
                {
                    localWing.Reset( spawnPosition, spawnRotation );

                    lapTime.Reset();
                    lapTime.Hide();

                    raceTrack.ResetProgressFor( localWingGameObject );
                    
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
                osdTelemetry.SetActive( false );
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
        
        void OnCrash()
        {
            playerProfile.numberOfCrashes++;
        }


        IEnumerator ExitCoroutine()
        {
            playerProfile.totalFlightTime += localWing.Flytime;
            playerProfile.totalFlightDistance += localWing.FlightDistance;
            playerProfile.longestFlightTime = Mathf.Max( playerProfile.longestFlightTime, localWing.Flytime );
            playerProfile.topSpeed = Mathf.Max( playerProfile.topSpeed, localWing.Speedometer.TopSpeedMs );

            Destroy( localWingGameObject );
            losCameraGameObject.SetActive( true );
            
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
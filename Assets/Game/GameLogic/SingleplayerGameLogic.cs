using UnityEngine;
using UnityEngine.SceneManagement;

namespace RWS
{
    public class SingleplayerGameLogic : MonoBehaviour
    {
        [SerializeField]
        GameObject pilotCamera = default;

        [SerializeField]
        PilotLook pilotLook = default;

        [SerializeField]
        FlyingWing flyingWing = default;

        [SerializeField]
        GameObject fpvCamera = default;
        
        [SerializeField]
        WingLauncher wingLauncher = default;

        [SerializeField]
        RaceTrack raceTrack = default;

        [SerializeField]
        LapTime lapTime = default;

        [SerializeField]
        GameObject osdTelemetry = default;

        [SerializeField]
        GameMenu gameMenu = default;

        [SerializeField]
        BlackScreen blackScreen = default;

        [SerializeField]
        SettingsPanel settingsPanel = default;

        [SerializeField]
        BloorEffectController bloorEffect = default;

        [SerializeField]
        GhostReplaySystem ghostReplay = default;

        [SerializeField]
        Leaderboard leaderboard = default;
        
        [SerializeField]
        BestLapKeys bestLapKeys = default;

        [SerializeField]
        InputManager inputManager = default;

        //----------------------------------------------------------------------------------------------------

        const string infiniteBatteryKey = "InfiniteBattery";
        const string infiniteRangeKey = "InfiniteRange";
        const string showGhostKey = "ShowGhost";

        Vector3 spawnPosition;
        Quaternion spawnRotation;
        float lastLaunchTime;
        bool fpvMode;
        bool showGhost;
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

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            inputManager.LaunchResetControl.Performed += OnLaunchResetButton;
            inputManager.ViewControl.Performed += OnViewButton;
            inputManager.OnEscapeButton += OnEscapeButton;
            
            gameMenu.OnResumeButton += OnResumeButton;
            gameMenu.OnSettingsButton += OnSettingsButton;
            gameMenu.OnExitButton += OnExitButton;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            
            inputManager.LaunchResetControl.Performed -= OnLaunchResetButton;
            inputManager.ViewControl.Performed -= OnViewButton;
            inputManager.OnEscapeButton -= OnEscapeButton;
            
            gameMenu.OnResumeButton -= OnResumeButton;
            gameMenu.OnSettingsButton -= OnSettingsButton;
            gameMenu.OnExitButton -= OnExitButton;
            
            
            playerProfile.totalFlightTime += flyingWing.Flytime;
            playerProfile.totalFlightDistance += flyingWing.FlightDistance;
            playerProfile.longestFlightTime = Mathf.Max( playerProfile.longestFlightTime, flyingWing.Flytime );
            playerProfile.topSpeed = Mathf.Max( playerProfile.topSpeed, flyingWing.Speedometer.TopSpeedMs );
            PlayerProfileDatabase.SavePlayerProfile( playerProfile );
        }

        void Start()
        {
            playerProfile = PlayerProfileDatabase.LoadPlayerProfile();
            
            flyingWing.Battery.InfiniteCapacity = PlayerPrefs.GetInt( infiniteBatteryKey, 0 ) > 0;
            flyingWing.Transceiver.InfiniteRange = PlayerPrefs.GetInt( infiniteRangeKey, 0 ) > 0;

            flyingWing.CrashDetector.OnCrashed.AddListener( OnCrash );
            
            spawnPosition = wingLauncher.transform.position;
            spawnRotation = wingLauncher.transform.rotation;

            pilotCamera.SetActive( true );
            fpvCamera.SetActive( false );
            
            osdTelemetry.SetActive( false );
            
            settingsPanel.Hide();
            gameMenu.Hide();

            
            showGhost = PlayerPrefs.GetInt( showGhostKey, 0 ) > 0;

            if( showGhost )
            {
                ghostReplay.LoadReplay();
            }

            raceTrack.OnStart.AddListener( _ =>
            {
                lapTime.StartNewTime();

                ghostReplay.StartRecording();
                
                if( showGhost && ghostReplay.HasReplay )
                {
                    ghostReplay.StartReplaying();
                }
                
            } );
            raceTrack.OnFinish.AddListener( _ =>
            {
                lapTime.CompareTime();
                
                playerProfile.completedLaps++;

            } );
            
            
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

                ghostReplay.SaveReplay();
                
            };
            lapTime.OnTimeOut += () =>
            {
                raceTrack.ResetProgressFor( flyingWing.gameObject );
            };
            lapTime.Hide();

            Cursor.visible = false;
        }

        void OnSceneLoaded( Scene scene, LoadSceneMode mode )
        {
            BlackScreen.Instance.StartFromBlackScreenAnimation();
        }


        void OnResumeButton()
        {
            gameMenu.Hide();
            settingsPanel.Hide();
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
            BlackScreen.Instance.StartToBlackScreenAnimation( () => { SceneManager.LoadSceneAsync( 0 ); } );
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
                    pilotCamera.SetActive( true );
                    fpvCamera.SetActive( false );
                    osdTelemetry.SetActive( false );

                    blackScreen.StartFromBlackScreenAnimation( () => { fpvMode = false; } );
                } );
            }

            // LOS (line of sight)
            else
            {
                blackScreen.StartToBlackScreenAnimation( () =>
                {
                    pilotCamera.SetActive( false );
                    fpvCamera.SetActive( true );
                    
                    osdTelemetry.SetActive( true );

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
                wingLauncher.Launch();
                lastLaunchTime = Time.time;

                playerProfile.numberOfLaunches++;
            }

            // Reset
            else
            {
                playerProfile.totalFlightTime += flyingWing.Flytime;
                playerProfile.totalFlightDistance += flyingWing.FlightDistance;
                playerProfile.longestFlightTime = Mathf.Max( playerProfile.longestFlightTime, flyingWing.Flytime );
                playerProfile.topSpeed = Mathf.Max( playerProfile.topSpeed, flyingWing.Speedometer.TopSpeedMs );
                
                blackScreen.StartToBlackScreenAnimation( () =>
                {
                    flyingWing.Reset( spawnPosition, spawnRotation );

                    lapTime.Reset();
                    lapTime.Hide();

                    raceTrack.ResetProgressFor( flyingWing.gameObject );
                    
                    ghostReplay.StopRecording();
                    if( ghostReplay.IsReplaying )
                    {
                        ghostReplay.StopReplaying();
                    }
                    
                    pilotLook.LookAtTarget();

                    blackScreen.StartFromBlackScreenAnimation( wingLauncher.Reset );
                } );
            }
        }

        void OnEscapeButton()
        {
            if( !gameMenu.IsActive )
            {
                osdTelemetry.SetActive( false );
                lapTime.Hide();
                gameMenu.Show();
                
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
    }
}
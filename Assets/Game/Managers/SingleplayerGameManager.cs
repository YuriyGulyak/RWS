using UnityEngine;
using UnityEngine.SceneManagement;

namespace RWS
{
    public class SingleplayerGameManager : MonoBehaviour
    {
        [SerializeField]
        GameObject pilotCamera = null;

        [SerializeField]
        PilotLook pilotLook = null;

        [SerializeField]
        GameObject fpvCamera = null;

        [SerializeField]
        FlyingWing flyingWing = null;

        [SerializeField]
        WingLauncher wingLauncher = null;

        [SerializeField]
        RaceTrack raceTrack = null;

        [SerializeField]
        LapTime lapTime = null;

        [SerializeField]
        OSDTelemetry osdTelemetry = null;

        [SerializeField]
        GameMenu gameMenu = null;

        [SerializeField]
        BlackScreen blackScreen = null;

        [SerializeField]
        SettingsPanel settingsPanel = null;

        [SerializeField]
        BloorEffectController bloorEffect = null;

        [SerializeField]
        GhostReplaySystem ghostReplay;
        
        //----------------------------------------------------------------------------------------------------

        readonly string infiniteBatteryKey = "InfiniteBattery";
        readonly string infiniteRangeKey = "InfiniteRange";
        readonly string localBestLapKey = "LocalBestLap";
        readonly string showGhostKey = "ShowGhost";
        readonly string dreamloPrivateCode = "ZbIxEmdjiU6we6WbZHRGZQp8S0j4TytEuwOlNrARz_Aw";

        Vector3 spawnPosition;
        Quaternion spawnRotation;
        float lastLaunchTime;
        bool fpvMode;
        Leaderboard leaderboard;
        bool showGhost;


        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void Awake()
        {
            gameMenu.OnResumeButton += OnResumeButton;
            gameMenu.OnSettingsButton += OnSettingsButton;
            gameMenu.OnExitButton += OnExitButton;

            var inputManager = InputManager.Instance;
            inputManager.LaunchResetControl.Performed += OnLaunchResetButton;
            inputManager.ViewControl.Performed += OnViewButton;
            inputManager.OnEscapeButton += OnEscapeButton;

            leaderboard = Leaderboard.Instance;
        }

        void Start()
        {
            flyingWing.Battery.InfiniteCapacity = PlayerPrefs.GetInt( infiniteBatteryKey, 0 ) > 0;
            flyingWing.Transceiver.InfiniteRange = PlayerPrefs.GetInt( infiniteRangeKey, 0 ) > 0;

            spawnPosition = wingLauncher.transform.position;
            spawnRotation = wingLauncher.transform.rotation;

            pilotCamera.SetActive( true );
            fpvCamera.SetActive( false );

            osdTelemetry.Hide();

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
                
            } );

            lapTime.Init( PlayerPrefs.GetFloat( localBestLapKey, 0f ) );
            lapTime.OnNewBestTime += newBestTime =>
            {
                PlayerPrefs.SetFloat( localBestLapKey, newBestTime );

                var pilotName = PlayerPrefs.GetString( "Nickname", "" );
                if( !string.IsNullOrEmpty( pilotName ) )
                {
                    leaderboard.AddRecord( dreamloPrivateCode, pilotName, "Mini Race Wing", newBestTime, null );
                }
                
                //ghostReplay.StopReplaying();
                ghostReplay.SaveReplay();
                
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

                    osdTelemetry.Hide();

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
                wingLauncher.Launch();
                lastLaunchTime = Time.time;
            }

            // Reset
            else
            {
                blackScreen.StartToBlackScreenAnimation( () =>
                {
                    flyingWing.Reset( spawnPosition, spawnRotation );

                    osdTelemetry.Reset();

                    lapTime.Reset();
                    lapTime.Hide();

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
                osdTelemetry.Hide();
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
    }
}
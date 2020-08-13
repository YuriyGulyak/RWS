using RWS;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleplayerGameManager : MonoBehaviour
{
    [SerializeField]
    GameObject pilotCamera = null;
    
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
    OSDHome osdHome = null;
    
    [SerializeField]
    GameMenu gameMenu = null;

    [SerializeField]
    BlackScreen blackScreen = null;
    
    [SerializeField]
    SettingsPanel settingsPanel = null;
    
    //----------------------------------------------------------------------------------------------------

    readonly string bestLapKey = "BestLap";
    
    bool gameStarted;
    
    
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
        osdTelemetry.Hide();
        osdHome.Hide();

        lapTime.Init( PlayerPrefs.GetFloat( bestLapKey, 0f ) );
        lapTime.OnNewBestTime += newBestTime =>
        {
            PlayerPrefs.SetFloat( bestLapKey, newBestTime );
        };
        lapTime.Hide();

        raceTrack.OnStart.AddListener( _ => lapTime.StartNewTime() );
        raceTrack.OnFinish.AddListener( _ => lapTime.CompareTime() );
        
        settingsPanel.Hide();
        gameMenu.Show();

        gameMenu.OnStartButton += OnStartButton;
        gameMenu.OnResumeButton += OnResumeButton;
        gameMenu.OnSettingsButton += OnSettingsButton;
        gameMenu.OnExitButton += OnExitButton;
        
        InputManager.Instance.OnEscapeButton += OnEscapeButton;
    }

    
    void OnSceneLoaded( Scene scene, LoadSceneMode mode )
    {
        BlackScreen.Instance.StartFromBlackScreenAnimation();
    }

    void OnStartButton()
    {
        gameMenu.Hide();
        
        blackScreen.StartToBlackScreenAnimation( () =>
        {
            pilotCamera.SetActive( false );
            fpvCamera.SetActive( true );

            osdTelemetry.Show();
            osdHome.Show();

            var wingRigibody = flyingWing.Rigidbody;
            wingRigibody.position = wingLauncher.transform.position;
            wingRigibody.rotation = wingLauncher.transform.rotation;

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
        gameMenu.Hide();
        settingsPanel.Hide();
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
            SceneManager.LoadSceneAsync( 0 );
        } );
    }

    void OnLaunchButton()
    {
        wingLauncher.Launch();
    }

    void OnResetButton()
    {
        blackScreen.StartToBlackScreenAnimation( () =>
        {
            var wingRigibody = flyingWing.Rigidbody;
            wingRigibody.isKinematic = true;
            wingRigibody.position = wingLauncher.transform.position;
            wingRigibody.rotation = wingLauncher.transform.rotation;

            flyingWing.Reset();

            osdTelemetry.Reset();
            osdHome.Reset();
        
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
            gameMenu.Show();
            Cursor.visible = true;
        }
    }
}
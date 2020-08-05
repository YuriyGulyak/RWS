using RWS;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleplayerGameManager : MonoBehaviour
{
    [SerializeField]
    GameObject orbitCamera = null;

    [SerializeField]
    GameObject fpvCamera = null;

    [SerializeField]
    WingLauncher wingLauncher = null;
    
    [SerializeField]
    RaceTrack raceTrack = null;
    
    [SerializeField]
    LapTime lapTime = null;

    [SerializeField]
    GameMenu gameMenu = null;

    [SerializeField]
    SettingsPanel settingsPanel = null;
    
    //----------------------------------------------------------------------------------------------------

    readonly string bestLapKey = "BestLap";
    
    bool gameStarted;

    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnSceneLoaded( Scene scene, LoadSceneMode mode )
    {
        BlackScreen.Instance.StartFromBlackScreenAnimation();
    }
    
    void Awake()
    {
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


    void OnStartButton()
    {
        gameMenu.Hide();
        
        orbitCamera.SetActive( false );
        fpvCamera.SetActive( true );

        var inputManager = InputManager.Instance;
        inputManager.LaunchControl.Performed += OnLaunchButton;
        inputManager.ResetControl.Performed += OnResetButton;

        Cursor.visible = false;
        
        gameStarted = true;
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
        var wing = FindObjectOfType<FlyingWing>();
        var wingRigibody = wing.GetComponentInChildren<Rigidbody>();

        wingRigibody.isKinematic = true;
        wingRigibody.position = wingLauncher.transform.position;
        wingRigibody.rotation = wingLauncher.transform.rotation;

        wingLauncher.Reset();

        lapTime.Reset();
        lapTime.Hide();
    }
    
    void OnEscapeButton()
    {
        if( !gameStarted )
        {
            return;
        }
        
        if( !gameMenu.IsOpen )
        {
            gameMenu.Show();
            Cursor.visible = true;
        }
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DemoGameManager : MonoBehaviour
{
    [SerializeField]
    GameObject pressSpacebarText = null;

    [SerializeField]
    GameObject orbitCamera = null;

    [SerializeField]
    GameObject fpvCamera = null;

    [SerializeField]
    GameObject inputInfo = null;

    [SerializeField]
    WingLauncher wingLauncher = null;
    
    [SerializeField]
    RaceTrack raceTrack = null;
    
    [SerializeField]
    LapTimer lapTimer = null;

    //----------------------------------------------------------------------------------------------------

    readonly string bestLapKey = "BestLap";
    
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnsceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnsceneLoaded;
    }

    void OnsceneLoaded( Scene scene, LoadSceneMode mode )
    {
        BlackScreen.Instance.StartFromBlackScreenAnimation();
    }
    
    void Awake()
    {
        //print( PlayerPrefs.HasKey( bestLapKey ) );
        
        if( PlayerPrefs.HasKey( bestLapKey ) )
        {
            lapTimer.Init( PlayerPrefs.GetFloat( bestLapKey ) );
        }
        lapTimer.OnNewBestTime += newBestTime =>
        {
            PlayerPrefs.SetFloat( bestLapKey, newBestTime );
        };

        raceTrack.OnStart.AddListener( _ => lapTimer.StartNewTime() );
        raceTrack.OnFinish.AddListener( _ => lapTimer.CompareTime() );
    }

    IEnumerator Start()
    {
        yield return new WaitUntil( () => Keyboard.current.spaceKey.wasPressedThisFrame );
                        
        var playerInput = PlayerInputWrapper.Instance;
        
        playerInput.Launch.AddListener( wingLauncher.Launch );
        playerInput.Restart.AddListener( RestartGame );
        
        StartGame();
    }
    

    void StartGame()
    {
        pressSpacebarText.SetActive( false );
        inputInfo.SetActive( false );
        
        orbitCamera.SetActive( false );
        fpvCamera.SetActive( true );
    }

    void RestartGame()
    {
        var wing = FindObjectOfType<FlyingWing>();
        var wingRigibody = wing.GetComponentInChildren<Rigidbody>();

        wingRigibody.isKinematic = true;
        wingRigibody.position = wingLauncher.transform.position;
        wingRigibody.rotation = wingLauncher.transform.rotation;

        wingLauncher.Reset();

        lapTimer.ResetAndHide();
    }
}
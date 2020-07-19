using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SingleplayerGameManager : MonoBehaviour
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
    LapTime lapTime = null;

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
        lapTime.Init( PlayerPrefs.HasKey( bestLapKey ) ? PlayerPrefs.GetFloat( bestLapKey ) : 0f );
        lapTime.OnNewBestTime += newBestTime =>
        {
            PlayerPrefs.SetFloat( bestLapKey, newBestTime );
        };
        lapTime.Hide();

        raceTrack.OnStart.AddListener( _ => lapTime.StartNewTime() );
        raceTrack.OnFinish.AddListener( _ => lapTime.CompareTime() );
    }

    IEnumerator Start()
    {
        yield return new WaitUntil( () => Keyboard.current.spaceKey.wasPressedThisFrame );
                 
        pressSpacebarText.SetActive( false );
        inputInfo.SetActive( false );
        
        orbitCamera.SetActive( false );
        fpvCamera.SetActive( true );

        var playerInput = PlayerInputWrapper.Instance;
        playerInput.Launch.AddListener( wingLauncher.Launch );
        playerInput.Restart.AddListener( RestartGame );
    }
    

    void RestartGame()
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
}
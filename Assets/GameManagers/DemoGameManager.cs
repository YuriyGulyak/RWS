using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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

    void Awake()
    {
        raceTrack.OnStart.AddListener( _ => lapTimer.StartNewTime() );
        raceTrack.OnFinish.AddListener( _ => lapTimer.CompareTime() );
        
        BlackScreen.Instance.StartFromBlackScreenAnimation();
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
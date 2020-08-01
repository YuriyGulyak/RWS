using System;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    Button startButton = null;
    
    [SerializeField]
    Button resumeButton = null;
    
    [SerializeField]
    Button settingsButton = null;
    
    [SerializeField]
    Button exitButton = null;

    //----------------------------------------------------------------------------------------------------

    public Action OnStartButton;
    public Action OnResumeButton;
    public Action OnSettingsButton;
    public Action OnExitButton;
    
    public bool IsOpen => gameObject.activeSelf;
    
    public void Show()
    {
        if( IsOpen )
        {
            return;
        }
        gameObject.SetActive( true );
    }

    public void Hide()
    {
        if( !IsOpen )
        {
            return;
        }
        gameObject.SetActive( false );
    }

    //----------------------------------------------------------------------------------------------------
    
    void Awake()
    {
        startButton.gameObject.SetActive( true );
        resumeButton.gameObject.SetActive( false );
        
        startButton.onClick.AddListener( () =>
        {
            startButton.gameObject.SetActive( false );
            resumeButton.gameObject.SetActive( true );
            OnStartButton?.Invoke();
        } );
        
        resumeButton.onClick.AddListener( () => OnResumeButton?.Invoke() );
        settingsButton.onClick.AddListener( () => OnSettingsButton?.Invoke() );
        exitButton.onClick.AddListener( () => OnExitButton?.Invoke() );
    }
}
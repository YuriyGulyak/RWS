using System;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    Button resumeButton = null;
    
    [SerializeField]
    Button settingsButton = null;
    
    [SerializeField]
    Button exitButton = null;

    //----------------------------------------------------------------------------------------------------

    public Action OnResumeButton;
    public Action OnSettingsButton;
    public Action OnExitButton;
    
    public bool IsActive => gameObject.activeSelf;
    
    public void Show()
    {
        if( IsActive )
        {
            return;
        }
        gameObject.SetActive( true );
    }

    public void Hide()
    {
        if( !IsActive )
        {
            return;
        }
        gameObject.SetActive( false );
    }

    //----------------------------------------------------------------------------------------------------
    
    void Awake()
    {
        resumeButton.onClick.AddListener( () => OnResumeButton?.Invoke() );
        settingsButton.onClick.AddListener( () => OnSettingsButton?.Invoke() );
        exitButton.onClick.AddListener( () => OnExitButton?.Invoke() );
    }
}
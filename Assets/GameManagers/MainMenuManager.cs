using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    Button singleplayerButton = null;
    
    [SerializeField]
    Button multiplayerButton = null;
    
    [SerializeField]
    Button settingsButton = null;
    
    [SerializeField]
    Button exitButton = null;
    
    [SerializeField]
    MultiplayerLobby multiplayerLobby = null;
    
    //----------------------------------------------------------------------------------------------------
    
    void Awake()
    {
        singleplayerButton.onClick.AddListener( OnSingleplayerButton );
        multiplayerButton.onClick.AddListener( OnMultiplayerButton );
        settingsButton.onClick.AddListener( OnSettingsButton );
        exitButton.onClick.AddListener( OnExitButton );

        multiplayerLobby.Hide();
    }


    void OnSingleplayerButton()
    {
    }
    
    void OnMultiplayerButton()
    {
        multiplayerLobby.Show();
    }
    
    void OnSettingsButton()
    {
    }
    
    void OnExitButton()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}

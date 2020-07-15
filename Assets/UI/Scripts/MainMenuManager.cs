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
    
    [SerializeField]
    SingleplayerPanel singleplayerPanel = null;
    
    //----------------------------------------------------------------------------------------------------
    
    void Awake()
    {
        singleplayerButton.onClick.AddListener( OnSingleplayerButton );
        multiplayerButton.onClick.AddListener( OnMultiplayerButton );
        settingsButton.onClick.AddListener( OnSettingsButton );
        exitButton.onClick.AddListener( OnExitButton );

        singleplayerPanel.Hide();
        multiplayerLobby.Hide();
    }


    void OnSingleplayerButton()
    {
        multiplayerLobby.Hide();
        singleplayerPanel.Show();
    }
    
    void OnMultiplayerButton()
    {
        singleplayerPanel.Hide();
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

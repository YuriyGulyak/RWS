using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
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
    MultiplayerPanel multiplayerPanel = null;
    
    [SerializeField]
    SingleplayerPanel singleplayerPanel = null;

    [SerializeField]
    SettingsPanel settingsPanel = null;
    
    //----------------------------------------------------------------------------------------------------
    
    void Awake()
    {
        singleplayerButton.onClick.AddListener( OnSingleplayerButton );
        multiplayerButton.onClick.AddListener( OnMultiplayerButton );
        settingsButton.onClick.AddListener( OnSettingsButton );
        exitButton.onClick.AddListener( OnExitButton );

        singleplayerPanel.Hide();
        multiplayerPanel.Hide();
        settingsPanel.Hide();
    }


    void OnSingleplayerButton()
    {
        multiplayerPanel.Hide();
        settingsPanel.Hide();
        singleplayerPanel.Show();
    }
    
    void OnMultiplayerButton()
    {
        singleplayerPanel.Hide();
        settingsPanel.Hide();
        multiplayerPanel.Show();
    }
    
    void OnSettingsButton()
    {
        singleplayerPanel.Hide();
        multiplayerPanel.Hide();
        settingsPanel.Show();
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

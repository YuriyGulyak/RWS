using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Button singleplayerButton = null;

    [SerializeField]
    Button multiplayerButton = null;

    [SerializeField]
    Button leaderboardButton = null;
    
    [SerializeField]
    Button profileButton = null;

    [SerializeField]
    Button settingsButton = null;

    [SerializeField]
    Button exitButton = null;


    [SerializeField]
    SingleplayerPanel singleplayerPanel = null;

    [SerializeField]
    MultiplayerPanel multiplayerPanel = null;

    [SerializeField]
    LeaderboardPanel leaderboardPanel = null;
    
    [SerializeField]
    PlayerProfilePanel profilePanel = null;

    [SerializeField]
    SettingsPanel settingsPanel = null;

    //----------------------------------------------------------------------------------------------------

    void Awake()
    {
        singleplayerButton.onClick.AddListener( OnSingleplayerButton );
        multiplayerButton.onClick.AddListener( OnMultiplayerButton );
        leaderboardButton.onClick.AddListener( OnLeaderboardButton );
        profileButton.onClick.AddListener( OnProfileButton );
        settingsButton.onClick.AddListener( OnSettingsButton );
        exitButton.onClick.AddListener( OnExitButton );

        singleplayerPanel.Hide();
        multiplayerPanel.Hide();
        leaderboardPanel.Hide();
        profilePanel.Hide();
        settingsPanel.Hide();
    }


    void OnSingleplayerButton()
    {
        singleplayerPanel.Show();
        multiplayerPanel.Hide();
        leaderboardPanel.Hide();
        profilePanel.Hide();
        settingsPanel.Hide();
    }

    void OnMultiplayerButton()
    {
        multiplayerPanel.Show();
        singleplayerPanel.Hide();
        leaderboardPanel.Hide();
        profilePanel.Hide();
        settingsPanel.Hide();
    }

    void OnLeaderboardButton()
    {
        singleplayerPanel.Hide();
        multiplayerPanel.Hide();
        leaderboardPanel.Show();
        profilePanel.Hide();
        settingsPanel.Hide();
    }

    void OnProfileButton()
    {
        singleplayerPanel.Hide();
        multiplayerPanel.Hide();
        leaderboardPanel.Hide();
        profilePanel.Show();
        settingsPanel.Hide();
    }
    
    void OnSettingsButton()
    {
        singleplayerPanel.Hide();
        multiplayerPanel.Hide();
        leaderboardPanel.Hide();
        profilePanel.Hide();
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
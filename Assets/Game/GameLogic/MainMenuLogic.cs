using UnityEngine;

namespace RWS
{
    public class MainMenuLogic : MonoBehaviour
    {
        [SerializeField]
        MainMenu mainMenu = null;
    
        [SerializeField]
        SingleplayerPanel singleplayerPanel = null;

        [SerializeField]
        MultiplayerPanel multiplayerPanel = null;

        [SerializeField]
        CraftPanel craftPanel = null;
    
        [SerializeField]
        LeaderboardPanel leaderboardPanel = null;
    
        [SerializeField]
        PlayerProfilePanel profilePanel = null;

        [SerializeField]
        SettingsPanel settingsPanel = null;


        void Awake()
        {
            mainMenu.OnSingleplayerButton.AddListener( OnSingleplayerButton );
            mainMenu.OnMultiplayerButton.AddListener( OnMultiplayerButton );
            mainMenu.OnCraftButton.AddListener( OnCraftButton );
            mainMenu.OnLeaderboardButton.AddListener( OnLeaderboardButton );
            mainMenu.OnProfileButton.AddListener( OnProfileButton );
            mainMenu.OnSettingsButton.AddListener( OnSettingsButton );
            mainMenu.OnExitButton.AddListener( OnExitButton );

            HideAllPanels();
        }

        void Start()
        {
            if( !PlayerPrefs.HasKey( "Nickname" ) )
            {
                profilePanel.Show();
            }
        }


        void OnSingleplayerButton()
        {
            if( singleplayerPanel.IsOpen )
            {
                return;
            }

            HideAllPanels();
            singleplayerPanel.Show();
        }

        void OnMultiplayerButton()
        {
            if( multiplayerPanel.IsOpen )
            {
                return;
            }
            
            HideAllPanels();
            multiplayerPanel.Show();
        }

        void OnCraftButton()
        {
            if( craftPanel.IsOpen )
            {
                return;
            }
            
            HideAllPanels();
            craftPanel.Show();
        }

        void OnLeaderboardButton()
        {
            if( leaderboardPanel.IsOpen )
            {
                return;
            }
            
            HideAllPanels();
            leaderboardPanel.Show();
        }

        void OnProfileButton()
        {
            if( profilePanel.IsOpen )
            {
                return;
            }
            
            HideAllPanels();
            profilePanel.Show();
        }
    
        void OnSettingsButton()
        {
            //if( settingsPanel.activeSelf )
            //{
            //    return;
            //}

            if( settingsPanel.IsOpen )
            {
                return;
            }
            
            HideAllPanels();
            //settingsPanel.SetActive( true );
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


        void HideAllPanels()
        {
            singleplayerPanel.Hide();
            multiplayerPanel.Hide();
            craftPanel.Hide();
            leaderboardPanel.Hide();
            profilePanel.Hide();
            settingsPanel.Hide();
            //settingsPanel.SetActive( false );
        }
    }
}
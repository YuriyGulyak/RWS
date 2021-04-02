using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RWS
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        Button singleplayerButton = null;

        [SerializeField]
        Button multiplayerButton = null;

        [SerializeField]
        Button craftButton = null;

        [SerializeField]
        Button leaderboardButton = null;

        [SerializeField]
        Button profileButton = null;

        [SerializeField]
        Button settingsButton = null;

        [SerializeField]
        Button exitButton = null;


        [HideInInspector]
        public UnityEvent OnSingleplayerButton;

        [HideInInspector]
        public UnityEvent OnMultiplayerButton;

        [HideInInspector]
        public UnityEvent OnCraftButton;

        [HideInInspector]
        public UnityEvent OnLeaderboardButton;

        [HideInInspector]
        public UnityEvent OnProfileButton;

        [HideInInspector]
        public UnityEvent OnSettingsButton;

        [HideInInspector]
        public UnityEvent OnExitButton;


        void Awake()
        {
            singleplayerButton.onClick.AddListener( OnSingleplayerButton.Invoke );
            multiplayerButton.onClick.AddListener( OnMultiplayerButton.Invoke );
            craftButton.onClick.AddListener( OnCraftButton.Invoke );
            leaderboardButton.onClick.AddListener( OnLeaderboardButton.Invoke );
            profileButton.onClick.AddListener( OnProfileButton.Invoke );
            settingsButton.onClick.AddListener( OnSettingsButton.Invoke );
            exitButton.onClick.AddListener( OnExitButton.Invoke );
            ;
        }
    }
}
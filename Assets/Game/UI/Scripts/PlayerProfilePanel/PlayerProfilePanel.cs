using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RWS
{
    public class PlayerProfilePanel : MonoBehaviour
    {
        [SerializeField]
        InputManager inputManager = null;
        
        [SerializeField]
        RectTransform panelRect = null;

        [SerializeField]
        Button closeButton = null;

        [SerializeField]
        TMP_InputField nameInputField = null;

        [SerializeField]
        TextMeshProUGUI infoText = null;

        [SerializeField]
        PlayerStatsPanel statsPanel = null;
        
        [SerializeField]
        Button applyButton = null;

        //----------------------------------------------------------------------------------------------------

        public void Initialize( PlayerProfile playerProfile )
        {
            this.playerProfile = playerProfile;
        }

        public bool IsOpen => gameObject.activeSelf;

        public void Show()
        {
            if( gameObject.activeSelf )
            {
                return;
            }

            gameObject.SetActive( true );
        }

        public void Hide()
        {
            if( !gameObject.activeSelf )
            {
                return;
            }

            gameObject.SetActive( false );
            panelRect.anchoredPosition = Vector2.zero;

            infoText.text = "";
            applyButton.gameObject.SetActive( false );
        }

        //----------------------------------------------------------------------------------------------------

        PlayerProfile playerProfile;
        

        void OnValidate()
        {
            if( !inputManager )
            {
                inputManager = InputManager.Instance;
            }
            if( !statsPanel )
            {
                statsPanel = GetComponentInChildren<PlayerStatsPanel>();
            }
        }

        void Awake()
        {
            closeButton.onClick.AddListener( Hide );

            nameInputField.onEndEdit.AddListener( OnNameInput );

            infoText.gameObject.SetActive( true );
            infoText.text = "";

            applyButton.onClick.AddListener( OnApplyButton );
            applyButton.gameObject.SetActive( false );
        }

        void OnEnable()
        {
            if( playerProfile != null )
            {
                RefreshData( playerProfile );
            }

            inputManager.OnEscapeButton += OnEscapeButton;
        }

        void OnDisable()
        {
            inputManager.OnEscapeButton -= OnEscapeButton;
        }


        void RefreshData( PlayerProfile player )
        {
            nameInputField.text = player.playerName;

            statsPanel.SetTotalFlightTime( player.totalFlightTime );
            statsPanel.SetTotalFlightDistance( player.totalFlightDistance );
            statsPanel.SetLongestFlightTime( player.longestFlightTime );
            statsPanel.SetTopSpeed( player.topSpeed * 3.6f );
            statsPanel.SetCompletedLaps( player.completedLaps );
            statsPanel.SetNumberOfLaunches( player.numberOfLaunches );
            statsPanel.SetNumberOfCrashes( player.numberOfCrashes );
            
            // statsPanel.SetTotalFlightTime( Random.Range( 0f, 6000f ) );
            // statsPanel.SetTotalFlightDistance( Random.Range( 0f, 6000f ) );
            // statsPanel.SetLongestFlightTime( Random.Range( 0f, 6000f ) );
            // statsPanel.SetTopSpeed( Random.Range( 0f, 200f ) );
            // statsPanel.SetCompletedLaps( Random.Range( 0, 200 ) );
            // statsPanel.SetNumberOfLaunches( Random.Range( 0, 200 ) );
            // statsPanel.SetNumberOfCrashes( Random.Range( 0, 200 ) );
        }

        void OnNameInput( string newName )
        {
            newName = newName.Trim();
            
            infoText.text = "";
            applyButton.gameObject.SetActive( false );
            
            if( newName.Equals( playerProfile.playerName ) )
            {
                return;
            }
            if( newName.Length < 4 )
            {
                infoText.text = "Name must be minimum 4 characters";
                nameInputField.ActivateInputField();
                return;
            }
            
            applyButton.gameObject.SetActive( true );
        }


        void OnApplyButton()
        {
            infoText.text = "Name changed";

            var newPlayerName = nameInputField.text.Trim();
            nameInputField.text = newPlayerName;

            playerProfile.playerName = newPlayerName;
            PlayerProfileDatabase.SavePlayerProfile( playerProfile );

            applyButton.gameObject.SetActive( false );
        }

        void OnEscapeButton()
        {
            if( nameInputField.isFocused )
            {
                return;
            }

            Hide();
        }
    }
}
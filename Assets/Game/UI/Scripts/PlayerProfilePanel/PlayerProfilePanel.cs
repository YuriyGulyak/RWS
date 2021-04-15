using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace RWS
{
    public class PlayerProfilePanel : MonoBehaviour
    {
        [SerializeField]
        RectTransform panelRect = null;

        [SerializeField]
        Button closeButton = null;

        [SerializeField]
        TMP_InputField nameInputField = null;

        [SerializeField]
        TextMeshProUGUI infoText = null;

        [SerializeField]
        Button applyButton = null;

        //----------------------------------------------------------------------------------------------------

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

        string pilotName;
        const string pilotNameKey = "Nickname";
        InputManager inputManager;
        

        void Awake()
        {
            closeButton.onClick.AddListener( Hide );

            nameInputField.onEndEdit.AddListener( OnNameInput );

            infoText.gameObject.SetActive( true );
            infoText.text = "";

            applyButton.onClick.AddListener( OnApplyButton );
            applyButton.gameObject.SetActive( false );

            if( PlayerPrefs.HasKey( pilotNameKey ) )
            {
                pilotName = PlayerPrefs.GetString( pilotNameKey );
            }
            else
            {
                pilotName = $"Pilot{Random.Range( 0, 10000 ):0000}";
                PlayerPrefs.SetString( pilotNameKey, pilotName );
            }

            inputManager = InputManager.Instance;
        }

        void OnEnable()
        {
            nameInputField.text = pilotName;
            
            inputManager.OnEscapeButton += OnEscapeButton;
        }

        void OnDisable()
        {
            inputManager.OnEscapeButton -= OnEscapeButton;
        }


        void OnNameInput( string newName )
        {
            newName = newName.Trim();

            if( newName.Equals( pilotName ) )
            {
                return;
            }

            if( newName.Length < 4 )
            {
                infoText.text = "Name must be minimum 4 characters";
                nameInputField.ActivateInputField();
            }
            else
            {
                infoText.text = "";
                applyButton.gameObject.SetActive( true );
            }
        }

        void OnApplyButton()
        {
            infoText.text = "Name changed";

            pilotName = nameInputField.text.Trim();
            nameInputField.text = pilotName;

            PlayerPrefs.SetString( pilotNameKey, pilotName );

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
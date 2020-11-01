using RWS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfilePanel : MonoBehaviour
{
    [SerializeField]
    RectTransform panelRect;

    [SerializeField]
    Button closeButton;

    [SerializeField]
    TMP_InputField nameInputField;

    [SerializeField]
    TextMeshProUGUI infoText;

    [SerializeField]
    Button applyButton;

    //----------------------------------------------------------------------------------------------------

    public void Show()
    {
        if( gameObject.activeSelf )
        {
            return;
        }

        gameObject.SetActive( true );

        //
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


    void Awake()
    {
        closeButton.onClick.AddListener( Hide );

        InputManager.Instance.OnEscapeButton += OnEscapeButton;

        nameInputField.onEndEdit.AddListener( OnNameInputFieldChanged );

        infoText.gameObject.SetActive( true );
        infoText.text = "";

        applyButton.onClick.AddListener( OnApplyButton );
        applyButton.gameObject.SetActive( false );

        if( PlayerPrefs.HasKey( "Nickname" ) )
        {
            pilotName = PlayerPrefs.GetString( "Nickname" );
        }
        else
        {
            pilotName = $"Pilot{UnityEngine.Random.Range( 0, 10000 ):0000}";
        }
    }

    void OnEnable()
    {
        nameInputField.text = pilotName;
    }


    void OnNameInputFieldChanged( string newName )
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

        PlayerPrefs.SetString( "Nickname", pilotName );

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
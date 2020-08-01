using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] 
    TMP_InputField nameInputField = null;

    [SerializeField] 
    Button loginButton = null;

    [SerializeField] 
    TextMeshProUGUI errorText = null;
    
    //----------------------------------------------------------------------------------------------------

    public string Nickname => nickname;
    
    public void Show( Action onLoginCallback = null )
    {
        this.onLoginCallback = onLoginCallback;
        gameObject.SetActive( true );
    }

    public void Hide()
    {
        gameObject.SetActive( false );
        onLoginCallback = null;
    }
    
    //----------------------------------------------------------------------------------------------------

    Action onLoginCallback;
    string nickname;
    
    
    void Awake()
    {
        loginButton.onClick.AddListener( OnLoginButton );
    }

    void OnEnable()
    {
        if( PlayerPrefs.HasKey( "Nickname" ) )
        {
            nickname = PlayerPrefs.GetString( "Nickname" );
        }
        else
        {
            nickname = $"Player{UnityEngine.Random.Range( 0, 10000 ):0000}";
        }
        
        nameInputField.text = nickname;
    }


    void OnLoginButton()
    {
        if( nameInputField.text.Length < 4 )
        {
            errorText.gameObject.SetActive( true );
            errorText.text = "Error: Nickname must be minimum 4 characters";
        }
        else
        {
            errorText.gameObject.SetActive( false );
            nickname = nameInputField.text.Trim();
            PlayerPrefs.SetString( "Nickname", nickname );
            onLoginCallback?.Invoke();
        }
    }
}
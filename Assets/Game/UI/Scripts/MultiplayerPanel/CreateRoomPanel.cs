using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using RWS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomPanel : MonoBehaviour
{
    [SerializeField]
    TMP_InputField roomNameInputField = null;

    //[SerializeField]
    //Toggle roomIsOpenToggle = null;
    
    //[SerializeField]
    //TMP_InputField passwordInputField = null;
    
    [SerializeField]
    TMP_InputField maxPlayersInputField = null;
    
    [SerializeField]
    Toggle infiniteBatteryToggle = null; 
    
    [SerializeField]
    Toggle infiniteRangeToggle = null;
    
    [SerializeField]
    Button createRoomButton = null;
    
    [SerializeField]
    Button cancelButton = null;

    [SerializeField]
    int minPlayers = 2;
    
    [SerializeField]
    int maxPlayers = 8;
    
    //----------------------------------------------------------------------------------------------------

    public void Show( Action onCancelCallback = null )
    {
        this.onCancelCallback = onCancelCallback;
        gameObject.SetActive( true );
        
        InputManager.Instance.OnEscapeButton += OnEscapeButton;
    }

    public void Hide()
    {
        gameObject.SetActive( false );
        onCancelCallback = null;
        
        InputManager.Instance.OnEscapeButton -= OnEscapeButton;
    }

    //----------------------------------------------------------------------------------------------------
    
    Action onCancelCallback;
    
    
    void Awake()
    {
        maxPlayersInputField.onEndEdit.AddListener( inputFieldValue =>
        {
            var inputValue = int.Parse( maxPlayersInputField.text );
            inputValue = Mathf.Clamp( inputValue, minPlayers, maxPlayers );
            maxPlayersInputField.text = inputValue.ToString();
        } );
        
        //infiniteBatteryToggle.

        createRoomButton.onClick.AddListener( OnCreateRoomButton );
        cancelButton.onClick.AddListener( OnCancelButton );
    }

    void OnEnable()
    {
        roomNameInputField.text = $"Room_{UnityEngine.Random.Range( 0, 10000 ):0000}";
    }


    void OnCreateRoomButton()
    {
        var roomName = roomNameInputField.text;
        var maxPlayersInputValue = (byte)Mathf.Clamp( int.Parse( maxPlayersInputField.text ), minPlayers, maxPlayers );

        var roomOptions = new RoomOptions
        {
            IsVisible = true, 
            IsOpen = true, 
            MaxPlayers = maxPlayersInputValue, 
            
            CustomRoomProperties = new Hashtable
            {
                { "InfiniteBattery", infiniteBatteryToggle.isOn }, 
                { "InfiniteRange", infiniteRangeToggle.isOn }
            }
        };

        PhotonNetwork.CreateRoom( roomName, roomOptions );
    }
    
    void OnCancelButton()
    {
        onCancelCallback?.Invoke();
    }

    void OnEscapeButton()
    {
        if( roomNameInputField.isFocused || maxPlayersInputField.isFocused )
        {
            return;
        }
        OnCancelButton();
    }
}

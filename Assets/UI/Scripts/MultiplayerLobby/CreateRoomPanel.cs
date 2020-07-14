using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomPanel : MonoBehaviour
{
    [SerializeField]
    TMP_InputField roomNameInputField = null;

    [SerializeField]
    Toggle roomIsOpenToggle = null;
    
    [SerializeField]
    TMP_InputField passwordInputField = null;
    
    [SerializeField]
    TMP_InputField maxPlayersInputField = null;
    
    [SerializeField]
    Button createRoomButton = null;
    
    [SerializeField]
    Button cancelButton = null;
    
    //----------------------------------------------------------------------------------------------------

    public void Show( Action onCancelCallback = null )
    {
        this.onCancelCallback = onCancelCallback;
        gameObject.SetActive( true );
    }

    public void Hide()
    {
        gameObject.SetActive( false );
        onCancelCallback = null;
    }

    //----------------------------------------------------------------------------------------------------
    
    Action onCancelCallback;
    
    
    void Awake()
    {
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
        
        if( !byte.TryParse( maxPlayersInputField.text, out var maxPlayers ) )
        {
            maxPlayers = 8;
        }
        if( maxPlayers < 2 )
        {
            maxPlayers = 2;
        }
        else if( maxPlayers > 12 )
        {
            maxPlayers = 12;
        }

        var roomOptions = new RoomOptions
        {
            IsVisible = true,
            IsOpen = roomIsOpenToggle.isOn,
            MaxPlayers = maxPlayers
        };

        PhotonNetwork.CreateRoom( roomName, roomOptions );
    }
    
    void OnCancelButton()
    {
        onCancelCallback?.Invoke();
    }
}

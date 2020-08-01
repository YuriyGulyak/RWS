using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerPanel : MonoBehaviourPunCallbacks
{
    [SerializeField]
    RectTransform panelRect = null;
    
    [SerializeField]
    Button closeButton = null;
    
    [SerializeField]
    LoginPanel loginPanel = null;
    
    [SerializeField]
    TextMeshProUGUI stateText = null;
   
    [SerializeField]
    Button createRoomButton = null;
    
    [SerializeField]
    Button showRoomListButton = null;
    
    [SerializeField]
    Button joinRandomRoomButton = null;
    
    [SerializeField]
    TextMeshProUGUI errorText = null;
    
    [SerializeField]
    CreateRoomPanel createRoomPanel = null;

    [SerializeField]
    InsideRoomPanel insideRoomPanel = null;

    [SerializeField]
    RoomListPanel roomListPanel = null;
    
    //----------------------------------------------------------------------------------------------------

    public void Show()
    {
        if( gameObject.activeSelf )
        {
            return;
        }
        gameObject.SetActive( true );

        loginPanel.Show( () =>
        {
            loginPanel.Hide();
            stateText.gameObject.SetActive( true );
            PhotonNetwork.NickName = loginPanel.Nickname;
            PhotonNetwork.ConnectUsingSettings();
        } );
    }

    public void Hide()
    {
        if( !gameObject.activeSelf )
        {
            return;
        }

        loginPanel.Hide();
        createRoomPanel.Hide();
        insideRoomPanel.Hide();
        roomListPanel.Hide();

        errorText.gameObject.SetActive( false );
        stateText.gameObject.SetActive( false );
        
        createRoomButton.gameObject.SetActive( false );
        showRoomListButton.gameObject.SetActive( false );
        joinRandomRoomButton.gameObject.SetActive( false );
        
        PhotonNetwork.Disconnect();
        
        gameObject.SetActive( false );
        panelRect.anchoredPosition = Vector2.zero;
    }

    //----------------------------------------------------------------------------------------------------

    // PUN Callbacks
    
    public override void OnConnectedToMaster()
    {
        stateText.gameObject.SetActive( false );
        
        createRoomButton.gameObject.SetActive( true );
        showRoomListButton.gameObject.SetActive( true );
        joinRandomRoomButton.gameObject.SetActive( true );
    }

    public override void OnDisconnected( DisconnectCause cause )
    {
    }

    public override void OnJoinedRoom()
    {
        //print( "OnJoinedRoom" );

        createRoomButton.gameObject.SetActive( false );
        showRoomListButton.gameObject.SetActive( false );
        joinRandomRoomButton.gameObject.SetActive( false );
        
        createRoomPanel.Hide();
        roomListPanel.Hide();
        
        insideRoomPanel.Show( () =>
        {
            insideRoomPanel.Hide();
            
            createRoomButton.gameObject.SetActive( true );
            showRoomListButton.gameObject.SetActive( true );
            joinRandomRoomButton.gameObject.SetActive( true );
        } );
        
        //leaveRoomButton.gameObject.SetActive( true );
    }

    public override void OnJoinRandomFailed( short returnCode, string message )
    {
        errorText.gameObject.SetActive( true );
        errorText.text = $"Error: {message}";
    }

    public override void OnCreatedRoom()
    {
        //print( "OnCreatedRoom" );
    }
    
    public override void OnCreateRoomFailed( short returnCode, string message )
    {
        createRoomPanel.Hide();

        errorText.gameObject.SetActive( true );
        errorText.text = $"Error: {message}";
    }

    //----------------------------------------------------------------------------------------------------
    
    ClientState prevState;

    
    void Awake()
    {
        closeButton.onClick.AddListener( Hide );
        
        createRoomButton.onClick.AddListener( OnCreateRoomButton );
        showRoomListButton.onClick.AddListener( OnShowRoomListButton );
        joinRandomRoomButton.onClick.AddListener( OnJoinRandomRoomButton );

        createRoomButton.gameObject.SetActive( false );
        showRoomListButton.gameObject.SetActive( false );
        joinRandomRoomButton.gameObject.SetActive( false );
        
        errorText.gameObject.SetActive( false );
        stateText.gameObject.SetActive( false );
        
        createRoomPanel.Hide();
        insideRoomPanel.Hide();
        roomListPanel.Hide();

        //PhotonNetwork.GameVersion = "";
        PhotonNetwork.AutomaticallySyncScene = true;

        loginPanel.Show( () =>
        {
            loginPanel.Hide();
            stateText.gameObject.SetActive( true );
            PhotonNetwork.NickName = loginPanel.Nickname;
            PhotonNetwork.ConnectUsingSettings();
        } );
    }

    void Update()
    {
        var curState = PhotonNetwork.NetworkClientState;
        if( curState != prevState )
        {
            stateText.text = curState.ToString();
        }
        prevState = curState;
    }


    // UI Callbacks
    
    void OnCreateRoomButton()
    {
        errorText.gameObject.SetActive( false );
        
        createRoomButton.gameObject.SetActive( false );
        showRoomListButton.gameObject.SetActive( false );
        joinRandomRoomButton.gameObject.SetActive( false );
        
        createRoomPanel.Show( () =>
        {
            createRoomPanel.Hide();
            createRoomButton.gameObject.SetActive( true );
            showRoomListButton.gameObject.SetActive( true );
            joinRandomRoomButton.gameObject.SetActive( true );
        } );
    }
    
    void OnShowRoomListButton()
    {
        errorText.gameObject.SetActive( false );
        
        createRoomButton.gameObject.SetActive( false );
        showRoomListButton.gameObject.SetActive( false );
        joinRandomRoomButton.gameObject.SetActive( false );

        void onBackButton()
        {
            roomListPanel.Hide();
            createRoomButton.gameObject.SetActive( true );
            showRoomListButton.gameObject.SetActive( true );
            joinRandomRoomButton.gameObject.SetActive( true );
        }
        void onJoinButton( RoomInfo roomInfo )
        {
            PhotonNetwork.JoinRoom( roomInfo.Name );
        }
        roomListPanel.Show( onBackButton, onJoinButton );
    }

    void OnJoinRandomRoomButton()
    {
        PhotonNetwork.JoinRandomRoom();
    }
}

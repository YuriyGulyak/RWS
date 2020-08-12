using Photon.Pun;
using Photon.Realtime;
using RWS;
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
    Button logoutButton = null;
    
    [SerializeField]
    TextMeshProUGUI errorText = null;
    
    [SerializeField]
    CreateRoomPanel createRoomPanel = null;

    [SerializeField]
    InsideRoomPanel insideRoomPanel = null;

    [SerializeField]
    RoomListPanel roomListPanel = null;

    [SerializeField]
    RoomChat roomChat = null;
    
    //----------------------------------------------------------------------------------------------------

    public void Show()
    {
        if( gameObject.activeSelf )
        {
            return;
        }
        gameObject.SetActive( true );

        if( PhotonNetwork.IsConnected )
        {
            ShowNavigationButtons();
        }
        else
        {
            loginPanel.Show( () =>
            {
                loginPanel.Hide();
                stateText.gameObject.SetActive( true );
                PhotonNetwork.NickName = loginPanel.Nickname;
                PhotonNetwork.ConnectUsingSettings();
            } );
        }
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

        roomChat.Hide();
        
        errorText.gameObject.SetActive( false );
        stateText.gameObject.SetActive( false );
        
        HideNavigationButtons();

        if( PhotonNetwork.InRoom )
        {
            PhotonNetwork.LeaveRoom();
        }
        //PhotonNetwork.Disconnect();
        
        gameObject.SetActive( false );
        panelRect.anchoredPosition = Vector2.zero;
    }

    //----------------------------------------------------------------------------------------------------

    // PUN Callbacks
    
    public override void OnConnectedToMaster()
    {
        stateText.gameObject.SetActive( false );
        
        ShowNavigationButtons();
    }

    public override void OnDisconnected( DisconnectCause cause )
    {
        print( "OnDisconnected: " + cause );
        
        loginPanel.Show( () =>
        {
            loginPanel.Hide();
            stateText.gameObject.SetActive( true );
            PhotonNetwork.NickName = loginPanel.Nickname;
            PhotonNetwork.ConnectUsingSettings();
        } );
    }

    public override void OnLeftLobby()
    {
        print( "OnLeftLobby" );
    }

    public override void OnJoinedRoom()
    {
        //print( "OnJoinedRoom" );

        HideNavigationButtons();
        createRoomPanel.Hide();
        roomListPanel.Hide();
        
        insideRoomPanel.Show( () =>
        {
            insideRoomPanel.Hide();
            ShowNavigationButtons();
        } );

        roomChat.Show();
        
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

    public override void OnLeftRoom()
    {
        roomChat.Hide();
    }
    
    //----------------------------------------------------------------------------------------------------
    
    ClientState prevState;

    
    void Awake()
    {
        closeButton.onClick.AddListener( Hide );
        
        createRoomButton.onClick.AddListener( OnCreateRoomButton );
        showRoomListButton.onClick.AddListener( OnShowRoomListButton );
        joinRandomRoomButton.onClick.AddListener( OnJoinRandomRoomButton );
        logoutButton.onClick.AddListener( OnLogoutButton );
        
        HideNavigationButtons();
        
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
        
        HideNavigationButtons();
        
        createRoomPanel.Show( () =>
        {
            createRoomPanel.Hide();
            ShowNavigationButtons();
            
        } );
    }
    
    void OnShowRoomListButton()
    {
        errorText.gameObject.SetActive( false );
        
        HideNavigationButtons();

        void onBackButton()
        {
            roomListPanel.Hide();
            ShowNavigationButtons();
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
    
    void OnLogoutButton()
    {
        HideNavigationButtons();
        
        errorText.gameObject.SetActive( false );
        stateText.gameObject.SetActive( false );
        
        PhotonNetwork.Disconnect();
    }


    void ShowNavigationButtons()
    {
        createRoomButton.gameObject.SetActive( true );
        showRoomListButton.gameObject.SetActive( true );
        joinRandomRoomButton.gameObject.SetActive( true );
        logoutButton.gameObject.SetActive( true );
    }
    
    void HideNavigationButtons()
    {
        createRoomButton.gameObject.SetActive( false );
        showRoomListButton.gameObject.SetActive( false );
        joinRandomRoomButton.gameObject.SetActive( false );
        logoutButton.gameObject.SetActive( false );
    }
}

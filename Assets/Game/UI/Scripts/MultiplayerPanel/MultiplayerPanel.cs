using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RWS
{
    public class MultiplayerPanel : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        RectTransform panelRect = null;

        [SerializeField]
        Button closeButton = null;

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

        [SerializeField]
        RoomChat roomChat = null;

        [SerializeField]
        TextMeshProUGUI regionText = null;

        //----------------------------------------------------------------------------------------------------

        public bool IsOpen => gameObject.activeSelf;

        public void Show()
        {
            if( gameObject.activeSelf )
            {
                return;
            }

            gameObject.SetActive( true );

            var playerProfile = PlayerProfileDatabase.LoadPlayerProfile();
            var playerName = playerProfile.playerName;
            
            if( PhotonNetwork.IsConnected )
            {
                var nicknameChanged = !PhotonNetwork.NickName.Equals( playerName );
                if( nicknameChanged )
                {
                    HideNavigationButtons();
                    stateText.gameObject.SetActive( true );

                    PhotonNetwork.Disconnect();
                    PhotonNetwork.NickName = playerName;
                    PhotonNetwork.ConnectUsingSettings();
                }
                else
                {
                    regionText.gameObject.SetActive( true );
                    ShowNavigationButtons();
                    stateText.gameObject.SetActive( false );
                }
            }
            else
            {
                HideNavigationButtons();
                regionText.gameObject.SetActive( false );
                stateText.gameObject.SetActive( true );

                PhotonNetwork.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public void Hide()
        {
            if( !gameObject.activeSelf )
            {
                return;
            }

            createRoomPanel.Hide();
            insideRoomPanel.Hide();
            roomListPanel.Hide();

            roomChat.Hide();

            errorText.gameObject.SetActive( false );
            regionText.gameObject.SetActive( false );
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

            regionText.gameObject.SetActive( true );
            regionText.text = $"Region: {PhotonNetwork.CloudRegion.ToUpper()}";

            ShowNavigationButtons();
        }

        public override void OnDisconnected( DisconnectCause cause )
        {
            //print( "OnDisconnected: " + cause );
        }

        public override void OnLeftLobby()
        {
            //print( "OnLeftLobby" );
        }

        public override void OnJoinedRoom()
        {
            //print( "OnJoinedRoom" );

            regionText.gameObject.SetActive( false );
            HideNavigationButtons();
            createRoomPanel.Hide();
            roomListPanel.Hide();

            insideRoomPanel.Show( () =>
            {
                insideRoomPanel.Hide();
                regionText.gameObject.SetActive( true );
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
        bool isNavigationPanel;
        InputManager inputManager;

        
        void Awake()
        {
            closeButton.onClick.AddListener( Hide );

            createRoomButton.onClick.AddListener( OnCreateRoomButton );
            showRoomListButton.onClick.AddListener( OnShowRoomListButton );
            joinRandomRoomButton.onClick.AddListener( OnJoinRandomRoomButton );

            HideNavigationButtons();

            errorText.gameObject.SetActive( false );
            stateText.gameObject.SetActive( false );
            regionText.gameObject.SetActive( false );

            createRoomPanel.Hide();
            insideRoomPanel.Hide();
            roomListPanel.Hide();

            //PhotonNetwork.GameVersion = Application.version;
            PhotonNetwork.AutomaticallySyncScene = true;

            inputManager = InputManager.Instance;
        }


        public override void OnEnable()
        {
            base.OnEnable();
            
            inputManager.OnEscapeButton += OnEscapeButton;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            
            inputManager.OnEscapeButton -= OnEscapeButton;
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
            regionText.gameObject.SetActive( false );

            HideNavigationButtons();

            createRoomPanel.Show( () =>
            {
                createRoomPanel.Hide();
                regionText.gameObject.SetActive( true );
                ShowNavigationButtons();
            } );
        }

        void OnShowRoomListButton()
        {
            errorText.gameObject.SetActive( false );
            regionText.gameObject.SetActive( false );

            HideNavigationButtons();

            void onBackButton()
            {
                roomListPanel.Hide();
                regionText.gameObject.SetActive( true );
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


        void ShowNavigationButtons()
        {
            createRoomButton.gameObject.SetActive( true );
            showRoomListButton.gameObject.SetActive( true );
            joinRandomRoomButton.gameObject.SetActive( true );

            isNavigationPanel = true;
        }

        void HideNavigationButtons()
        {
            createRoomButton.gameObject.SetActive( false );
            showRoomListButton.gameObject.SetActive( false );
            joinRandomRoomButton.gameObject.SetActive( false );

            isNavigationPanel = false;
        }


        void OnEscapeButton()
        {
            if( isNavigationPanel )
            {
                Hide();
            }
        }
    }
}
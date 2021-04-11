using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RWS
{
    public class RoomListPanel : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        TextMeshProUGUI headerText = null;

        [SerializeField]
        GameObject roomListEntryPrefab = null;

        [SerializeField]
        Transform roomListEntryParent = null;

        [SerializeField]
        Button backButton = null;

        //----------------------------------------------------------------------------------------------------

        public void Show( Action onBackCallback, Action<RoomInfo> onJoinCallback )
        {
            this.onBackCallback = onBackCallback;
            this.onJoinCallback = onJoinCallback;

            gameObject.SetActive( true );

            if( PhotonNetwork.IsConnected && !PhotonNetwork.InLobby )
            {
                PhotonNetwork.JoinLobby();
            }
        }

        public void Hide()
        {
            onBackCallback = null;
            onJoinCallback = null;

            if( PhotonNetwork.IsConnected && PhotonNetwork.InLobby )
            {
                PhotonNetwork.LeaveLobby();
            }

            gameObject.SetActive( false );
        }

        //----------------------------------------------------------------------------------------------------

        public override void OnRoomListUpdate( List<RoomInfo> roomList )
        {
            headerText.text = $"Rooms ({roomList.Count})";

            foreach( var roomGameObject in roomGameObjectList )
            {
                Destroy( roomGameObject );
            }

            roomGameObjectList.Clear();

            foreach( var roomInfo in roomList )
            {
                if( roomInfo.RemovedFromList )
                {
                    continue;
                }

                var roomGameObject = Instantiate( roomListEntryPrefab, roomListEntryParent );
                //roomGameObject.transform.SetAsLastSibling();

                var roomListEntry = roomGameObject.GetComponent<RoomListEntry>();
                var roomNumber = roomGameObjectList.Count + 1;

                roomListEntry.Init( roomNumber, roomInfo, info =>
                {
                    print( $"Join to {roomInfo.Name}" );
                    onJoinCallback?.Invoke( roomInfo );
                } );

                roomGameObjectList.Add( roomGameObject );
            }
        }

        //----------------------------------------------------------------------------------------------------

        Action onBackCallback;
        Action<RoomInfo> onJoinCallback;
        List<GameObject> roomGameObjectList;
        InputManager inputManager;

        
        void Awake()
        {
            roomGameObjectList = new List<GameObject>();
            backButton.onClick.AddListener( OnBackButton );

            inputManager = InputManager.Instance;
        }

        void OnEnable()
        {
            inputManager.OnEscapeButton += OnEscapeButton;
        }

        void OnDisable()
        {
            inputManager.OnEscapeButton -= OnEscapeButton;
        }
        

        void OnBackButton()
        {
            onBackCallback?.Invoke();
        }

        void OnEscapeButton()
        {
            OnBackButton();
        }
    }
}
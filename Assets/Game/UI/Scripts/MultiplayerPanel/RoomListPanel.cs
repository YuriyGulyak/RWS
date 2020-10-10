using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using RWS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        
        InputManager.Instance.OnEscapeButton += OnEscapeButton;
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
        
        InputManager.Instance.OnEscapeButton -= OnEscapeButton;
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

    
    void Awake()
    {
        roomGameObjectList = new List<GameObject>();
        backButton.onClick.AddListener( OnBackButton );
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
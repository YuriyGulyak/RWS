using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InsideRoomPanel : MonoBehaviourPunCallbacks
{
    [SerializeField]
    TextMeshProUGUI roomNameText = null;
    
    [SerializeField]
    TextMeshProUGUI playerCountText = null;
    
    [SerializeField]
    Button leaveRoomButton = null;
    
    [SerializeField]
    Button startGameButton = null;

    [SerializeField]
    GameObject playerListEntryPrefab = null;

    [SerializeField]
    Transform playerListEntryParent = null;
    
    //----------------------------------------------------------------------------------------------------

    public void Show( Action onLeaveRoomCallback = null )
    {
        this.onLeaveRoomCallback = onLeaveRoomCallback;
        
        gameObject.SetActive( true );
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        foreach( var entry in PhotonNetwork.CurrentRoom.Players )
        {
            AddPlayerListEntry( entry.Value );
        }

        UpdatePlayerCountText();

        startGameButton.gameObject.SetActive( PhotonNetwork.IsMasterClient );
    }

    public void Hide()
    {
        gameObject.SetActive( false );
        onLeaveRoomCallback = null;
    }

    //----------------------------------------------------------------------------------------------------
    
    public override void OnPlayerEnteredRoom( Player newPlayer )
    {
        //print( "OnPlayerEnteredRoom" );
        UpdatePlayerCountText();
        AddPlayerListEntry( newPlayer );
    }

    public override void OnPlayerLeftRoom( Player otherPlayer )
    {
        //print( "OnPlayerLeftRoom" );
        UpdatePlayerCountText();
        RemovePlayerListEntry( otherPlayer );
    }

    //----------------------------------------------------------------------------------------------------
    
    Action onLeaveRoomCallback;
    Dictionary<int, GameObject> playerDictionary;
    
    
    void Awake()
    {
        playerDictionary = new Dictionary<int, GameObject>();
        leaveRoomButton.onClick.AddListener( OnLeaveRoomButton );
        startGameButton.onClick.AddListener( OnStartGameButton );
    }


    void OnLeaveRoomButton()
    {
        PhotonNetwork.LeaveRoom();

        foreach( var entry in playerDictionary )
        {
            Destroy( entry.Value );
        }
        playerDictionary.Clear();
        
        onLeaveRoomCallback?.Invoke();
    }

    void OnStartGameButton()
    {
        //PhotonNetwork.CurrentRoom.IsOpen = false;
        //PhotonNetwork.CurrentRoom.IsVisible = false;

        BlackScreen.Instance.StartToBlackScreenAnimation( () =>
        {
            PhotonNetwork.LoadLevel( 2 );
        } );
    }

    
    void UpdatePlayerCountText()
    {
        playerCountText.text = $"Players: {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
    }
    
    void AddPlayerListEntry( Player player )
    {
        var playerGameObject = Instantiate( playerListEntryPrefab, playerListEntryParent );
        //playerGameObject.transform.SetAsLastSibling();
        
        var playerListEntry = playerGameObject.GetComponent<PlayerListEntry>();
        playerListEntry.Init( playerDictionary.Count + 1, player.NickName );

        playerDictionary.Add( player.ActorNumber, playerGameObject );
    }

    void RemovePlayerListEntry( Player player )
    {
        if( playerDictionary.ContainsKey( player.ActorNumber ) )
        {
            Destroy( playerDictionary[ player.ActorNumber ] );
            playerDictionary.Remove( player.ActorNumber );
        }
    }
}

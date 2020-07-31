using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerOverviewPanel : MonoBehaviourPunCallbacks
{
    [SerializeField] 
    GameObject playerOverviewEntryPrefab = null;

    [SerializeField] 
    Transform playerOverviewEntryParent = null;

    //----------------------------------------------------------------------------------------------------

    public bool IsActive => gameObject.activeSelf;
    
    public void Show()
    {
        if( !IsActive )
        {
            gameObject.SetActive( true );
        }
    }

    public void Hide()
    {
        if( IsActive )
        {
            gameObject.SetActive( false );
        }
    }
    
    //----------------------------------------------------------------------------------------------------

    public override void OnPlayerEnteredRoom( Player newPlayer )
    {
        //print( $"OnPlayerEnteredRoom: {newPlayer.ActorNumber}" );

        if( !playerListEntries.ContainsKey( newPlayer.ActorNumber ) )
        {
            playerListEntries.Add( newPlayer.ActorNumber, CreatePlayerEntry( playerListEntries.Count + 1, 0f, newPlayer.NickName ) );
        }
    }
    
    public override void OnPlayerLeftRoom( Player otherPlayer )
    {
        //print( $"OnPlayerLeftRoom: {otherPlayer.ActorNumber}" );

        if( playerListEntries.ContainsKey( otherPlayer.ActorNumber ) )
        {
            Destroy( playerListEntries[ otherPlayer.ActorNumber ].gameObject );
            playerListEntries.Remove( otherPlayer.ActorNumber );
        }
    }

    public override void OnPlayerPropertiesUpdate( Player targetPlayer, Hashtable hashtable )
    {
        //print( $"OnPlayerPropertiesUpdate: {targetPlayer.NickName}" );
        
        if( playerListEntries.ContainsKey( targetPlayer.ActorNumber ) )
        {
            var newBestTime = (float)targetPlayer.CustomProperties[ bestLapPropertyKey ];
            playerListEntries[ targetPlayer.ActorNumber ].SetTime( newBestTime );
        }
    }
    

    readonly string bestLapPropertyKey = "BestLapProperty";
    Dictionary<int, PlayerOverviewEntry> playerListEntries;

    
    void Awake()
    {
        playerListEntries = new Dictionary<int, PlayerOverviewEntry>();

        if( !PhotonNetwork.IsConnected )
        {
            return;
        }

        var playerNumber = 1;
        foreach( var player in PhotonNetwork.PlayerList )
        {
            playerListEntries.Add( player.ActorNumber, CreatePlayerEntry( playerNumber++, 0f, player.NickName ) );
        }
    }

    /*
    void Update()
    {
        if( Input.GetKeyDown( KeyCode.C ) )
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties( new Hashtable { { bestLapPropertyKey, 15f } } );
        }
    }
    */

    PlayerOverviewEntry CreatePlayerEntry( int number, float time, string nickName )
    {
        var playerOverviewEntryGameObject = Instantiate( playerOverviewEntryPrefab, playerOverviewEntryParent );
        var playerOverviewEntry = playerOverviewEntryGameObject.GetComponent<PlayerOverviewEntry>();

        playerOverviewEntry.SetNumber( number );
        playerOverviewEntry.SetTime( time );
        playerOverviewEntry.SetName( nickName );

        return playerOverviewEntry;
    }
}
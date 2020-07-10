using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class TestPhotonConnect : MonoBehaviourPunCallbacks
{
    void Start()
    {
        //PhotonNetwork.NickName = "";
        //PhotonNetwork.GameVersion = "";
        PhotonNetwork.ConnectUsingSettings();
    }


    public override void OnConnectedToMaster()
    {
        print( "OnConnectedToMaster" );
    }

    public override void OnDisconnected( DisconnectCause cause )
    {
        print( "OnDisconnected: " + cause );
    }
}
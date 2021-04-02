using System;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RWS
{
    public class RoomListEntry : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI numberText = null;

        [SerializeField]
        TextMeshProUGUI nameText = null;

        [SerializeField]
        TextMeshProUGUI playersText = null;

        [SerializeField]
        GameObject closedGameObject = null;

        [SerializeField]
        Button joinButton = null;

        //----------------------------------------------------------------------------------------------------

        public void Init( int number, RoomInfo roomInfo, Action<RoomInfo> onJoinCallback )
        {
            this.roomInfo = roomInfo;
            this.onJoinCallback = onJoinCallback;

            numberText.text = $"{number}.";
            nameText.text = roomInfo.Name;
            playersText.text = $"Players: {roomInfo.PlayerCount} / {roomInfo.MaxPlayers}";
            closedGameObject.SetActive( !roomInfo.IsOpen );
        }

        //----------------------------------------------------------------------------------------------------

        RoomInfo roomInfo;
        Action<RoomInfo> onJoinCallback;


        void Awake()
        {
            joinButton.onClick.AddListener( OnJoinButton );
        }

        void OnDestroy()
        {
            onJoinCallback = null;
        }


        void OnJoinButton()
        {
            onJoinCallback?.Invoke( roomInfo );
        }
    }
}

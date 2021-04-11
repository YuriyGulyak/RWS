using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RWS
{
    public class CreateRoomPanel : MonoBehaviour
    {
        [SerializeField]
        TMP_InputField roomNameInputField;

        //[SerializeField]
        //Toggle roomIsOpenToggle = null;

        //[SerializeField]
        //TMP_InputField passwordInputField = null;

        [SerializeField]
        TMP_InputField maxPlayersInputField;

        [SerializeField]
        TMP_Dropdown trackDropdown;

        [SerializeField]
        Toggle infiniteBatteryToggle;

        [SerializeField]
        Toggle infiniteRangeToggle;

        [SerializeField]
        Button createRoomButton;

        [SerializeField]
        Button cancelButton;

        [SerializeField]
        int minPlayers = 2;

        [SerializeField]
        int maxPlayers = 8;

        //----------------------------------------------------------------------------------------------------

        public void Show( Action onCancelCallback = null )
        {
            this.onCancelCallback = onCancelCallback;
            gameObject.SetActive( true );

            roomNameInputField.text = $"Room_{UnityEngine.Random.Range( 0, 10000 ):0000}";
        }

        public void Hide()
        {
            gameObject.SetActive( false );
            onCancelCallback = null;
        }

        //----------------------------------------------------------------------------------------------------

        Action onCancelCallback;
        InputManager inputManager;
        

        void Awake()
        {
            roomNameInputField.text = "";

            maxPlayersInputField.onEndEdit.AddListener( inputFieldValue =>
            {
                var inputValue = int.Parse( maxPlayersInputField.text );
                inputValue = Mathf.Clamp( inputValue, minPlayers, maxPlayers );
                maxPlayersInputField.text = inputValue.ToString();
            } );

            trackDropdown.onValueChanged.AddListener( trackIndex => {} );

            createRoomButton.onClick.AddListener( OnCreateRoomButton );
            cancelButton.onClick.AddListener( OnCancelButton );

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


        void OnCreateRoomButton()
        {
            var roomName = roomNameInputField.text;
            var trackIndex = trackDropdown.value;
            var maxPlayersInputValue =
                (byte)Mathf.Clamp( int.Parse( maxPlayersInputField.text ), minPlayers, maxPlayers );

            var trackName = $"Racing Track {trackIndex + 1}";

            var roomOptions = new RoomOptions
            {
                IsVisible = true,
                IsOpen = true,
                MaxPlayers = maxPlayersInputValue,

                CustomRoomProperties = new Hashtable
                {
                    { "TrackIndex", trackIndex },
                    { "TrackName", trackName },
                    { "InfiniteBattery", infiniteBatteryToggle.isOn },
                    { "InfiniteRange", infiniteRangeToggle.isOn }
                }
            };

            PhotonNetwork.CreateRoom( roomName, roomOptions );
        }

        void OnCancelButton()
        {
            onCancelCallback?.Invoke();
        }

        void OnEscapeButton()
        {
            if( roomNameInputField.isFocused || maxPlayersInputField.isFocused )
            {
                return;
            }

            OnCancelButton();
        }
    }
}
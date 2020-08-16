using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RWS
{
    public class RoomChat : MonoBehaviourPun
    {
        [SerializeField]
        TMP_InputField inputField = null;

        [SerializeField]
        Button sendButton = null;

        [SerializeField]
        GameObject messagePrefab = null;

        [SerializeField]
        Transform contentTransform = null;

        [SerializeField]
        GameObject inputPanel = null;
        
        [SerializeField]
        int maxMessages = 10;
        
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

        public void ShowInput()
        {
            inputPanel.SetActive( true );
            inputField.Select();
        }
        
        public void HideInput()
        {
            inputPanel.SetActive( false );
        }
        
        //----------------------------------------------------------------------------------------------------

        void Awake()
        {
            inputField.onSubmit.AddListener( OnSubmit );
            sendButton.onClick.AddListener( () => OnSubmit( inputField.text ) );
        }

        //----------------------------------------------------------------------------------------------------

        [PunRPC]
        void ChatMessageRPC( Player sender, string message )
        {
            CreateChatMessage( sender.NickName, message );
        }

        void OnSubmit(  string message )
        {
            if( string.IsNullOrEmpty( message ) )
            {
                return;
            }

            SendChatMessage( message );
            inputField.text = "";
            inputField.ActivateInputField();
        }

        void SendChatMessage( string message )
        {
            if( string.IsNullOrEmpty( message ) )
            {
                return;
            }

            if( !PhotonNetwork.IsConnected )
            {
                // Debug
                //CreateChatMessage( "Nickname", message );
                return;
            }
            
            photonView.RPC( "ChatMessageRPC", RpcTarget.All, PhotonNetwork.LocalPlayer, message );
        }

        void CreateChatMessage( string senderName, string message )
        {
            var messageGameObject = Instantiate( messagePrefab, contentTransform );
            messageGameObject.GetComponent<RoomChatMessage>().Init( $"<b>{senderName}</b>: {message}" );

            if( contentTransform.childCount > maxMessages )
            {
                Destroy( contentTransform.GetChild( 0 ).gameObject );
            }
        }
    }
}

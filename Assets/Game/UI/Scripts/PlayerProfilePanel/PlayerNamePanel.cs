/*
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RWS
{
    public class PlayerNamePanel : MonoBehaviour
    {
        [SerializeField]
        TMP_InputField nameInputField = null;

        [SerializeField]
        Button applyNameButton = null;

        [SerializeField]
        TextMeshProUGUI errorText = null;


        string nickname;


        void OnEnable()
        {
            if( PlayerPrefs.HasKey( "Nickname" ) )
            {
                nickname = PlayerPrefs.GetString( "Nickname" );
            }
            else
            {
                nickname = $"Player{UnityEngine.Random.Range( 0, 10000 ):0000}";
            }

            nameInputField.text = nickname;
        }
    }
}
*/

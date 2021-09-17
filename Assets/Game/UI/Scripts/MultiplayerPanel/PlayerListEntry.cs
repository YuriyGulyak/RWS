using TMPro;
using UnityEngine;

namespace RWS
{
    public class PlayerListEntry : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI numberText = null;

        [SerializeField]
        TextMeshProUGUI nameText = null;


        public void Init( int number, string playerName )
        {
            numberText.text = $"{number}.";
            nameText.text = playerName;
        }
    }
}
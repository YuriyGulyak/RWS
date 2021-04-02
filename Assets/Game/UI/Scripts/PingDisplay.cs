using System.Globalization;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace RWS
{
    public class PingDisplay : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI textMesh = null;

        [SerializeField]
        float updateRate = 30f;


        float lastUpdateTime;
        string pingFormat;
        CultureInfo invariantCulture;


        void Awake()
        {
            pingFormat = textMesh.text;
            invariantCulture = CultureInfo.InvariantCulture;
        }

        void Update()
        {
            if( Time.timeScale.Equals( 0f ) )
            {
                return;
            }

            var time = Time.time;
            if( time - lastUpdateTime > 1f / updateRate )
            {
                lastUpdateTime = time;

                var ping = PhotonNetwork.GetPing();
                textMesh.text = ping.ToString( pingFormat, invariantCulture );
            }
        }
    }
}

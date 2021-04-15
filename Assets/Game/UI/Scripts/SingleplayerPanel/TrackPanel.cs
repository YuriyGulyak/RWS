using System;
using TMPro;
using UnityEngine;

namespace RWS
{
    public class TrackPanel : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI localBestLapText = null;

        [SerializeField]
        TextMeshProUGUI globalBestLapText = null;

        [SerializeField]
        BestLapKeyStorage bestLapKeyStorage = null;

        [SerializeField]
        int trackIndex = 0;

        //----------------------------------------------------------------------------------------------------

        readonly string timeFormat = @"mm\:ss\.ff";
        Leaderboard leaderboard;


        void Awake()
        {
            leaderboard = Leaderboard.Instance;
        }

        void OnEnable()
        {
            var bestLapKeyItem = bestLapKeyStorage.items[ trackIndex ];

            var localBestLapKey = bestLapKeyItem.playerPrefsKey;
            if( PlayerPrefs.HasKey( localBestLapKey ) )
            {
                var bestLapSeconds = PlayerPrefs.GetFloat( localBestLapKey );
                localBestLapText.text = TimeSpan.FromSeconds( bestLapSeconds ).ToString( timeFormat );
            }
            else
            {
                localBestLapText.text = "N/A";
            }

            var dreamloPublicCode = bestLapKeyItem.dreamloPublicCode;
            leaderboard.GetRecords( dreamloPublicCode, 0, 1, records =>
            {
                if( records.Length == 0 )
                {
                    globalBestLapText.text = "N/A";
                }
                else
                {
                    globalBestLapText.text = TimeSpan.FromSeconds( records[ 0 ].seconds ).ToString( timeFormat );
                }
            }, null );
        }
    }
}

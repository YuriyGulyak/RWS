using System;
using TMPro;
using UnityEngine;

public class TrackPanel : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI localBestLapText;

    [SerializeField]
    TextMeshProUGUI globalBestLapText;
    
    [SerializeField]
    string localBestLapKey;
    
    [SerializeField]
    string dreamloPublicCode;

    //----------------------------------------------------------------------------------------------------
    
    readonly string timeFormat = @"mm\:ss\.ff";
    Leaderboard leaderboard;

    
    void Awake()
    {
        leaderboard = Leaderboard.Instance;   
    }

    void OnEnable()
    {
        if( PlayerPrefs.HasKey( localBestLapKey ) )
        {
            var bestLapSeconds = PlayerPrefs.GetFloat( localBestLapKey );
            localBestLapText.text = TimeSpan.FromSeconds( bestLapSeconds ).ToString( timeFormat );
        }
        else
        {
            localBestLapText.text = "N/A";
        }

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

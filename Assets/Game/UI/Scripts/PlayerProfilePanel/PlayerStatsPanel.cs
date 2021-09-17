using System;
using System.Globalization;
using UnityEngine;
using TMPro;

namespace RWS
{
    public class PlayerStatsPanel : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI totalFlightTimeText = null;
        
        [SerializeField]
        TextMeshProUGUI totalFlightDistanceText = null;
        
        [SerializeField]
        TextMeshProUGUI longestFlightTimeText = null;
        
        [SerializeField]
        TextMeshProUGUI topSpeedText = null;
        
        [SerializeField]
        TextMeshProUGUI completedLapsText = null;
        
        [SerializeField]
        TextMeshProUGUI numberOfLaunchesText = null;
        
        [SerializeField]
        TextMeshProUGUI numberOfCrashesText = null;


        public void SetTotalFlightTime( float seconds )
        {
            if( seconds > 0f )
            {
                var time = TimeSpan.FromSeconds( seconds );
                totalFlightTimeText.text = time.ToString( @"hh\:mm\:ss" );
            }
            else
            {
                totalFlightTimeText.text = "N/A";
            }
        }
        
        public void SetTotalFlightDistance( float meters )
        {
            if( meters > 0f )
            {
                totalFlightDistanceText.text = $"{( meters / 1000f ).ToString( "0.0", CultureInfo.InvariantCulture )} km";
            }
            else
            {
                totalFlightDistanceText.text = "N/A";
            }
        }

        public void SetLongestFlightTime( float seconds )
        {
            if( seconds > 0f )
            {
                var time = TimeSpan.FromSeconds( seconds );
                longestFlightTimeText.text = time.ToString( @"hh\:mm\:ss" );
            }
            else
            {
                longestFlightTimeText.text = "N/A";
            }
        }
        
        public void SetTopSpeed( float kmh )
        {
            if( kmh > 0f )
            {
                topSpeedText.text = $"{kmh.ToString( "0", CultureInfo.InvariantCulture )} km/h";
            }
            else
            {
                topSpeedText.text = "N/A";
            }
        }
        
        public void SetCompletedLaps( int count )
        {
            if( count > 0 )
            {
                completedLapsText.text = count.ToString();
            }
            else
            {
                completedLapsText.text = "N/A";
            }
        }
        
        public void SetNumberOfLaunches( int count )
        {
            if( count > 0 )
            {
                numberOfLaunchesText.text = count.ToString();
            }
            else
            {
                numberOfLaunchesText.text = "N/A";
            }
        }
        
        public void SetNumberOfCrashes( int count )
        {
            if( count > 0 )
            {
                numberOfCrashesText.text = count.ToString();
            }
            else
            {
                numberOfCrashesText.text = "N/A";
            }
        }
    }
}

using System;
using TMPro;
using UnityEngine;

public class PlayerOverviewEntry : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI numberText = null;
    
    [SerializeField]
    TextMeshProUGUI timeText = null;
    
    [SerializeField]
    TextMeshProUGUI nameText = null;
    
    
    readonly string timeFormat = @"mm\:ss\.ff";
    

    public void SetNumber( int number )
    {
        numberText.text = $"{number}.";
    }
    
    public void SetTime( float seconds )
    {
        if( seconds.Equals( 0f ) )
        {
            timeText.text = "No Time";
        }
        else
        {
            timeText.text = TimeSpan.FromSeconds( seconds ).ToString( timeFormat );
        }
    }

    public void SetName( string name )
    {
        nameText.text = name;
    }
}
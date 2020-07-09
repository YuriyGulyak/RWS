using System;
using TMPro;
using UnityEngine;

public class LapTimer : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI timeText = null;
 
    [SerializeField]
    TextMeshProUGUI bestTimeText = null;

    [SerializeField]
    float maxTime = 120f;
    
    [SerializeField]
    float updateRate = 30f;

    //----------------------------------------------------------------------------------------------------

    public void StartNewTime()
    {
        lapTime = 0f;
        lapStarted = true;

        if( !timeVisible )
        {
            ShowTime();
        }
    }
    
    public void CompareTime()
    {
        if( bestTime.Equals( 0f ) || lapTime < bestTime )
        {
            bestTime = lapTime;
            bestTimeText.text = $"Best: {TimeSpan.FromSeconds( bestTime ).ToString( timeFormat )}";
        }
    }

    public void ResetAndHide()
    {
        lapTime = 0f;
        lapStarted = false;

        if( timeVisible )
        {
            HideTime();
        }
    }

    //----------------------------------------------------------------------------------------------------
    
    string timeFormat = @"mm\:ss\.ff";
    
    bool lapStarted;
    float lapTime;
    float bestTime;
    float lastUpdateTime;
    bool timeVisible;
    
    
    void Awake()
    {
        HideTime();
    }
    
    void Update()
    {
        if( !lapStarted )
        {
            return;
        }
        
        lapTime += Time.deltaTime;

        if( Time.time - lastUpdateTime > 1f / updateRate )
        {
            lastUpdateTime = Time.time;
            timeText.text = TimeSpan.FromSeconds( lapTime ).ToString( timeFormat );
        }
        
        if( lapTime > maxTime )
        {
            lapTime = 0f;
            lapStarted = false;
            HideTime();
        }
    }

    
    void ShowTime()
    {
        timeVisible = true;
        timeText.enabled = true;
        bestTimeText.enabled = true;
    }
    
    void HideTime()
    {
        timeVisible = false;
        timeText.enabled = false;
        bestTimeText.enabled = false;
    }
}

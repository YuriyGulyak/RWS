﻿using System;
using RWS;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SingleplayerPanel : MonoBehaviour
{
    [SerializeField]
    RectTransform panelRect;

    [SerializeField]
    Button closeButton;

    [SerializeField]
    TextMeshProUGUI localBestLapText;

    [SerializeField]
    TextMeshProUGUI globalBestLapText;

    [SerializeField]
    Toggle infiniteBatteryToggle;

    [SerializeField]
    Toggle infiniteRangeToggle;

    [SerializeField]
    Toggle showGhostToggle;

    [SerializeField]
    Button startGameButton;

    //----------------------------------------------------------------------------------------------------

    public bool IsOpen => gameObject.activeSelf;
    
    public void Show()
    {
        if( gameObject.activeSelf )
        {
            return;
        }

        gameObject.SetActive( true );

        
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

        
        infiniteBatteryToggle.isOn = PlayerPrefs.GetInt( infiniteBatteryKey, 0 ) > 0;
        infiniteRangeToggle.isOn = PlayerPrefs.GetInt( infiniteRangeKey, 0 ) > 0;
        showGhostToggle.isOn = PlayerPrefs.GetInt( showGhostKey, 0 ) > 0;
    }

    public void Hide()
    {
        if( !gameObject.activeSelf )
        {
            return;
        }

        gameObject.SetActive( false );
        panelRect.anchoredPosition = Vector2.zero;
    }

    //----------------------------------------------------------------------------------------------------

    readonly string localBestLapKey = "LocalBestLap";
    readonly string infiniteBatteryKey = "InfiniteBattery";
    readonly string infiniteRangeKey = "InfiniteRange";
    readonly string showGhostKey = "ShowGhost";

    readonly string dreamloPublicCode = "5f9550d3eb371809c4a783ef";
    readonly string timeFormat = @"mm\:ss\.ff";
    Leaderboard leaderboard;


    void Awake()
    {
        closeButton.onClick.AddListener( Hide );

        infiniteBatteryToggle.onValueChanged.AddListener( value => PlayerPrefs.SetInt( infiniteBatteryKey, value ? 1 : 0 ) );
        infiniteRangeToggle.onValueChanged.AddListener( value => PlayerPrefs.SetInt( infiniteRangeKey, value ? 1 : 0 ) );
        showGhostToggle.onValueChanged.AddListener( value => PlayerPrefs.SetInt( showGhostKey, value ? 1 : 0 ) );

        startGameButton.onClick.AddListener( () => { BlackScreen.Instance.StartToBlackScreenAnimation( () => { SceneManager.LoadSceneAsync( 1 ); } ); } );

        InputManager.Instance.OnEscapeButton += OnEscapeButton;

        leaderboard = Leaderboard.Instance;
    }


    void OnEscapeButton()
    {
        Hide();
    }
}
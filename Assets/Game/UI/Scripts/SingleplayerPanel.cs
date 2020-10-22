using System;
using RWS;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SingleplayerPanel : MonoBehaviour
{
    [SerializeField]
    RectTransform panelRect = null;
    
    [SerializeField]
    Button closeButton = null;
    
    [SerializeField]
    TextMeshProUGUI bestLapText = null;

    [SerializeField]
    Toggle infiniteBatteryToggle = null; 
    
    [SerializeField]
    Toggle infiniteRangeToggle = null;
    
    [SerializeField]
    Button startGameButton = null;
    
    //----------------------------------------------------------------------------------------------------
    
    public void Show()
    {
        if( gameObject.activeSelf )
        {
            return;
        }
        gameObject.SetActive( true );

        if( PlayerPrefs.HasKey( bestLapKey ) )
        {
            var bestLapSeconds = PlayerPrefs.GetFloat( bestLapKey );
            bestLapText.text = TimeSpan.FromSeconds( bestLapSeconds ).ToString( timeFormat );
        }
        else
        {
            bestLapText.text = "N/A";
        }

        infiniteBatteryToggle.isOn = PlayerPrefs.GetInt( infiniteBatteryKey, 0 ) > 0;
        infiniteRangeToggle.isOn = PlayerPrefs.GetInt( infiniteRangeKey, 0 ) > 0;
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

    // TODO Temporary solution
    readonly string bestLapKey = "BestLap";
    readonly string infiniteBatteryKey = "InfiniteBattery";
    readonly string infiniteRangeKey = "InfiniteRange";
    
    string timeFormat = @"mm\:ss\.ff";
    
    
    void Awake()
    {
        closeButton.onClick.AddListener( Hide );
        
        infiniteBatteryToggle.onValueChanged.AddListener( value =>
        {
            PlayerPrefs.SetInt( infiniteBatteryKey, value ? 1 : 0 );
        } );
        infiniteRangeToggle.onValueChanged.AddListener( value =>
        {
            PlayerPrefs.SetInt( infiniteRangeKey, value ? 1 : 0 );
        } );

        startGameButton.onClick.AddListener( () =>
        {
            BlackScreen.Instance.StartToBlackScreenAnimation( () =>
            {
                SceneManager.LoadSceneAsync( 1 );
            } );
        } );

        InputManager.Instance.OnEscapeButton += OnEscapeButton;
    }

    
    void OnEscapeButton()
    {
        Hide();
    }
}

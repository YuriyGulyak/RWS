using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class OSDTelemetry : MonoBehaviour
{
    [SerializeField]
    FlyingWing flyingWing = null;

    [SerializeField]
    OSDHome osdHome = null;

    [SerializeField]
    AttitudeIndicator attitudeIndicator = null;

    [SerializeField]
    OSDWarnings osdWarnings = null;  
    
    [SerializeField]
    TextMeshProUGUI voltageText = null;
    
    [SerializeField]
    TextMeshProUGUI cellVoltageText = null;

    [SerializeField]
    TextMeshProUGUI mahUsedText = null;

    [SerializeField]
    TextMeshProUGUI currentText = null;

    [SerializeField]
    TextMeshProUGUI rpmText = null;
    
    [SerializeField]
    TextMeshProUGUI speedText = null;

    [SerializeField]
    TextMeshProUGUI altitudeText = null;

    [SerializeField]
    TextMeshProUGUI flytimeText = null;
    
    [SerializeField]
    TextMeshProUGUI throttleText = null;
    
    [SerializeField]
    TextMeshProUGUI rssiText = null;
    
    [SerializeField]
    float updateRate = 30f;
    
    //----------------------------------------------------------------------------------------------------
    
    public void Init( FlyingWing flyingWing )
    {
        this.flyingWing = flyingWing;
        
        osdHome.Init( flyingWing );
        attitudeIndicator.Init( flyingWing );
        osdWarnings.Init( flyingWing );
    }
    
    public bool IsActive => gameObject.activeSelf;
    
    public void Show()
    {
        if( !IsActive )
        {
            gameObject.SetActive( true );
        }
        attitudeIndicator.Show();
    }

    public void Hide()
    {
        if( IsActive )
        {
            gameObject.SetActive( false );
        }
        attitudeIndicator.Hide();
    }

    public void Reset()
    {
        voltageText.text = 0f.ToString( voltageFormat, cultureInfo );
        cellVoltageText.text = 0f.ToString( voltageFormat, cultureInfo );
        mahUsedText.text = 0f.ToString( mahUsedFormat, cultureInfo );
        rpmText.text = 0f.ToString( rpmFormat, cultureInfo );
        currentText.text = 0f.ToString( currentFormat, cultureInfo );
        speedText.text = 0f.ToString( speedFormat, cultureInfo );
        altitudeText.text = 0f.ToString( altitudeFormat, cultureInfo );
        flytimeText.text = TimeSpan.FromSeconds( 0f ).ToString( timeFormat, cultureInfo );
        rssiText.text = 0f.ToString( rssiFormat, cultureInfo );
        
        osdHome.Reset();
        attitudeIndicator.Reset();
        osdWarnings.Reset();
    }

    //----------------------------------------------------------------------------------------------------

    readonly string timeFormat = @"m\:ss";
    
    string voltageFormat;
    string mahUsedFormat;
    string rpmFormat;
    string currentFormat;
    string speedFormat;
    string altitudeFormat;
    string throttleFormat;
    string rssiFormat;
    CultureInfo cultureInfo;
    CustomUpdate customUpdate;


    void Awake()
    {
        voltageFormat = voltageText.text;
        mahUsedFormat = mahUsedText.text;
        currentFormat = currentText.text;
        rpmFormat = rpmText.text;
        speedFormat = speedText.text;
        altitudeFormat = altitudeText.text;
        throttleFormat = throttleText.text;
        rssiFormat = rssiText.text;
        cultureInfo = CultureInfo.InvariantCulture;
        
        customUpdate = new CustomUpdate( updateRate );
        customUpdate.OnUpdate += OnUpdate;
    }

    void Update()
    {
        customUpdate.Update( Time.time );
    }


    void OnUpdate( float deltaTime )
    {
        if( !flyingWing )
        {
            return;
        }

        speedText.text = ( flyingWing.TAS * 3.6f ).ToString( speedFormat, cultureInfo );
        altitudeText.text = flyingWing.Altitude.ToString( altitudeFormat, cultureInfo );
        flytimeText.text = TimeSpan.FromSeconds( flyingWing.Flytime ).ToString( timeFormat, cultureInfo );
        throttleText.text = flyingWing.Throttle.ToString( throttleFormat, cultureInfo );
        rssiText.text = flyingWing.RSSI.ToString( rssiFormat, cultureInfo );
        voltageText.text = flyingWing.Voltage.ToString( voltageFormat, cultureInfo );
        cellVoltageText.text = flyingWing.CellVoltage.ToString( voltageFormat, cultureInfo );
        mahUsedText.text = flyingWing.CapacityDrawn.ToString( mahUsedFormat, cultureInfo );
        currentText.text = flyingWing.CurrentDraw.ToString( currentFormat, cultureInfo );
        rpmText.text = flyingWing.RPM.ToString( rpmFormat, cultureInfo );
    }
}

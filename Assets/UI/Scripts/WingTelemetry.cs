using System.Globalization;
using TMPro;
using UnityEngine;

public class WingTelemetry : MonoBehaviour
{
    [SerializeField]
    FlyingWing flyingWing = null;

    [SerializeField]
    TextMeshProUGUI speedText = null;

    [SerializeField]
    TextMeshProUGUI altitudeText = null;
    
    [SerializeField]
    TextMeshProUGUI angleOfAttackText = null;
    
    [SerializeField]
    TextMeshProUGUI rollSpeedText = null;
    
    [SerializeField]
    TextMeshProUGUI pitchSpeedText = null;
    
    [SerializeField]
    TextMeshProUGUI sideslipAngleText = null;

    [SerializeField]
    float updateRate = 30f;
    

    public void Init( FlyingWing flyingWing )
    {
        this.flyingWing = flyingWing;
    }


    string speedFormat;
    string altitudeFormat;
    string angleOfAttackFormat;
    string rollSpeedFormat;
    string pitchSpeedFormat;
    string sideslipAngleFormat;
    CultureInfo cultureInfo;
    float lastUpdateTime;


    void Awake()
    {
        speedFormat = speedText.text;
        altitudeFormat = altitudeText.text;
        angleOfAttackFormat = angleOfAttackText.text;
        rollSpeedFormat = rollSpeedText.text;
        pitchSpeedFormat = pitchSpeedText.text;
        sideslipAngleFormat = sideslipAngleText.text;

        cultureInfo = CultureInfo.InvariantCulture;
    }

    void Update()
    {
        if( !flyingWing )
        {
            return;
        }

        if( Time.time - lastUpdateTime > 1f / updateRate )
        {
            lastUpdateTime = Time.time;
            UpdateUI();
        }
    }


    void UpdateUI()
    {
        speedText.text = ( flyingWing.TAS * 3.6f ).ToString( speedFormat, cultureInfo );
        altitudeText.text = flyingWing.Altitude.ToString( altitudeFormat, cultureInfo );
        angleOfAttackText.text = flyingWing.AngleOfAttack.ToString( angleOfAttackFormat, cultureInfo );
        rollSpeedText.text = flyingWing.RollSpeed.ToString( rollSpeedFormat, cultureInfo );
        pitchSpeedText.text = flyingWing.PitchSpeed.ToString( pitchSpeedFormat, cultureInfo );
        sideslipAngleText.text = flyingWing.SideslipAngle.ToString( sideslipAngleFormat, cultureInfo );
    }
}

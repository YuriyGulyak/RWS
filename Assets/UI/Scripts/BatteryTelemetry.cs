using System.Globalization;
using TMPro;
using UnityEngine;

public class BatteryTelemetry : MonoBehaviour
{
    [SerializeField]
    Battery battery = null;

    [SerializeField]
    TextMeshProUGUI socText = null;
    
    [SerializeField]
    TextMeshProUGUI voltageText = null;

    [SerializeField]
    TextMeshProUGUI mahUsedText = null;

    [SerializeField]
    float updateRate = 30f;
    
    
    public void Init( Battery battery )
    {
        this.battery = battery;
    }
    
    
    string socFormat;
    string voltageFormat;
    string mahUsedFormat;
    CultureInfo cultureInfo;
    float lastUpdateTime;
    

    void Awake()
    {
        socFormat = socText.text;
        voltageFormat = voltageText.text;
        mahUsedFormat = mahUsedText.text;
        cultureInfo = CultureInfo.InvariantCulture;
    }

    void Update()
    {
        if( !battery )
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
        socText.text = battery.StateOfCharge.ToString( socFormat, cultureInfo );
        voltageText.text = battery.Voltage.ToString( voltageFormat, cultureInfo );

        var mahUsed = battery.CapacityDrawn * 1000f;
        mahUsedText.text = mahUsed.ToString( mahUsedFormat, cultureInfo );
    }
}

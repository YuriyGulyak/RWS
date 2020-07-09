using System.Globalization;
using TMPro;
using UnityEngine;

public class MotorTelemetry : MonoBehaviour
{
    [SerializeField]
    Motor motor = null;

    [SerializeField]
    TextMeshProUGUI throttleText = null;
    
    [SerializeField]
    TextMeshProUGUI rpmText = null;

    [SerializeField]
    TextMeshProUGUI currentText = null;
    
    [SerializeField]
    TextMeshProUGUI thrustText = null;
    
    [SerializeField]
    float updateRate = 30f;
    
    
    public void Init( Motor motor )
    {
        this.motor = motor;
    }
    
    
    string throttleFormat;
    string rpmFormat;
    string currentFormat;
    string thrustFormat;
    CultureInfo cultureInfo;
    float lastUpdateTime;
    

    void Awake()
    {
        throttleFormat = throttleText.text;
        rpmFormat = rpmText.text;
        currentFormat = currentText.text;
        thrustFormat = thrustText.text;
        cultureInfo = CultureInfo.InvariantCulture;
    }

    void Update()
    {
        if( !motor )
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
        throttleText.text = motor.throttle.ToString( throttleFormat, cultureInfo );
        rpmText.text = motor.rpm.ToString( rpmFormat, cultureInfo );
        currentText.text = motor.current.ToString( currentFormat, cultureInfo );
        thrustText.text = ( motor.thrust / 9.8f ).ToString( thrustFormat, cultureInfo );
    }
}

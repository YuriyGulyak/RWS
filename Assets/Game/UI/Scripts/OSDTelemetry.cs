﻿using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class OSDTelemetry : MonoBehaviour
{
    [SerializeField]
    Battery battery = null;

    [SerializeField]
    Motor motor = null;

    [SerializeField]
    FlyingWing flyingWing = null;

    [SerializeField]
    TextMeshProUGUI socText = null;
    
    [SerializeField]
    TextMeshProUGUI voltageText = null;

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
    float updateRate = 30f;
    
    
    public void Init( FlyingWing flyingWing, Motor motor, Battery battery )
    {
        this.flyingWing = flyingWing;
        this.motor = motor;
        this.battery = battery;
    }
    
    public bool IsActive => gameObject.activeSelf;
    
    public void Show()
    {
        if( !IsActive )
        {
            gameObject.SetActive( true );
        }
    }

    public void Hide()
    {
        if( IsActive )
        {
            gameObject.SetActive( false );
        }
    }

    public void Reset()
    {
        flytime = 0f;
        
        socText.text = 0f.ToString( socFormat, cultureInfo );
        voltageText.text = 0f.ToString( voltageFormat, cultureInfo );
        mahUsedText.text = 0f.ToString( mahUsedFormat, cultureInfo );
        rpmText.text = 0f.ToString( rpmFormat, cultureInfo );
        currentText.text = 0f.ToString( currentFormat, cultureInfo );
        speedText.text = 0f.ToString( speedFormat, cultureInfo );
        altitudeText.text = 0f.ToString( altitudeFormat, cultureInfo );
        flytimeText.text = TimeSpan.FromSeconds( 0f ).ToString( timeFormat, cultureInfo );
    }


    readonly string timeFormat = @"m\:ss";
    
    string socFormat;
    string voltageFormat;
    string mahUsedFormat;
    string rpmFormat;
    string currentFormat;
    string speedFormat;
    string altitudeFormat;
    //string flytimeFormat;
    string throttleFormat;
    CultureInfo cultureInfo;
    float lastUpdateTime;
    float flytime;
    

    void Awake()
    {
        socFormat = socText.text;
        voltageFormat = voltageText.text;
        mahUsedFormat = mahUsedText.text;
        currentFormat = currentText.text;
        rpmFormat = rpmText.text;
        speedFormat = speedText.text;
        altitudeFormat = altitudeText.text;
        //flytimeFormat = flytimeText.text;
        throttleFormat = throttleText.text;
        
        cultureInfo = CultureInfo.InvariantCulture;
    }

    void Update()
    {
        if( Time.time - lastUpdateTime > 1f / updateRate )
        {
            lastUpdateTime = Time.time;
            UpdateUI();
        }

        if( flyingWing && flyingWing.TAS > 0.1f )
        {
            flytime += Time.deltaTime;
        }
    }


    void UpdateUI()
    {
        if( battery )
        {
            socText.text = battery.StateOfCharge.ToString( socFormat, cultureInfo );
            voltageText.text = battery.Voltage.ToString( voltageFormat, cultureInfo );

            var mahUsed = battery.CapacityDrawn * 1000f;
            mahUsedText.text = mahUsed.ToString( mahUsedFormat, cultureInfo );
        }

        if( motor )
        {
            rpmText.text = motor.rpm.ToString( rpmFormat, cultureInfo );
            currentText.text = motor.current.ToString( currentFormat, cultureInfo );
        }

        if( flyingWing )
        {
            speedText.text = ( flyingWing.TAS * 3.6f ).ToString( speedFormat, cultureInfo );
            altitudeText.text = flyingWing.Altitude.ToString( altitudeFormat, cultureInfo );
            flytimeText.text = TimeSpan.FromSeconds( flytime ).ToString( timeFormat, cultureInfo );
            throttleText.text = flyingWing.Throttle.ToString( throttleFormat, cultureInfo );
        }
    }
}

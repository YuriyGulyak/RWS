using System;
using UnityEngine;

public class WingPlayerControl : MonoBehaviour
{
    [SerializeField]
    FlyingWing wing = null;

    //[SerializeField]
    //RateProfile rateProfile = null;

    [SerializeField]
    float rollRate = 1f;

    [SerializeField]
    float pitchRate = 1f;

    [SerializeField]
    float pitchTrim = 0f;
    
    [SerializeField]
    float pitchTrimRate = 0.1f;

    //----------------------------------------------------------------------------------------------------

    ControllerSensitivity sensitivity;
    
    float throttle;
    float roll;
    float pitch;


    void Awake()
    {
        sensitivity = new ControllerSensitivity();
        sensitivity.LoadPlayerPrefs();
    }

    void OnEnable()
    {
        var playerInput = PlayerInputWrapper.Instance;
        if( playerInput )
        {        
            playerInput.Throttle.AddListener( SetThrottle );
            playerInput.Roll.AddListener( SetRoll );
            playerInput.Pitch.AddListener( SetPitch );
            playerInput.Trim.AddListener( SetTrim );
        }
    }

    void OnDisable()
    {
        var playerInput = PlayerInputWrapper.Instance;
        if( playerInput )
        {
            playerInput.Throttle.RemoveListener( SetThrottle );
            playerInput.Roll.RemoveListener( SetRoll );
            playerInput.Pitch.RemoveListener( SetPitch );
            playerInput.Trim.RemoveListener( SetTrim );
        }
    }


    void SetThrottle( float value )
    {
        throttle = value;
        wing.Throttle = throttle;
    }
    
    void SetRoll( float value )
    {
        roll = sensitivity.EvaluateRoll( value ) * rollRate;
        wing.RollSetpoint = roll;
    }
    
    void SetPitch( float value )
    {
        pitch = ( sensitivity.EvaluatePitch( value ) * pitchRate ) + ( pitchTrim * pitchTrimRate );
        wing.PitchSetpoint = pitch;
    }
    
    void SetTrim( float value )
    {
        pitchTrim = value;
    }
}
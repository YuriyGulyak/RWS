using RWS;
using UnityEngine;

public class WingPlayerControl : MonoBehaviour
{
    [SerializeField]
    FlyingWing wing = null;

    [SerializeField]
    float rollRate = 1f;

    [SerializeField]
    float pitchRate = 1f;

    [SerializeField]
    float pitchTrim = 0f;
    
    [SerializeField]
    float pitchTrimRate = 0.1f;

    //----------------------------------------------------------------------------------------------------

    InputManager inputManager;
    ControlSensitivity sensitivity;
    
    float throttle;
    float roll;
    float pitch;


    void OnEnable()
    {
        sensitivity = new ControlSensitivity( true );

        inputManager = InputManager.Instance;
        inputManager.ThrottleControl.Performed += SetThrottle;
        inputManager.RollControl.Performed += SetRoll;
        inputManager.PitchControl.Performed += SetPitch;
        inputManager.TrimControl.Performed += SetTrim;
    }

    void OnDisable()
    {
        inputManager.ThrottleControl.Performed -= SetThrottle;
        inputManager.RollControl.Performed -= SetRoll;
        inputManager.PitchControl.Performed -= SetPitch;
        inputManager.TrimControl.Performed -= SetTrim;
    }


    void SetThrottle( float value )
    {
        throttle = Mathf.InverseLerp( -1f, 1f, value );
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
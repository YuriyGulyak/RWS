using UnityEngine;

public class WingAutoControl : MonoBehaviour
{
    [SerializeField]
    FlyingWing flyingWing = null;

    [SerializeField]
    bool autoRoll = true;
    
    [SerializeField]
    bool autoPitch = true;
    
    [SerializeField]
    PIDController rollPID = new PIDController();
    
    [SerializeField]
    PIDController pitchPID = new PIDController();
  
    [SerializeField]
    float roll = 0f;
    
    [SerializeField]
    float pitch = 0f;
    
    [SerializeField]
    float targetAltitude = 0f;

    [SerializeField]
    float throttle = 1f;

    [SerializeField]
    float rollAngle;

    
    float rollAngleSmoothed;
    float rollVelocity;
    float pitchVelocity;

    
    
    void Start()
    {
        flyingWing.Throttle = throttle;
    }

    void FixedUpdate()
    {
        var deltaTime = Time.fixedDeltaTime;

        if( autoRoll )
        {
            rollAngle = Vector3.SignedAngle( Vector3.up, flyingWing.transform.up, flyingWing.transform.forward ) * -1f;
            rollAngleSmoothed = Mathf.SmoothDamp( rollAngleSmoothed, rollAngle, ref rollVelocity, 0.2f );
            
            roll = rollPID.UpdateState( rollAngleSmoothed, 0f, deltaTime );
            flyingWing.RollSetpoint = roll;
        }

        if( autoPitch )
        {
            pitch = pitchPID.UpdateState( flyingWing.Altitude, targetAltitude, deltaTime );
            flyingWing.PitchSetpoint = pitch;
        }
    }
}
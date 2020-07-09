using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputWrapper : Singleton<PlayerInputWrapper>
{
    [System.Serializable]
    public class FloatEvent : UnityEvent<float> { }

    public FloatEvent Throttle;
    public FloatEvent Roll;
    public FloatEvent Pitch;
    public FloatEvent Trim;
    public UnityEvent Launch;
    public UnityEvent Restart;


    public void ThrottleCallback( InputAction.CallbackContext context )
    {
        Throttle.Invoke( Mathf.InverseLerp( -1f, 1f, context.ReadValue<float>() ) );
    }

    public void RollCallback( InputAction.CallbackContext context )
    {
        Roll.Invoke( context.ReadValue<float>() );
    }

    public void PitchCallback( InputAction.CallbackContext context )
    {
        Pitch.Invoke( context.ReadValue<float>() );
    }

    public void TrimCallback( InputAction.CallbackContext context )
    {
        Trim.Invoke( context.ReadValue<float>() );
    }

    public void LaunchCallback( InputAction.CallbackContext context )
    {
        if( context.ReadValue<float>().Equals( 0f ) );
        {
            Launch.Invoke();
        }
    }

    public void RestartCallback( InputAction.CallbackContext context )
    {
        if( context.ReadValue<float>().Equals( 0f ) );
        {
            Restart.Invoke();
        }
    }
}
using UnityEngine;

public class ServoSound : MonoBehaviour
{
    [SerializeField]
    Elevon leftElevon = null;

    [SerializeField]
    Elevon rightRlevon = null;
    
    [SerializeField]
    AudioSource audioSource = null;

    [SerializeField]
    AnimationCurve volumeCurve = AnimationCurve.Linear( 0f, 0.1f, 1f, 1f );
    
    [SerializeField]
    AnimationCurve pitchCurve = AnimationCurve.Linear( 0f, 0.75f, 1f, 3.5f );

    [SerializeField, Range( 0f, 1f )]
    float soundTransition = 0f;

    //--------------------------------------------------------------------------------------------------------------
    
    public float SoundTransition
    {
        get => soundTransition;
        set
        {
            soundTransition = Mathf.Clamp01( value );
            UpdateAudioSource();
        }
    }
    
    //--------------------------------------------------------------------------------------------------------------

    float leftElevonAngleLast;
    float rightElevonAngleLast;

    
    void OnValidate()
    {
        UpdateAudioSource();
    }

    void Update()
    {
        var deltaTime = Time.deltaTime;
        
        var leftElevonAngle = leftElevon.Angle;
        var leftElevonAngleDelta = leftElevonAngle - leftElevonAngleLast;
        leftElevonAngleLast = leftElevonAngle;
        var leftElevonSpeed = leftElevonAngleDelta / deltaTime;
        
        var rightElevonAngle = rightRlevon.Angle;
        var rightElevonAngleDelta = rightElevonAngle - rightElevonAngleLast;
        rightElevonAngleLast = rightElevonAngle;
        var rightElevonSpeed = rightElevonAngleDelta / deltaTime;

        var elevonSpeedAbs = Mathf.Max( Mathf.Abs( leftElevonSpeed ), Mathf.Abs( rightElevonSpeed ) );
        if( elevonSpeedAbs < 1f )
        {
            SoundTransition = 0f;
        }
        else
        {
            SoundTransition = elevonSpeedAbs / 300f;
        }
    }
    
    //--------------------------------------------------------------------------------------------------------------
    
    void UpdateAudioSource()
    {
        audioSource.volume = volumeCurve.Evaluate( soundTransition );
        audioSource.pitch = pitchCurve.Evaluate( soundTransition );
    }
}
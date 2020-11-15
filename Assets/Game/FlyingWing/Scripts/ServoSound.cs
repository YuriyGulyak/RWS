using RWS;
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
    
    [SerializeField]
    float volumeScale = 1f;
    
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
    
    void OnValidate()
    {
        UpdateAudioSource();
    }

    void OnEnable()
    {
        if( SoundManager )
        {
            volumeScale = SoundManager.ServoVolume * SoundManager.MasterVolume;
            UpdateAudioSource();

            SoundManager.OnServoVolumeChanged += OnManagerVolumeChanged;
        }
        
        audioSource.Play();
    }

    void OnDisable()
    {
        if( SoundManager )
        {
            SoundManager.OnServoVolumeChanged -= OnManagerVolumeChanged;
        }
        
        audioSource.Stop();
    }
    
    void Update()
    {
        var deltaTime = Time.deltaTime;
        
        var leftElevonAngleDelta = leftElevon.Angle - leftElevonAngleLast;
        leftElevonAngleLast = leftElevon.Angle;
        var leftElevonSpeed = leftElevonAngleDelta / deltaTime;

        var rightElevonAngleDelta = rightRlevon.Angle - rightElevonAngleLast;
        rightElevonAngleLast = rightRlevon.Angle;
        var rightElevonSpeed = rightElevonAngleDelta / deltaTime;

        var elevonSpeedAbs = Mathf.Max( Mathf.Abs( leftElevonSpeed ), Mathf.Abs( rightElevonSpeed ) ); // Degrees per second
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
    
    float leftElevonAngleLast;
    float rightElevonAngleLast;
    
    
    SoundManager SoundManager => SoundManager.Instance;
    
    void OnManagerVolumeChanged( float newServoVolume, float masterVolume )
    {
        volumeScale = newServoVolume * masterVolume;
        UpdateAudioSource();
    }


    void UpdateAudioSource()
    {
        audioSource.volume = volumeCurve.Evaluate( soundTransition ) * volumeScale;
        audioSource.pitch = pitchCurve.Evaluate( soundTransition );
    }
}
using UnityEngine;

namespace RWS
{
    public class WindSound : MonoBehaviour
    {
        [SerializeField]
        Rigidbody _rigidbody = null;

        [SerializeField]
        AudioSource audioSource = null;

        [SerializeField]
        WindSoundSettings windSoundSettings = null;

        // Serialized for sound tests
        [SerializeField, Range( 0f, 1f )]
        float soundTransition = 0f;

        //--------------------------------------------------------------------------------------------------------------

        void OnValidate()
        {
            if( !_rigidbody )
            {
                _rigidbody = GetComponentInParent<Rigidbody>();
            }

            UpdateAudioSource();
        }

        void Update()
        {
            soundTransition = CalcSpeedKmh() / windSoundSettings.maxSpeedKmh;
            soundTransition = Mathf.Clamp01( soundTransition );
            
            UpdateAudioSource();
        }

        //--------------------------------------------------------------------------------------------------------------

        void UpdateAudioSource()
        {
            audioSource.volume = windSoundSettings.volumeCurve.Evaluate( soundTransition );
            audioSource.pitch = windSoundSettings.pitchCurve.Evaluate( soundTransition );
        }

        float CalcSpeedKmh()
        {
            return _rigidbody.velocity.magnitude * 3.6f;
        }
    }
}
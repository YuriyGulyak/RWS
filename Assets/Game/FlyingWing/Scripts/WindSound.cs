﻿using UnityEngine;

namespace RWS
{
    public class WindSound : MonoBehaviour
    {
        [SerializeField]
        FlyingWing flyingWing = null;

        [SerializeField]
        AudioSource audioSource = null;

        [SerializeField]
        AnimationCurve volumeCurve = AnimationCurve.Linear( 0f, 0.1f, 1f, 1f );

        [SerializeField]
        AnimationCurve pitchCurve = AnimationCurve.Linear( 0f, 0.75f, 1f, 3.5f );

        // Serialized for sound debug
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

        void Start()
        {
            soundManager = SoundManager.Instance;
            
            if( soundManager )
            {
                volumeScale = soundManager.WindVolume * soundManager.MasterVolume;
                UpdateAudioSource();

                soundManager.OnWindVolumeChanged += OnManagerVolumeChanged;
            }

            audioSource.Play();
        }

        void OnDisable()
        {
            if( soundManager )
            {
                soundManager.OnWindVolumeChanged -= OnManagerVolumeChanged;
            }

            audioSource.Stop();
        }

        void Update()
        {
            SoundTransition = flyingWing.TAS / 160f;
        }

        //--------------------------------------------------------------------------------------------------------------

        SoundManager soundManager;

        void OnManagerVolumeChanged( float newWindVolume, float masterVolume )
        {
            volumeScale = newWindVolume * masterVolume;
            UpdateAudioSource();
        }

        void UpdateAudioSource()
        {
            audioSource.volume = volumeCurve.Evaluate( soundTransition ) * volumeScale;
            audioSource.pitch = pitchCurve.Evaluate( soundTransition );
        }
    }
}
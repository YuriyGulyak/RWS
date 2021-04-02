using UnityEngine;

namespace RWS
{
    public class BuzzerSound : MonoBehaviour
    {
        [SerializeField]
        FlyingWing flyingWing = null;

        [SerializeField]
        AudioSource audioSource = null;

        [SerializeField]
        float volumeScale = 1f;

        [SerializeField]
        float updateRate = 10f;

        //--------------------------------------------------------------------------------------------------------------

        void OnValidate()
        {
            audioSource.volume = volumeScale;
        }

        void OnEnable()
        {
            if( SoundManager )
            {
                volumeScale = SoundManager.BuzzerVolume * SoundManager.MasterVolume;
                SoundManager.OnBuzzerVolumeChanged += OnManagerVolumeChanged;
            }
        }

        void OnDisable()
        {
            if( SoundManager )
            {
                SoundManager.OnBuzzerVolumeChanged -= OnManagerVolumeChanged;
            }
        }

        void Awake()
        {
            customUpdate = new CustomUpdate( updateRate );
            customUpdate.OnUpdate += OnUpdate;
        }

        void Update()
        {
            customUpdate.Update( Time.time );
        }

        //--------------------------------------------------------------------------------------------------------------

        CustomUpdate customUpdate;

        SoundManager SoundManager => SoundManager.Instance;

        void OnManagerVolumeChanged( float newBuzzerVolume, float masterVolume )
        {
            volumeScale = newBuzzerVolume * masterVolume;
            audioSource.volume = volumeScale;
        }

        void OnUpdate( float deltaTime )
        {
            if( flyingWing.Battery.VoltageStatus == VoltageStatus.Critical )
            {
                if( !audioSource.isPlaying )
                {
                    audioSource.Play();
                }
            }
            else
            {
                if( audioSource.isPlaying )
                {
                    audioSource.Stop();
                }
            }
        }
    }
}
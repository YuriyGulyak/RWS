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

        void Start()
        {
            soundManager = SoundManager.Instance;
            
            if( soundManager )
            {
                volumeScale = soundManager.BuzzerVolume * soundManager.MasterVolume;
                soundManager.OnBuzzerVolumeChanged += OnManagerVolumeChanged;
            }
        }

        void OnDisable()
        {
            if( soundManager )
            {
                soundManager.OnBuzzerVolumeChanged -= OnManagerVolumeChanged;
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
        SoundManager soundManager;

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
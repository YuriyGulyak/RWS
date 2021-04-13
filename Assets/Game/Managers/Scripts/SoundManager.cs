using UnityEngine;
using UnityEngine.Audio;

namespace RWS
{
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField]
        AudioMixer audioMixer;


        public float MasterVolume
        {
            get => masterVolume;
            set
            {
                masterVolume = Mathf.Clamp01( value );
                audioMixer.SetFloat( MASTER_VOLUME_KEY, LinearToLogarithmicScale( masterVolume ) );
            }
        }

        public float MotorVolume
        {
            get => motorVolume;
            set
            {
                motorVolume = Mathf.Clamp01( value );
                audioMixer.SetFloat( MOTOR_VOLUME_KEY, LinearToLogarithmicScale( motorVolume ) );
            }
        }

        public float ServoVolume
        {
            get => servoVolume;
            set
            {
                servoVolume = Mathf.Clamp01( value );
                audioMixer.SetFloat( SERVO_VOLUME_KEY, LinearToLogarithmicScale( servoVolume ) );
            }
        }
        
        public float BuzzerVolume
        {
            get => buzzerVolume;
            set
            {
                buzzerVolume = Mathf.Clamp01( value );
                audioMixer.SetFloat( BUZZER_VOLUME_KEY, LinearToLogarithmicScale( buzzerVolume ) );
            }
        }
        
        public float WindVolume
        {
            get => windVolume;
            set
            {
                windVolume = Mathf.Clamp01( value );
                audioMixer.SetFloat( WIND_VOLUME_KEY, LinearToLogarithmicScale( windVolume ) );
            }
        }


        public void LoadPlayerPrefs()
        {
            MasterVolume = PlayerPrefs.GetFloat( MASTER_VOLUME_KEY, 1f );
            MotorVolume = PlayerPrefs.GetFloat( MOTOR_VOLUME_KEY, 1f );
            ServoVolume = PlayerPrefs.GetFloat( SERVO_VOLUME_KEY, 1f );
            BuzzerVolume = PlayerPrefs.GetFloat( BUZZER_VOLUME_KEY, 1f );
            WindVolume = PlayerPrefs.GetFloat( WIND_VOLUME_KEY, 1f );
        }

        public void SavePlayerPrefs()
        {
            PlayerPrefs.SetFloat( MASTER_VOLUME_KEY, masterVolume );
            PlayerPrefs.SetFloat( MOTOR_VOLUME_KEY, motorVolume );
            PlayerPrefs.SetFloat( SERVO_VOLUME_KEY, servoVolume );
            PlayerPrefs.SetFloat( BUZZER_VOLUME_KEY, buzzerVolume );
            PlayerPrefs.SetFloat( WIND_VOLUME_KEY, windVolume );
        }


        const string MASTER_VOLUME_KEY = "MasterVolume";
        const string MOTOR_VOLUME_KEY = "MotorVolume";
        const string SERVO_VOLUME_KEY = "ServoVolume";
        const string BUZZER_VOLUME_KEY = "BuzzerVolume";
        const string WIND_VOLUME_KEY = "WindVolume";
        
        float masterVolume;
        float motorVolume;
        float servoVolume;
        float buzzerVolume;
        float windVolume;


        void Start()
        {
            LoadPlayerPrefs();
        }


        static float LinearToLogarithmicScale( float linearScale )
        {
            linearScale = Mathf.Clamp( linearScale, 0.0001f, 1f );
            //return Mathf.Log( linearScale ) * 20f;
            return Mathf.Log10( linearScale ) * 20f;
        }
    }
}
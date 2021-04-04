using System;
using UnityEngine;

namespace RWS
{
    public class SoundManager : Singleton<SoundManager>
    {
        public Action<float, float> OnMotorVolumeChanged;
        public Action<float, float> OnServoVolumeChanged;
        public Action<float, float> OnBuzzerVolumeChanged;
        public Action<float, float> OnWindVolumeChanged;
        
        public float MasterVolume
        {
            get => masterVolume;
            set
            {
                masterVolume = Mathf.Clamp01( value );
                OnMotorVolumeChanged?.Invoke( motorVolume, masterVolume );
                OnServoVolumeChanged?.Invoke( servoVolume, masterVolume );
                OnWindVolumeChanged?.Invoke( windVolume, masterVolume );
            }
        }

        public float MotorVolume
        {
            get => motorVolume;
            set
            {
                motorVolume = Mathf.Clamp01( value );
                OnMotorVolumeChanged?.Invoke( motorVolume, masterVolume );
            }
        }

        public float ServoVolume
        {
            get => servoVolume;
            set
            {
                servoVolume = Mathf.Clamp01( value );
                OnServoVolumeChanged?.Invoke( servoVolume, masterVolume );
            }
        }
        
        public float BuzzerVolume
        {
            get => buzzerVolume;
            set
            {
                buzzerVolume = Mathf.Clamp01( value );
                OnBuzzerVolumeChanged?.Invoke( buzzerVolume, masterVolume );
            }
        }
        
        public float WindVolume
        {
            get => windVolume;
            set
            {
                windVolume = Mathf.Clamp01( value );
                OnWindVolumeChanged?.Invoke( windVolume, masterVolume );
            }
        }


        public void LoadPlayerPrefs()
        {
            MasterVolume = PlayerPrefs.GetFloat( masterVolumeKey, 1f );
            MotorVolume = PlayerPrefs.GetFloat( motorVolumeKey, 1f );
            ServoVolume = PlayerPrefs.GetFloat( servoVolumeKey, 1f );
            BuzzerVolume = PlayerPrefs.GetFloat( buzzerVolumeKey, 1f );
            WindVolume = PlayerPrefs.GetFloat( windVolumeKey, 1f );
        }

        public void SavePlayerPrefs()
        {
            PlayerPrefs.SetFloat( masterVolumeKey, masterVolume );
            PlayerPrefs.SetFloat( motorVolumeKey, motorVolume );
            PlayerPrefs.SetFloat( servoVolumeKey, servoVolume );
            PlayerPrefs.SetFloat( buzzerVolumeKey, buzzerVolume );
            PlayerPrefs.SetFloat( windVolumeKey, windVolume );
        }
    
        //--------------------------------------------------------------------------------------------------------------
    
        readonly string masterVolumeKey = "MasterVolume";
        readonly string motorVolumeKey = "MotorVolume";
        readonly string servoVolumeKey = "ServoVolume";
        readonly string buzzerVolumeKey = "BuzzerVolume";
        readonly string windVolumeKey = "WindVolume";
    
        float masterVolume;
        float motorVolume;
        float servoVolume;
        float buzzerVolume;
        float windVolume;


        void Awake()
        {
            LoadPlayerPrefs();
        }
    }
}
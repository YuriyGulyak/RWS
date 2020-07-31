using System;
using UnityEngine;

namespace RWS
{
    public class SoundManager : Singleton<SoundManager>
    {
        public Action<float> OnMasterVolumeChanged;
        public Action<float> OnMotorVolumeChanged;
        public Action<float> OnWindVolumeChanged;


        public float MasterVolume
        {
            get => masterVolume;
            set
            {
                masterVolume = Mathf.Clamp01( value );
                OnMasterVolumeChanged?.Invoke( masterVolume );
            }
        }

        public float MotorVolume
        {
            get => motorVolume;
            set
            {
                motorVolume = Mathf.Clamp01( value );
                OnMotorVolumeChanged?.Invoke( motorVolume );
            }
        }

        public float WindVolume
        {
            get => windVolume;
            set
            {
                windVolume = Mathf.Clamp01( value );
                OnWindVolumeChanged?.Invoke( windVolume );
            }
        }


        public void LoadPlayerPrefs()
        {
            MasterVolume = PlayerPrefs.GetFloat( masterVolumeKey, 1f );
            MotorVolume = PlayerPrefs.GetFloat( masterVolumeKey, 1f );
            WindVolume = PlayerPrefs.GetFloat( masterVolumeKey, 1f );
        }

        public void SavePlayerPrefs()
        {
            PlayerPrefs.SetFloat( masterVolumeKey, masterVolume );
            PlayerPrefs.SetFloat( motorVolumeKey, motorVolume );
            PlayerPrefs.SetFloat( windVolumeKey, windVolume );
        }
    
        //--------------------------------------------------------------------------------------------------------------
    
        readonly string masterVolumeKey = "MasterVolume";
        readonly string motorVolumeKey = "MotorVolume";
        readonly string windVolumeKey = "WindVolume";
    
        float masterVolume;
        float motorVolume;
        float windVolume;


        void Awake()
        {
            LoadPlayerPrefs();
        }
    }
}